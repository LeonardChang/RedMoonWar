using UnityEngine;
using System.Collections;

public class LabelTweener : UITweener
{
    public int from = 1;
    public int to = 100;
    public string format = "";

    UILabel mLabel = null;

    private int mNumber = 1;
    public int number
    {
        get
        {
            return mNumber;
        }
        set
        {
            mNumber = value;

            if (mLabel != null)
            {
                if (string.IsNullOrEmpty(format))
                {
                     mLabel.text = mNumber.ToString();
                }
                else
                {
                    mLabel.text = string.Format(format, mNumber);
                }
            }
        }
    }

    void Awake()
    {
        mLabel = GetComponent<UILabel>();
    }

    /// <summary>
    /// Interpolate and update the alpha.
    /// </summary>

    override protected void OnUpdate(float factor, bool isFinished)
    {
        number = Mathf.RoundToInt(Mathf.Lerp(from, to, factor));
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public LabelTweener Begin(GameObject go, float duration, int from, int to)
    {
        LabelTweener comp = UITweener.Begin<LabelTweener>(go, duration);
        comp.from = from;
        comp.to = to;
        
        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
