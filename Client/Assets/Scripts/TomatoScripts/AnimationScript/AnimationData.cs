using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public struct SDataAniFrameCell
{
    public int Pattern;
    public int X;
    public int Y;
    public int Scaling;
    public int Rotate;
    public int Mirror;
    public int Opacity;
    public int BlendType;
};

public struct SDataAniTiming
{
    public uint Frame;

    public string SeName;

    public int SeVolume;
    public int SePitch;

    public int FlashScope;
    public int FlashColorR;
    public int FlashColorG;
    public int FlashColorB;
    public int FlashColorA;
    public int FlashDuration;

    public int Weight;
};


public struct SDataAniFrame
{
    public uint CellNumber;
    public List<SDataAniFrameCell> CellList;
};









public struct SDataAnimation
{
    public uint AniID;
    public string Name;
    public string AniImg01;
    public ushort AniImg01Hue;
    public string AniImg02;
    public ushort AniImg02Hue;
    public ushort Position;
    public uint FrameNumber;
    public List<SDataAniFrame> FrameList;
    public uint TimingNumber;
    public List<SDataAniTiming> TimingList;
    public uint TotalWeight;
    public int MaxCellCount;
};






public class AnimationData{

    public List<SDataAnimation> animationDatas;

    public void Init()
    {
        animationDatas = new List<SDataAnimation>();
    }


    public void ReadAnimationDatas(string fileName)
    {
        string path = Application.dataPath + "/animation/animation/" + fileName + ".asset";
        Debug.Log(path);
        FileStream fs;
        fs = new FileStream(path,FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        uint number = br.ReadUInt32();
        for (int i = 0; i < number; i++)
        {
            //id
            SDataAnimation aniData = new SDataAnimation();
            aniData.MaxCellCount = 0;
            aniData.AniID = br.ReadUInt32();

            //name
            uint Length = br.ReadUInt32();
            byte[] data = br.ReadBytes((int)Length * 2-1);
            br.ReadBytes(1);
            aniData.Name = System.Text.Encoding.Unicode.GetString(data);
            Debug.Log("[data][Name]--------------------------------->" + aniData.Name);

            //animation1
            Length = br.ReadUInt32();
            data = br.ReadBytes((int)Length * 2-1);
            br.ReadBytes(1);
            aniData.AniImg01 = System.Text.Encoding.Unicode.GetString(data);
            aniData.AniImg01 = aniData.AniImg01.ToString();
            Debug.Log("[data][AniImg01]--------------------------------->" + aniData.AniImg01);

            //	ani image 01 hue
            aniData.AniImg01Hue = br.ReadUInt16();
            Debug.Log("[data][AniImg01Hue]--------------------------------->" + aniData.AniImg01Hue);

            //animation2
            Length = br.ReadUInt32();
            data = br.ReadBytes((int)Length * 2-1);
            br.ReadBytes(1);
            aniData.AniImg02 = System.Text.Encoding.Unicode.GetString(data);
            Debug.Log("[data][AniImg01]--------------------------------->" + aniData.AniImg02);

            //	ani image 02 hue
            aniData.AniImg02Hue = br.ReadUInt16();
            Debug.Log("[data][AniImg02Hue]--------------------------------->" + aniData.AniImg02Hue);

            //	position
            aniData.Position = br.ReadUInt16();

            //	frames
            aniData.FrameNumber = br.ReadUInt32();
            Debug.Log("aniData.FrameNumber" + aniData.FrameNumber);
            aniData.FrameList = new List<SDataAniFrame>();

            for (int frameidx = 0; frameidx < aniData.FrameNumber; frameidx ++)
            {
                SDataAniFrame frameData = new SDataAniFrame();
                frameData.CellNumber = br.ReadUInt32();
                if (frameData.CellNumber > aniData.MaxCellCount)
                {
                    aniData.MaxCellCount = (int)frameData.CellNumber;
                }
                frameData.CellList = new List<SDataAniFrameCell>();
                Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                for (int cellidx = 0; cellidx < frameData.CellNumber; cellidx++ )
                {
                    SDataAniFrameCell cellData = new SDataAniFrameCell();
                    cellData.Pattern = br.ReadInt32();
                    Debug.Log("Pattern------->" + cellData.Pattern);
                    cellData.X = br.ReadInt32();
                    cellData.Y = br.ReadInt32();
                    cellData.Scaling = br.ReadInt32();
                    cellData.Rotate = br.ReadInt32();
                    cellData.Mirror = br.ReadInt32();
                    cellData.Opacity = br.ReadInt32();
                    cellData.BlendType = br.ReadInt32();
                    frameData.CellList.Add(cellData);
                }

                Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                aniData.FrameList.Add(frameData);
            }

            aniData.TimingNumber = br.ReadUInt32();
            aniData.TimingList = new List<SDataAniTiming>();
            aniData.TotalWeight = 0;

            for (int timingidx = 0; timingidx < aniData.TimingNumber; timingidx++)
            {
                SDataAniTiming timeData = new SDataAniTiming();

                timeData.Frame = br.ReadUInt32();
                Length = br.ReadUInt32();
                data = br.ReadBytes((int)Length * 2-1);
                br.ReadBytes(1);
                timeData.SeName = System.Text.Encoding.Unicode.GetString(data);
                Debug.Log(timeData.SeName);


                timeData.SeVolume = br.ReadInt32();
                Debug.Log(timeData.SeVolume);
                timeData.SePitch = br.ReadInt32();
                timeData.FlashScope = br.ReadInt32();
                timeData.FlashColorR = br.ReadInt32();
                timeData.FlashColorG = br.ReadInt32();
                timeData.FlashColorB = br.ReadInt32();
                timeData.FlashColorA = br.ReadInt32();
                timeData.FlashDuration = br.ReadInt32();
                timeData.Weight = 0;

                aniData.TimingList.Add(timeData);
            }


            animationDatas.Add(aniData);
        }

        
    }
}
