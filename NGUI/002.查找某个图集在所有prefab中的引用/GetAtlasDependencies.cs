using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
public class GetAtlasDependencies : MonoBehaviour
{
    //prefab目录
    const string TargerPath = "/prefabs/Game";
    //选择的资源路径
    public static string ClickAssetsPath;

    //prefab  相对路径
    public static string sourcePath;

    //选中资源的名称
    static string TargetName;

    //存储所找到的所有prefabs名称
    public static List<string> allPrefabsNames;

    //资源目录
    public static string DirectoryName;

    //资源的所有组件
    public static List<Transform> chindernList = new List<Transform>();
    //入口   选择图集并指定查找资源的目录
    [MenuItem("Assets/FindAtlasRefrenceInPrefabs")]
    static void FindAtlasRefrenceInPrefab()
    {
        UnityEngine.Object selectAssets = Selection.activeObject;
        ClickAssetsPath = AssetDatabase.GetAssetPath(selectAssets);
        TargetName = selectAssets.name;
        allPrefabsNames = new List<string>();
        allPrefabsNames.Clear();
        if (!ClickAssetsPath.EndsWith(".png"))
        {
            Debug.Log("请选择图集");
            return;
        }

        sourcePath = Application.dataPath + TargerPath;
        Pack(sourcePath);
    }
    //递归查找目录中的资源Obj
    static void Pack(string source)
    {
        DirectoryInfo folder = new DirectoryInfo(source);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int i = 0; i < length; i++)
        {
            if (files[i] is DirectoryInfo)
            {
                DirectoryName = files[i].FullName;
                Pack(files[i].FullName);
            }
            else
            {
                if (!files[i].Name.EndsWith(".meta"))
                {
                    File(files[i].Name, DirectoryName);
                    SetProgress((float)i / (float)files.Length, files.Length, i, files[i].Name);
                }
            }
        }
        ViewByScriptableObject();
        EditorUtility.ClearProgressBar();
    }
    //资源处理并获取数据
    static void File(string prefabName, string DirectoryName)
    {
        string path = DirectoryName + @"\" + prefabName;
        path = path.Replace("\\", "/");
        path = path.Substring(Application.dataPath.Length);
        path = "Assets" + path;
        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GetAllTransform(obj.transform);
        int pos = prefabName.IndexOf(".");
        prefabName = prefabName.Substring(0, pos);
        for (int i = 0; i < chindernList.Count; i++)
        {
            UISprite _sprite;
            if ((_sprite = chindernList[i].GetComponent<UISprite>()) && (_sprite.atlas) && (_sprite.spriteName != null))
            {
                if (_sprite.atlas.name == TargetName)
                {
                    if (!allPrefabsNames.Contains(prefabName))
                    {
                        allPrefabsNames.Add(prefabName);
                    }
                }
            }
        }
        chindernList.Clear();
    }

    //递归查找该obj的下的所有节点并保存
    static List<Transform> GetAllTransform(Transform parent)
    {
        foreach (Transform child in parent)
        {
            chindernList.Add(child);
            if (child.childCount > 0)
                GetAllTransform(child);
        }
        return null;
    }
    //保存至容器并显示
    static void ViewByScriptableObject()
    {
        SetPrefabsInfos _infos = ScriptableObject.CreateInstance<SetPrefabsInfos>();
        foreach (var v in allPrefabsNames)
        {
            _infos.prefabName.Add(v);
        }
        AssetDatabase.CreateAsset(_infos, ClickAssetsPath.Replace(".png", "_DependenInfos.prefab"));
        AssetDatabase.Refresh();
    }
    static void SetProgress(float value, int total, int cur, string prefabName)
    {
        EditorUtility.DisplayProgressBar("查找预制体中...", string.Format("请稍等{0}/{1}  ({2})", cur, total, prefabName), value);
    }
}
[Serializable]
public class SetPrefabsInfos : ScriptableObject
{
    public List<string> prefabName = new List<string>();
}