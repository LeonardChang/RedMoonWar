using UnityEngine;
using System.Collections;

public class TrailKiller : MonoBehaviour {
    public float KillTime = 1;

    private float mDeathTime = 1;
    public float DeathTime
    {
        get
        {
            return mDeathTime;
        }
    }

	// Use this for initialization
	void Start () {
        float time = gameObject.GetComponent<TrailRenderer>().time;
        mDeathTime = time + KillTime;
        Destroy(gameObject, mDeathTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
