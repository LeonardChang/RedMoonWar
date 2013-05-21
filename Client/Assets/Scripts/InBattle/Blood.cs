using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TweenPosition.Begin(gameObject, 2.0f, transform.localPosition + new Vector3(0, 80, 0));
        TweenScaleEx.Begin(gameObject, 0.25f, new Vector3(100, 100, 1), new Vector3(50, 50, 1), 0.5f).from = new Vector3(1, 1, 1);

        Invoke("FadeOut", 1.5f);
        Destroy(gameObject, 2.0f);
	}

    void FadeOut()
    {
        Color color = gameObject.GetComponent<UILabel>().color;
        TweenColor.Begin(gameObject, 0.5f, new Color(color.r, color.g, color.b, 0));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
