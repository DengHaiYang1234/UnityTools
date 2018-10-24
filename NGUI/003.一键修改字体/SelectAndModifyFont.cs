using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
public class SelectAndModifyFont : MonoBehaviour
{
    static GameObject targetObj;
    static string ChangeFont1 = "cg_w9";//要替换的字体
    static string ChangeFont2 = "cg_w7";//要替换的字体
                                        //可以更改字体的obj列表
    static List<UILabel> waiteForUpdate = new List<UILabel>();
    //所有子物体
    static List<Transform> chindernList = new List<Transform>();
    //递归找到所有子节点
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
    [MenuItem("FontEdit/SelectTarget")]
    static void ShowTextCount()
    {
        targetObj = Selection.activeGameObject;//得到选中对象
        if (targetObj)
        {
            bool isok = false;
            chindernList.Clear();
            GetAllTransform(targetObj.transform);//递归得到所有的子对象
            for (int i = 0; i < chindernList.Count; i++)
            {
                //获取obj，并保存
                UILabel _Text = null;
                if ((_Text = chindernList[i].GetComponent<UILabel>()) && (_Text.bitmapFont) && (_Text.bitmapFont.name == ChangeFont1 || _Text.bitmapFont.name == ChangeFont2))
                {
                    waiteForUpdate.Add(chindernList[i].GetComponent<UILabel>());
                    isok = true;
                }
            }
            if (isok)
            {
                if (!targetObj.GetComponent<SetFont>())
                {
                    targetObj.AddComponent<SetFont>();
                }
                SetFont set = targetObj.GetComponent<SetFont>();//为对象添加个组件
                set.UpdateFont(waiteForUpdate);
                chindernList.Clear();
                waiteForUpdate.Clear();
                GameObject.DestroyImmediate(targetObj.GetComponent<SetFont>());//修改完毕移除组件
            }
        }
    }
}