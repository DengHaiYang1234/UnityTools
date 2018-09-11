using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AutoBuildCodeTemp
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
	//autoEnd
}
";


}


