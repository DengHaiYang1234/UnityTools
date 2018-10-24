using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class GetUseAtlasName
{
	static GameObject targetObj;
	static List<string> names = new List<string>();
	static Dictionary<string, List<string>> atlasInfos = new Dictionary<string, List<string>>();
	static List<Transform> chindernList = new List<Transform>();
	static Transform GetAllTransform(Transform parent)
	{
		foreach (Transform child in parent)
		{
			chindernList.Add(child);
			if (child.childCount > 0)
				GetAllTransform(child);
		}
		return null;
	}
	[MenuItem("Assets/GetAtlasInfos")]
	static void ShowTextCount()
	{
		targetObj = Selection.activeGameObject;//得到选中对象
		string path = AssetDatabase.GetAssetPath(targetObj);
		if (targetObj)
		{
			chindernList.Clear();
			names.Clear();
			atlasInfos.Clear();
			GetAllTransform(targetObj.transform);//递归得到所有的子对象
			GetUseAtlasNames atlass = ScriptableObject.CreateInstance<GetUseAtlasNames>();
			atlass.obj.Clear();

            
          


            for (int i = 0; i < chindernList.Count; i++)
			{
                
				UISprite _sprite;



                if ((_sprite = chindernList[i].GetComponent<UISprite>()) && (_sprite.atlas) && (_sprite.spriteName != null))
				{
					if (!atlasInfos.ContainsKey(_sprite.atlas.name))
					{
						atlasInfos.Add(_sprite.atlas.name, new List<string> { _sprite.spriteName });
					}
					else if (atlasInfos.ContainsKey(_sprite.atlas.name))
					{
						if (!atlasInfos[_sprite.atlas.name].Contains(_sprite.spriteName))
						{
							atlasInfos[_sprite.atlas.name].Add(_sprite.spriteName);
						}
					}
				}
			}

			//存储数据  可视化显示
			foreach (var infos in atlasInfos)
			{
				GetUseAtlasInfos uInfos = new GetUseAtlasInfos() { AtlasName = infos.Key, spriteNames = infos.Value };
				atlass.obj.Add(uInfos);
			}
			AssetDatabase.CreateAsset(atlass, path.Replace(".prefab", "_Atlas.prefab"));
			AssetDatabase.Refresh();
			atlasInfos.Clear();
		}
	}
}
[Serializable]
public class GetUseAtlasInfos
{
	public string AtlasName;
	public List<string> spriteNames;
}
public class GetUseAtlasNames : ScriptableObject
{
	public List<GetUseAtlasInfos> obj = new List<GetUseAtlasInfos>();
}