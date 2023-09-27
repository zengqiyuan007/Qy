//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ugui版本的TweenPosition插值动画组件 Tween the object's position.
/// </summary>

[AddComponentMenu("UGUI/Tween/UGUI Tween Position")]
public class UGUITweenPosition : UITweener
{
    public Vector3 from;
    public Vector3 to;

    Vector3 orignFrom;
    Vector3 orignTo;

    [HideInInspector]
    public bool worldSpace = false;

    RectTransform mTrans;
    UIRect mRect;

    public RectTransform cachedTransform { get { if (mTrans == null) mTrans = GetComponent<RectTransform>(); return mTrans; } set { mTrans = value; } }

    [System.Obsolete("Use 'value' instead")]
    public Vector3 position { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return worldSpace ? cachedTransform.position : cachedTransform.anchoredPosition3D;
        }
        set
        {
            if (mRect == null || !mRect.isAnchored || worldSpace)
            {
                if (worldSpace) cachedTransform.position = value;
                else cachedTransform.anchoredPosition3D = value;
            }
            else
            {
                value -= cachedTransform.anchoredPosition3D;
                NGUIMath.MoveRect(mRect, value.x, value.y);
            }
        }
    }

    void Awake()
    {
        mRect = GetComponent<UIRect>();
        orignFrom = from;
        orignTo = to;
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public UGUITweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        UGUITweenPosition comp = UITweener.Begin<UGUITweenPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public UGUITweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
    {
        UGUITweenPosition comp = UITweener.Begin<UGUITweenPosition>(go, duration);
        comp.worldSpace = worldSpace;
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public void PlayTween(bool forward)
    {
        this.Play(forward);
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
