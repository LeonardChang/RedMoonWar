using UnityEngine;
using System.Collections;

public class Turner : MonoBehaviour {
    public string[] animationFileNames;

	// Use this for initialization
	void Start () {
        AnimationData animationData = new AnimationData();
        animationData.Init();
        int animationFileCount = animationFileNames.Length;
        for (int i = 0; i < animationFileCount; i++)
        {
            string fileName = animationFileNames[i];
            animationData.ReadAnimationDatas(fileName);
        }

        animationData.Write();

        Debug.Log("WRITE OK");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
