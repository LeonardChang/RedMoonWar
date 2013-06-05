using UnityEngine;
using System.Collections;

public class TAnimObject : MonoBehaviour {
    public string Name;

	// Use this for initialization
	void Start () {
        TAnimation anim = AnimationManager.MakeAnimation(Name, gameObject);
        anim.endTarget = gameObject;
        anim.endMessage = "TAnimationEnd";
        anim.timeTarget = gameObject;
        anim.timeMessage = "TimePoint";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TAnimationEnd()
    {
        Destroy(gameObject);
    }

    void TimePoint(SDataAniTiming _data)
    {
        if (!string.IsNullOrEmpty(_data.SeName))
        {
            AudioSource source = AudioCenter.Instance.PlaySound(_data.SeName, (float)_data.SeVolume / 100);
			if (source != null)
			{
            	source.pitch = (float)_data.SePitch / 100;
			}
        }
    }
}
