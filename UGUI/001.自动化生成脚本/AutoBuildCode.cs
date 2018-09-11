using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;


#region 脚本解析
//AutoBuildTemplate类

//Unity中的C#脚本代码本质上为包含字符串内容的文本文件，以.cs后缀保存。因此我们如果要自动生成脚本只用编辑好代码的文本内容，然后添加文件后缀保存文件就完成了。
//我们替换其中的 #类名#、#查找#、#成员#，保存成xxx.cs的文件就可以生成一个脚本类出来。


//AutoBuild类

//GameObject[] selectobjs = Selection.gameObjects;
//Unity中可以在Editor脚本调用Selection类得到当前选中的物体。因为存在多选情况，返回的物体为一个数组。

//var dicUIType = new Dictionary<string, string>();
//dicUIType.Add("Img", "Image");
//dicUIType.Add("Btn", "Button");
//dicUIType.Add("Txt", "Text");
//dicUIType.Add("Tran", "Transform");

//外部按钮、图片、文本等组件物体的关键字与类型的映射，当子物体中名字存在“Img”、“Btn”就识别为Image和Button。


//var mainNode = from trans in childList where trans.name.Contains('_') && dicUIType.Keys.Contains(trans.name.Split('_')[0]) select trans;

//Linq的from 、where  、select 语句遍历寻找出前缀存在字典映射中的物体。
///*查询是一种从数据源检索数据的表达式。 查询通常用专门的查询语言来表示。 随着时间的推移，人们已经为各种数据源开发了不同的语言；例如，用于关系数据库的 SQL 和用于 XML 的 XQuery。 因此，开发人员对于他们必须支持的每种数据源或数据格式，都不得不学习一种新的查询语言。 LINQ 通过提供一种跨各种数据源和数据格式使用数据的一致模型，简化了这一情况。 在 LINQ 查询中，始终会用到对象。 可以使用相同的基本编码模式来查询和转换 XML 文档、SQL 数据库、http://ADO.NET 数据集、.NET 集合中的数据以及对其有 LINQ 提供程序可用的任何其他格式的数据。
//资料地址https://docs.microsoft.com/zh-cn/dotnet/csharp/linq/linq-in-csharp*/


//string unchangeStr = Regex.Split(classStr, splitStr, RegexOptions.IgnoreCase)[0];

//正则表达式去分割字符串，因为当脚本已经存在，我们不能覆盖掉已经书写的代码，所以基础文本中有一个//auto来分割自动生成代码区域和手写区域。


//classStr = classStr.Replace("#类名#", selectobj.name);

//字符串替换功能，将基础文本中的关键字替换。




//FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
//StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
//fileW.Write(classStr);
//fileW.Flush();
//fileW.Close();
//file.Close();
//AssetDatabase.SaveAssets();
//AssetDatabase.Refresh();


//创建流文件，当写入完成后关闭流。在Unity生成了物体必须调用 AssetDatabase.SaveAssets()和AssetDatabase.Refresh()才能即时的看到资源刷新。
#endregion

public class AutoBuildCode
{
	[MenuItem("生成/创建或刷新界面")]
	public static void BuildUIScript()
	{
		var dicUIType = new Dictionary<string, string>();
		dicUIType.Add("Img", "Image");
		dicUIType.Add("Btn", "Button");
		dicUIType.Add("Txt", "Text");
		dicUIType.Add("Tran", "Transform");



		GameObject[] selectObjs = Selection.gameObjects;
		foreach(GameObject go in selectObjs)
		{
			//transform.root  返回最高级
			GameObject selectObj = go.transform.root.gameObject;

			Transform[] _trans = selectObj.GetComponentsInChildren<Transform>(true);

			List<Transform> childList = new List<Transform>(_trans);

			var mainNode = from trans in childList where trans.name.Contains('_') &&
   dicUIType.Keys.Contains(trans.name.Split('_')[0]) select trans;

			var nodePathList = new Dictionary<string, string>();

			foreach(Transform node in mainNode)
			{
				Transform tempNode = node;
				string nodePath = "/" + tempNode.name;
				while(tempNode != tempNode.root)
				{
					tempNode = tempNode.parent;
					int index = nodePath.IndexOf('/');
					nodePath = nodePath.Insert(index, "/" + tempNode.name);
				}
				nodePathList.Add(node.name, nodePath);
			}

			string memberstring = "";

			string loadedcontant = "";

			foreach(Transform itemtran in mainNode)
			{
				string typeStr = dicUIType[itemtran.name.Split('_')[0]];

				memberstring += "public " + typeStr + " " + itemtran.name + " = null;\r\n\t";

				loadedcontant += itemtran.name + " = " + "gameObject.transform.Find(\"" + nodePathList[itemtran.name] + "\").GetComponent<" + typeStr + ">();\r\n\t\t";

			}

			string scriptPath = Application.dataPath + "/Scripts/" + selectObj.name + ".cs";

			string classStr = "";

			if(File.Exists(scriptPath))
			{
				FileStream classFile = new FileStream(scriptPath, FileMode.Open);
				StreamReader read = new StreamReader(classFile);
				classStr = read.ReadToEnd();
				read.Close();
				classFile.Close();
				File.Delete(scriptPath);

				string splitStr = "//auto";
				string unchangeStr = Regex.Split(classStr, splitStr, RegexOptions.IgnoreCase)[0];

				string changeStr = Regex.Split(AutoBuildCodeTemp.UIClass, splitStr, RegexOptions.IgnoreCase)[1];

				StringBuilder build = new StringBuilder();
				build.Append(unchangeStr);
				build.Append(splitStr);
				build.Append(changeStr);
				classStr = build.ToString();

			}
			else
			{
				classStr = AutoBuildCodeTemp.UIClass;
			}

			classStr = classStr.Replace("#类名#", selectObj.name);
			classStr = classStr.Replace("#查找#", loadedcontant);
			classStr = classStr.Replace("#成员#", memberstring);

			FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
			StreamWriter fileSW = new StreamWriter(file, System.Text.Encoding.UTF8);
			fileSW.Write(classStr);
			fileSW.Flush();
			fileSW.Close();
			file.Close();


			Debug.LogError("创建脚本" + Application.dataPath + "/Scripts/" + selectObj.name
				+ ".cs成功！");

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

		}
	}
}

#region 总结
//以上利用UI预制体代码自动生成为例讲解了自动化生成的方法，我这里是通过Find来查找物体引用的，当然可以利用Unity序列化参数的方法来赋值（就是拖拽操作的赋值方法），用后者可以节约UI第一次打开的性能（毕竟Unity的Find还是很消耗性能的），
//可以在我们的脚本创建好后加入给预制体挂载脚本赋值的功能流程。
#endregion
