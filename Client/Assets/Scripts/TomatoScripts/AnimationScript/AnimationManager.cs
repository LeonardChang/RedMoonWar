using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {
    public static AnimationData animationData;
    public static GameObject atlasObject;

    public static List<UIAtlas> atlasList;

	// Use this for initialization
	void Start () {
        atlasList = new List<UIAtlas>();
        animationData = new AnimationData();
        animationData.Init();
        //int animationFileCount = animationFileNames.Length;
        //for (int i = 0; i < animationFileCount; i++)
        //{
        //    string fileName = animationFileNames[i];
        //    animationData.ReadAnimationDatas(fileName);
        //}
        animationData.ReadFromXml();

        AnimationManager.atlasObject = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.C))
        {
            MakeAnimation("HealthHP", GameObject.Find("Panel"));
            
        }
	}

    /// <summary>
    /// ͨ�����ֻ�ö�������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static object GetAnimationByName(string name)
    {
        foreach (SDataAnimation data in animationData.animationDatas)
        {
            if (data.Name == name)
            {
                return data;
            }
        }
        return null;
    }

    static TAnimation CreateAnimationObject(SDataAnimation data, GameObject target)
    {
        GameObject newGame = new GameObject();
        GameObject father = (GameObject)Instantiate(newGame);
        father.transform.parent = target.transform;
        father.transform.localPosition = Vector3.zero;
        father.transform.localScale = new Vector3(1,1,1);
        father.name = data.Name;
        TAnimation ani = father.AddComponent<TAnimation>();
        ani.Init();
        ani.data = data;

        if (data.AniImg01 != "")
        {
            string name1 =  "1_" + data.Name;
            UIAtlas at1 = CheckAtlas(name1);
            if (at1 == null)
            {
                at1 = MakeAtlas(data, 1);
            }

            ani.atlases.Add(at1);
        }

        if (data.AniImg02 != "")
        {
            string name2 = "2_" + data.Name;
            UIAtlas at2 = CheckAtlas(name2);
            if (at2 == null)
            {
                at2 = MakeAtlas(data, 2);
            }

            ani.atlases.Add(at2);
        }



        for (int i = 0; i < data.MaxCellCount; i++)
        {
            GameObject child = (GameObject)Instantiate(newGame);
            child.name = "animation" + (i + 1).ToString();
            child.transform.parent = father.transform;
            UISprite sprite =  child.AddComponent<UISprite>();
            sprite.atlas = ani.atlases[0];
            ani.sprites.Add(sprite);
        }
        Destroy(newGame);
        return ani;
        

    }

    /// <summary>
    /// ����һ������
    /// </summary>
    /// <param name="name"></param>
    public static TAnimation MakeAnimation(string name,GameObject target)
    {
        object data = GetAnimationByName(name);
        if (data != null)
        {
            return CreateAnimationObject((SDataAnimation)data, target);
        }
        else
        {
            Debug.Log("data is null");
            return null;
        }
    }

    static UIAtlas CheckAtlas(string name)
    {
        foreach (UIAtlas atlas in atlasList)
        {

            if(atlas.gameObject.name == name)
            {
                return atlas;
            }
        }
        return null;
    }

    static UIAtlas MakeAtlas(SDataAnimation data,int index)
    {
        GameObject atObj = new GameObject();
        atObj.transform.parent = atlasObject.transform;
        UIAtlas atlas = atObj.AddComponent<UIAtlas>();
        atObj.name = index.ToString() + "_" +  data.Name;
        Texture texture;
        Material atlasMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
        if (index == 1)
        {
            texture = (Texture)Resources.Load("TempAnimation/" + data.AniImg01);
            Debug.Log("TempAnimation/" + data.AniImg01);
        }
        else
        {
            texture = (Texture)Resources.Load("TempAnimation/" + data.AniImg02);
            Debug.Log("TempAnimation/" + data.AniImg02);
        }

        atlasMaterial.mainTexture = texture;
        atlas.material = atlasMaterial;

        int col = texture.width / 192;
        int raw = texture.height / 192;

        for (int rawIndex = 0; rawIndex < raw; rawIndex++)
        {
            for (int colIndex = 0; colIndex < col; colIndex++)
            {
                UIAtlas.Sprite aSprite = new UIAtlas.Sprite();
                int Idx = (rawIndex * col + colIndex);
                if (index == 2)
                {
                    Idx += 100;
                }
                aSprite.name = Idx.ToString();
                Rect outer = new Rect(colIndex * 192, rawIndex*192, 192, 192);
                aSprite.outer = outer;
                atlas.spriteList.Add(aSprite);
            }
        }

        atlasList.Add(atlas);
        return atlas;
    }


}
