using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
public class ToolsForSetSpriteType
{
    [MenuItem("Tools/ChangeSpriteAndCreatUIAtals")]
    static void ChangeSprite()
    {
        Object[] textures = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        string sourcePath = AssetDatabase.GetAssetPath(Selection.activeObject);
        Pack(textures);  //设置图片type
    }
    static void Pack(Object[] textures)
    {
        if (textures != null)
        {
            if (textures.Length < 1)
            {
                Debug.Log("找不到图片!!");
                return;
            }
        }
        else
        {
            Debug.Log("请选择正确的文件目录!!!!");
            return;
        }
        int i = 0;
        foreach (Texture texture in textures)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImport = AssetImporter.GetAtPath(path) as TextureImporter;
            textureImport.textureType = TextureImporterType.Sprite;
            textureImport.spriteImportMode = SpriteImportMode.Multiple;
            SetProgress((float)i / (float)textures.Length, textures.Length, i);
            i++;
            AssetDatabase.ImportAsset(path);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }
    static void SetProgress(float value, int total, int cur)
    {
        EditorUtility.DisplayProgressBar("设置图片中...", string.Format("请稍等{0}/{1}", cur, total), value);
    }
}