//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using UnityEngine;

/// <summary>
/// ugui版本的TweenScale插值动画组件 Tween the object's local scale.
/// </summary>

[AddComponentMenu("UGUI/Tween/UGUI Tween Scale")]
public class UGUITweenScale : UITweener
{
    public Vector3 from = Vector3.one;
    public Vector3 to = Vector3.one;
    public bool updateTable = false;

    RectTransform mTrans;
    UILayout mLayout;

    public RectTransform cachedTransform { get { if (mTrans == null) mTrans = GetComponent<RectTransform>(); return mTrans; } set { mTrans = value; } }

    public Vector3 value { get { return cachedTransform.localScale; } set { cachedTransform.localScale = value; } }

    [System.Obsolete("Use 'value' instead")]
    public Vector3 scale { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;

        if (updateTable)
        {
            if (mLayout == null)
            {
                mLayout = NGUITools.FindInParents<UILayout>(gameObject);
                if (mLayout == null) { updateTable = false; return; }
            }
            mLayout.repositionNow = true;
        }
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public UGUITweenScale Begin(GameObject go, float duration, Vector3 scale)
    {
        UGUITweenScale comp = UITweener.Begin<UGUITweenScale>(go, duration);
        comp.from = comp.value;
        comp.to = scale;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
    ///只给剧情编辑器使用
    public void CustomBegin()
    {
        UGUITweenScale comp = UITweener.Begin<UGUITweenScale>(this.gameObject, duration);
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }
}
