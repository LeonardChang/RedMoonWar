using UnityEngine;
using System.Collections;

public class AutoDeath : MonoBehaviour {
    public float DeathTime = 10;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, DeathTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
