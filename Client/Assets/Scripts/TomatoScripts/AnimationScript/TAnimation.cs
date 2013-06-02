using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class TAnimation : MonoBehaviour {
    [HideInInspector]
    public SDataAnimation data;
    [HideInInspector]
    public List<UIAtlas> atlases;
    [HideInInspector]
    public List<UISprite> sprites;


    public bool loop = false;
    public GameObject endTarget;
    public string endMessage;

    public GameObject timeTarget;
    public string timeMessage;



    [HideInInspector]
    public UISprite sprite;

    int nowFrameIndex = 0;

    float frameTime = 0;
    float frame = 0.05f;
    //float frame = 5f;

    public void Init()
    {
        sprites = new List<UISprite>();
        atlases = new List<UIAtlas>();
    }


    


    void Update()
    {
        if (nowFrameIndex == data.FrameNumber)
        {
            return;
        }
        frameTime += Time.deltaTime;
        if (frameTime <= frame)
        {
            return;
        }
        else
        {
            frameTime = 0;
        }

        for (int timeIndex = 0; timeIndex < data.TimingNumber; timeIndex++)
        {
            //Debug.Log(data.TimingList[timeIndex].Frame);
            if (nowFrameIndex == data.TimingList[timeIndex].Frame)
            {
                if (timeTarget != null)
                {
                    if (timeMessage != "")
                    {
                        timeTarget.SendMessage(timeMessage,data.TimingList[timeIndex],SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
        SDataAniFrame frameData = data.FrameList[nowFrameIndex];
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        for (int start = (int)frameData.CellNumber; start < data.MaxCellCount; start++)
        {
            sprites[start].gameObject.SetActive(false);
        }


        for (int cellIndex = 0; cellIndex < frameData.CellNumber; cellIndex++)
        {
            
            SDataAniFrameCell cellData = frameData.CellList[cellIndex];
            //Debug.Log(cellData.Pattern);
            if (cellData.Pattern != -1)
            {
                sprites[cellIndex].gameObject.SetActive(true);
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


                GameObject sGame = sprites[cellIndex].gameObject;

                sGame.transform.rotation = Quaternion.identity;
                sGame.transform.RotateAround(sGame.transform.forward, cellData.Rotate);

                Vector3 scale = new Vector3(cellData.Scaling / 100 * 192, cellData.Scaling / 100 * 192, 1);

                sGame.transform.localPosition = position;
                if (scale.x == 0)
                {
                    scale.x = 0.001f;
                }
                if (scale.y == 0)
                {
                    scale.y = 0.001f;
                }

                sGame.transform.localScale = scale;
                Color color = sprites[cellIndex].color;
                color.a = (float)cellData.Opacity / 255;
                sprites[cellIndex].color = color;
            }
            else
            {
                sprites[cellIndex].gameObject.SetActive(false);
            }
        }

        nowFrameIndex++;
        if (nowFrameIndex == data.FrameNumber)
        {
            if (loop)
            {
                nowFrameIndex = 0;
            }
            else
            {
                if (endTarget != null)
                {
                    if (endMessage != "")
                    {
                        endTarget.SendMessage(endMessage, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            
        }

    }

}