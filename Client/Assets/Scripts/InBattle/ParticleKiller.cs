using UnityEngine;
using System.Collections;

public class ParticleKiller : MonoBehaviour {
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
        float maxEnergy = gameObject.GetComponent<ParticleEmitter>().maxEnergy;
        Invoke("EndEmitter", KillTime);
        mDeathTime = maxEnergy + KillTime;
        Destroy(gameObject, mDeathTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void EndEmitter()
    {
        foreach (ParticleEmitter emitter in gameObject.GetComponentsInChildren<ParticleEmitter>())
        {
            emitter.emit = false;
        }
    }
}
