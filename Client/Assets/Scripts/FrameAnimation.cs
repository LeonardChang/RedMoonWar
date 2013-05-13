using UnityEngine;
using System.Collections;

public class FrameAnimation : MonoBehaviour {
    public GameObject[] SpriteList;
    public float TimeFrame;

    int mFrame = -1;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < SpriteList.Length; i++)
        {
            SpriteList[i].active = false;
        }
        InvokeRepeating("Next", TimeFrame, TimeFrame);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Next()
    {
        mFrame += 1;
        for (int i = 0; i < SpriteList.Length; i++ )
        {
            SpriteList[i].active = mFrame == i;
        }

        if (mFrame >= SpriteList.Length)
        {
            Destroy(gameObject);
        }
    }
}
