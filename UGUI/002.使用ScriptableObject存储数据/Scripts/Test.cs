using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获取资源的相关属性
/// </summary>
public class Test : MonoBehaviour
{
	private void OnGUI()
	{
		if(GUILayout.Button("获取资源"))
		{
			var config = Instantiate(Resources.Load("config/2") as SceneConfigObject);
			Debug.Log(config.mIndex);
			Debug.Log(config.sqawnPos);
		}
	}
}
