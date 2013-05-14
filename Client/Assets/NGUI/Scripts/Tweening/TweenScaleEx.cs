using UnityEngine;

/// <summary>
/// Tween the object's local scale.
/// </summary>

[AddComponentMenu("NGUI/Tween/ScaleEx")]
public class TweenScaleEx : UITweener
{
    public Vector3 from;
    public Vector3 mid;
    public Vector3 to;
    public bool updateTable = false;

    public float midPosition = 0.5f;

    Transform mTrans;
    UITable mTable;

    public Vector3 scale { get { return mTrans.localScale; } set { mTrans.localScale = value; } }

    void Awake()
    {
        mTrans = transform;
        if (updateTable) mTable = NGUITools.FindInParents<UITable>(gameObject);
    }

    override protected void OnUpdate(float factor)
    {
        Vector3 old = factor <= midPosition ? from : mid;
        Vector3 target = factor <= midPosition ? mid : to;
        float realfactor = factor <= midPosition ? factor / midPosition : (factor - midPosition) / (1 - midPosition);

        mTrans.localScale = old * (1f - realfactor) + target * realfactor;
        if (mTable != null) mTable.repositionNow = true;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenScaleEx Begin(GameObject go, float duration, Vector3 scaleMid, Vector3 scaleEnd, float mid)
    {
        TweenScaleEx comp = UITweener.Begin<TweenScaleEx>(go, duration);
        comp.from = comp.scale;
        comp.mid = scaleMid;
        comp.to = scaleEnd;
        comp.midPosition = mid;
        return comp;
    }
}