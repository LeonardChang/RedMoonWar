using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageTest : MonoBehaviour {
    public UIInput StageIDInputer;
    public GameObject[] CardInfoRoots;

    private List<BattleCharacterData> mList = new List<BattleCharacterData>();

	// Use this for initialization
	void Start () {
        StageIDInputer.text = Stage.Instance.CurrentLocalStageID.ToString();

        if (Application.isEditor && System.IO.File.Exists(Application.dataPath + "/Resources/Datas/TestTeam.txt"))
        {
            System.Xml.Serialization.XmlSerializer sel = new System.Xml.Serialization.XmlSerializer(typeof(List<BattleCharacterData>));

            System.IO.StreamReader reader = new System.IO.StreamReader(Application.dataPath + "/Resources/Datas/TestTeam.txt");
            mList = sel.Deserialize(reader) as List<BattleCharacterData>;
            reader.Close();

            for (int i = 0; i < 6; i++)
            {
                Transform root = CardInfoRoots[i].transform;
                UILabel infoLabel = root.FindChild("Label").gameObject.GetComponent<UILabel>();
                UIInput idInput = root.FindChild("InputID").gameObject.GetComponent<UIInput>();
                UIInput levelInput = root.FindChild("InputLevel").gameObject.GetComponent<UIInput>();

                infoLabel.text = CardManager.Instance.GetCard(mList[i].CardID).Name;
                idInput.text = mList[i].CardID.ToString();
                levelInput.text = mList[i].Level.ToString();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ResetInfo()
    {
        Stage stage = Stage.Instance;

        mList.Clear();
        for (int i = 0; i < 6; i++)
        {
            Transform root = CardInfoRoots[i].transform;
            UILabel infoLabel = root.FindChild("Label").gameObject.GetComponent<UILabel>();
            UIInput idInput = root.FindChild("InputID").gameObject.GetComponent<UIInput>();
            UIInput levelInput = root.FindChild("InputLevel").gameObject.GetComponent<UIInput>();

            int cardID = int.Parse(idInput.text);
            int level = int.Parse(levelInput.text);

            BattleCharacterData data = new BattleCharacterData();
            stage.CreateCharactor(ref data, i, cardID, level);
            data.InitX = 0 + i;
            data.InitY = 1;

            mList.Add(data);
        }

        stage.ClearPlayers();
        for (int i = 0; i < 6; i++)
        {
            stage.AddCharactor(mList[i]);
        }
    }

    void SaveInfo()
    {
        ResetInfo();

        for (int i = 0; i < 6; i++)
        {
            Transform root = CardInfoRoots[i].transform;
            UILabel infoLabel = root.FindChild("Label").gameObject.GetComponent<UILabel>();
            UIInput idInput = root.FindChild("InputID").gameObject.GetComponent<UIInput>();
            UIInput levelInput = root.FindChild("InputLevel").gameObject.GetComponent<UIInput>();

            infoLabel.text = CardManager.Instance.GetCard(mList[i].CardID).Name;
        }

        if (Application.isEditor)
        {
            System.Xml.Serialization.XmlSerializer sel = new System.Xml.Serialization.XmlSerializer(typeof(List<BattleCharacterData>));
            System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.dataPath + "/Resources/Datas/TestTeam.txt", false);
            sel.Serialize(writer, mList);
            writer.Close();
        }
    }

    void StartStage()
    {
        SaveInfo();

        Stage.Instance.InitializeLocal(int.Parse(StageIDInputer.text));
        Application.LoadLevel("Game");
    }
}
