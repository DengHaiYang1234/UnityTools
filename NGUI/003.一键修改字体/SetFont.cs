using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class SetFont : MonoBehaviour
{
    public UIFont UYFont;    //注：UIFont为项目所使用的字体，Font为Unity自带的字体（使用会增加内存）.
    public void UpdateFont(List<UILabel> sets)
    {
        print(sets.Count);
        for (int i = 0; i < sets.Count; i++)
        {
            string str = sets[i].bitmapFont == null ? "null" : sets[i].trueTypeFont.name;
            print(sets[i].name + ":" + str);
            sets[i].bitmapFont = UYFont;
        }
    }
}