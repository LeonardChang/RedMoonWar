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
    float frame = 0.033f;
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
                Debug.Log(data.TimingList[timeIndex].SeName);
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