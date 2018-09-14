using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;
public class AnimtorErrorTool : Editor
{
	public static string modePrefabsPath = "Assets/Resources/animator";

	[MenuItem("Tools/动画查错",priority = 0)]
	public static void FreshAnimtor()
	{
		if(!Directory.Exists(modePrefabsPath))
		{
			if(EditorUtility.DisplayDialog("目录不存在","请确认路径是否正确","继续"))
			{
				return;
			}
		}


		FileInfo[] modeDirect = new DirectoryInfo(modePrefabsPath).GetFiles();

		if(modeDirect.Length == 0)
		{
			if(EditorUtility.DisplayDialog("目录为空","请向目录中添加动画文件","继续"))
			{
				return;
			}
		}

		for(int i = 0; i< modeDirect.Length;i++)
		{
			if(modeDirect[i].Name.Contains("meta"))
			{
				continue;
			}

			string modeName = modeDirect[i].Name;

			string animtorPath = modePrefabsPath + modeName;

			AnimatorController AnimatorTemplate =
				AssetDatabase.LoadAssetAtPath(animtorPath, typeof(AnimatorController))
				as AnimatorController;

			if(AnimatorTemplate == null)
			{
				//错误提示面板
				if (EditorUtility.DisplayDialog("错误的路径", "寻求动画路径失败："
					+ animtorPath + ",检查动画控制器的名字是否和模型名字匹配", "继续"))
				{
					return;
				}
			}

			foreach(var obj in AnimatorTemplate.layers[0].stateMachine.states)
			{
				if(obj.state.motion == null)
				{
					if(EditorUtility.DisplayDialog("存在空的动画Clip","动画" + animtorPath +
						 "的状态" + obj.state.name + "为空","继续"))
					{
						continue;
					}
				}
			}
		}
	}
}
