//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tween the object's color.
/// </summary>

[AddComponentMenu("NGUI/Tween/ColorWZC")]
public class TweenColorWZC : UITweener
{
	public Color from = Color.white;
	public Color to = Color.white;

	Transform mTrans;
	UIWidget mWidget;
    UIWidget[] mWidgets;
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

    public void SetColors()
    {
        int count = mWidgets.Length;
        for (int i = 0; i < count; i++ )
        {
            UIWidget wi = mWidgets[i];
 
        }
        
    }

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mWidget = GetComponentInChildren<UIWidget>();
        mWidgets = GetComponentsInChildren<UIWidget>();
		Renderer ren = renderer;
		if (ren != null) mMat = ren.material;
		mLight = light;
	}

	/// <summary>
	/// Interpolate and update the color.
	/// </summary>

	override protected void OnUpdate(float factor, bool isFinished) 
    { 
        color = Color.Lerp(from, to, factor);
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenColor Begin (GameObject go, float duration, Color color)
	{
		TweenColor comp = UITweener.Begin<TweenColor>(go, duration);
		comp.from = comp.color;
		comp.to = color;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}