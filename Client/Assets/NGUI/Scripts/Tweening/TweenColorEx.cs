//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's color.
/// </summary>

[AddComponentMenu("NGUI/Tween/ColorEx")]
public class TweenColorEx : UITweener
{
	public Color from = Color.white;
    public Color mid = Color.white;
	public Color to = Color.white;

    public float midPosition = 0.5f;

	Transform mTrans;
	UIWidget mWidget;
	Material mMat;
	Light mLight;

	/// <summary>
	/// Current color.
	/// </summary>

	public Color color
	{
		get
		{
			if (mWidget != null) return mWidget.color;
			if (mLight != null) return mLight.color;
			if (mMat != null) return mMat.color;
			return Color.black;
		}
		set
		{
			if (mWidget != null) mWidget.color = value;
			if (mMat != null) mMat.color = value;

			if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
			}
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mWidget = GetComponentInChildren<UIWidget>();
		Renderer ren = renderer;
		if (ren != null) mMat = ren.material;
		mLight = light;
	}

	/// <summary>
	/// Interpolate and update the color.
	/// </summary>

    override protected void OnUpdate(float factor, bool isFinished)
    {
        Color old = factor <= midPosition ? from : mid;
        Color target = factor <= midPosition ? mid : to;
        float realfactor = factor <= midPosition ? factor / midPosition : (factor - midPosition) / (1 - midPosition);

        color = old * (1f - realfactor) + target * realfactor;
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenColorEx Begin(GameObject go, float duration, Color colorMid, Color colorEnd, float mid)
	{
		TweenColorEx comp = UITweener.Begin<TweenColorEx>(go, duration);
		comp.from = comp.color;
        comp.mid = colorMid;
        comp.to = colorEnd;
        comp.midPosition = mid;
		return comp;
	}
}