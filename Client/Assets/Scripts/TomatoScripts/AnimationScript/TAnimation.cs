using UnityEngine;
using System.Collections;

public class TAnimation : MonoBehaviour {
    SDataAnimation data;

    public UISprite sprite;

	// Use this for initialization
	void Start () {
        UIAtlas atlas = sprite.gameObject.AddComponent<UIAtlas>();
        sprite.atlas = atlas;

        Material atlasMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
        Texture texture = (Texture)Resources.Load("Temp/1");

        int col = texture.width / 192;
        int raw = texture.height / 192;

        atlasMaterial.mainTexture = texture;

        atlas.name = "testAtlas";
        atlas.material = atlasMaterial;




        UIAtlas.Sprite aSprite = new UIAtlas.Sprite();
        aSprite.name = "testsprite_1";
        Rect outer = new Rect(0, 0, 192, 192);
        aSprite.outer = outer;

        atlas.spriteList.Add(aSprite);

        sprite.atlas = atlas;
        sprite.spriteName = "testsprite_1";

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}