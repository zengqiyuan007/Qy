//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// 拓展Ngui组件的UIWidgetContainer,可对子元素重排位置
/// </summary>
public abstract class UILayout : UIWidgetContainer
{
	/// <summary>
	/// 立即重排
	/// </summary>
	public abstract void Reposition();

	/// <summary>
	/// 设置是否需要重排
	/// </summary>
	public abstract bool repositionNow
	{
		set;
	}
}
