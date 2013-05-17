using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class TAnimation : MonoBehaviour {
    public SDataAnimation data;
    public List<UIAtlas> atlases;
    public List<UISprite> sprites;

    public UISprite sprite;

    int nowFrameIndex = 0;

    float frameTime = 0;
    float frame = 0.033f;
    //float frame = 5f;

    public void Init()
    {
        sprites = new List<UISprite>();
        atlases = new List<UIAtlas>();
    }


    void Temp()
    {
        UIAtlas atlas = sprite.gameObject.AddComponent<UIAtlas>();
        sprite.atlas = atlas;

        Material atlasMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
        Texture texture = (Texture)Resources.Load("TempAnimation/1");

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


    void Update()
    {
        frameTime += Time.deltaTime;
        if (frameTime <= frame)
        {
            return;
        }
        else
        {
            frameTime = 0;
        }

        SDataAniFrame frameData = data.FrameList[nowFrameIndex];
        for (int cellIndex = 0; cellIndex < frameData.CellNumber; cellIndex++)
        {
            
            SDataAniFrameCell cellData = frameData.CellList[cellIndex];
            if (cellData.Pattern != -1)
            {
                sprites[cellIndex].gameObject.active = true;
                if (cellData.Pattern < 100)
                {
                    sprites[cellIndex].atlas = atlases[0];
                }
                else
                {
                    sprites[cellIndex].atlas = atlases[1];
                }
                sprites[cellIndex].spriteName = cellData.Pattern.ToString();
                Vector3 position = Vector3.zero;
                position.x = cellData.X;
                position.y = -cellData.Y;

                Vector3 scale = new Vector3(cellData.Scaling/100*192,cellData.Scaling/100*192,1);

                sprites[cellIndex].gameObject.transform.localPosition = position;
                sprites[cellIndex].gameObject.transform.localScale = scale;
                Color color = sprites[cellIndex].color;
                color.a = (float)cellData.Opacity/255;
                sprites[cellIndex].color = color;
            }
            else
            {
                sprites[cellIndex].gameObject.active = false;
            }
        }
        nowFrameIndex++;
        if (nowFrameIndex == data.FrameNumber)
        {
            nowFrameIndex = 0;
        }

    }

}