using UnityEngine;
using System.Collections;

public class CardAnimation : MonoBehaviour {
    public System.Action<string> AnimationEvent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AnimationEventCall(string _event)
    {
        if (AnimationEvent != null)
        {
            AnimationEvent(_event);
        }
    }
}
