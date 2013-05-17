using UnityEngine;
using System.Collections;

public class TAnimObject : MonoBehaviour {
    public string Name;

	// Use this for initialization
	void Start () {
        TAnimation anim = AnimationManager.MakeAnimation(Name, gameObject);
        anim.endTarget = gameObject;
        anim.endMessage = "TAnimationEnd";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TAnimationEnd()
    {
        Destroy(gameObject);
    }
}
