//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ugui版本的TweenAlpha插值动画组件  Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

public class UGUITweenAlpha : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    [Range(0f, 1f)]
    public float to = 1f;

    bool mCached = false;
    /// <summary>
    /// 效果包含子节点
    /// </summary>
    CanvasGroup group = null;
    Graphic graphic = null;

    [System.Obsolete("Use 'value' instead")]
    public float alpha { get { return this.value; } set { this.value = value; } }

    void Cache()
    {
        mCached = true;
        group = GetComponent<CanvasGroup>();
        if (group == null)
        {
            graphic = GetComponent<Graphic>();
        }
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get
        {
            if (!mCached) Cache();
            if (group != null) return group.alpha;
            return graphic.color.a;
        }
        set
        {
            if (!mCached) Cache();

            if (group != null)
            {
                group.alpha = value;
            }
            else if (graphic != null)
            {
                Color c = graphic.color;
                c.a = value;
                graphic.color = c;
            }else
            {
                Debug.LogError("TweenAlpha need a Image or CanvasGroup Component");
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public UGUITweenAlpha Begin(GameObject go, float duration, float alpha)
    {
        UGUITweenAlpha comp = UITweener.Begin<UGUITweenAlpha>(go, duration);
        comp.from = comp.value;
        comp.to = alpha;

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
		UGUITweenAlpha comp = UITweener.Begin<UGUITweenAlpha>(this.gameObject, duration);
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
