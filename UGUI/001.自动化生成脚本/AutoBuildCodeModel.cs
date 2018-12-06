using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBuildCodeModel
{
    public static string UIClass =
        @"using UnityEngine;
          using UnityEngine.UI;
          using UnityEngine.EventSystems;
          using System;


        public class #类名#:MonoBehaviour
            {
	            //auto
	            #成员#
	            public void Start()
	                {
		                #查找#
	                }


                    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {
        base.OnResume();
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
    }

	            //autoEnd
            }
            ";

}
