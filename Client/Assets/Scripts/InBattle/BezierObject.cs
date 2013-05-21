using UnityEngine;
using System.Collections;

public class BezierObject : MonoBehaviour {
    public Vector3 Position0;
    public Vector3 Position1;
    public Vector3 Position2;
    public Vector3 Position3;

    public float MoveTime = 1;

    Bezier mBezier;
    float mTime = 0;

	// Use this for initialization
	void Start () {
        mBezier = new Bezier(Position0, Position1, Position2, Position3);
        mTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (mTime < MoveTime)
        {
            mTime += Time.deltaTime;
            float progress = mTime / MoveTime;
            if (progress > 1)
            {
                progress = 1;
            }

            Vector3 pos = mBezier.GetPointAtTime(progress);
            gameObject.transform.localPosition = pos;
        }
	}
}
