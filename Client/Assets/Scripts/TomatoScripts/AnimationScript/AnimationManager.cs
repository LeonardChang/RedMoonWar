using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {
    public static AnimationData animationData;
    public string[] animationFileNames;

	// Use this for initialization
	void Start () {

        animationData = new AnimationData();
        animationData.Init();
        int animationFileCount = animationFileNames.Length;
        for (int i = 0; i < animationFileCount; i++ )
        {
            string fileName = animationFileNames[i];
            animationData.ReadAnimationDatas(fileName);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public object GetAnimationByName(string name)
    {
        foreach (SDataAnimation data in animationData.animationDatas)
        {
            if (data.Name.Equals(name))
            {
                return data;
            }
        }
        return null;
    }
}
