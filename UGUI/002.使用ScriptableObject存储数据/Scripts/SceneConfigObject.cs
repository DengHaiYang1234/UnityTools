using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用ScriptableObject一定要记得序列化.不然会报空引用的错误
/// </summary>
[SerializeField]
public class SceneConfigObject : ScriptableObject
{
	public string mIndex;
	public Vector3 sqawnPos;
}
