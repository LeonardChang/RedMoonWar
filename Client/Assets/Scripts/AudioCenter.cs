using UnityEngine;
using System.Collections;

public class AudioCenter : MonoBehaviour {
    public AudioSource BGMPlayer;
    public AudioSource[] SEPlayers;

    private static AudioCenter mInstance = null;
    public static AudioCenter Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject perfab = Resources.Load("Perfabs/AudioCenter") as GameObject;
                GameObject obj = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
                mInstance = obj.GetComponent<AudioCenter>();
            }
            return mInstance;
        }
    }

    private bool mOpenMusic = true;
    private bool mOpenSound = true;
    private float mMusicVolume = 0.7f;
    private float mSoundVolume = 0.7f;

    private float mSaveBGMVolume = 1;
    private float[] mSaveSEVolume = new float[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        
    /// <summary>
    /// �� / �ر�����
    /// </summary>
    public bool OpenMusicEffect
    {
        set
        {
            mOpenMusic = value;
            BGMPlayer.volume = mOpenMusic ? mMusicVolume * mSaveBGMVolume : 0;

            PlayerPrefs.SetInt("mOpenMusic", mOpenMusic ? 1 : 0);
            PlayerPrefs.Save();
        }

        get { return mOpenMusic; }
    }

    /// <summary>
    /// �� / �ر���Ч
    /// </summary>
    public bool OpenSoundEffect
    {
        set
        {
            mOpenSound = value;
            for (int i = 0; i < SEPlayers.Length; i++)
            {
                SEPlayers[i].volume = mOpenSound ? mSoundVolume * mSaveSEVolume[i] : 0;
            }

            PlayerPrefs.SetInt("mOpenSound", mOpenSound ? 1 : 0);
            PlayerPrefs.Save();
        }

        get { return mOpenSound; }
    }

    /// <summary>
    /// ȫ����������
    /// </summary>
    public float GlobalMusicVolume
    {
        set
        { 
            mMusicVolume = value;
            BGMPlayer.volume = mOpenMusic ? mMusicVolume * mSaveBGMVolume : 0;

            PlayerPrefs.SetFloat("mMusicVolume", mMusicVolume);
            PlayerPrefs.Save();
        }

        get { return mMusicVolume; }
    }

    /// <summary>
    /// ȫ����Ч����
    /// </summary>
    public float GlobalSoundVolume
    {
        set
        { 
            mSoundVolume = value;
            for (int i = 0; i < SEPlayers.Length; i++)
            {
                SEPlayers[i].volume = mOpenSound ? mSoundVolume * mSaveSEVolume[i] : 0;
            }

            PlayerPrefs.SetFloat("mSoundVolume", mSoundVolume);
            PlayerPrefs.Save();
        }

        get { return mSoundVolume; }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("mOpenMusic"))
        {
            mOpenMusic = PlayerPrefs.GetInt("mOpenMusic") == 1;
            mOpenSound = PlayerPrefs.GetInt("mOpenSound") == 1;
            mMusicVolume = PlayerPrefs.GetFloat("mMusicVolume");
            mSoundVolume = PlayerPrefs.GetFloat("mSoundVolume");
        }
        else
        {
            OpenMusicEffect = true;
            OpenSoundEffect = true;
            GlobalMusicVolume = 0.7f;
            GlobalSoundVolume = 0.7f;
        }

        if (mInstance == null)
        {
            mInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        InvokeRepeating("ClearSoundResources", 2, 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// ����һ������
    /// </summary>
    /// <param name="_clipName"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(string _clipName)
    {
        return PlayMusic(_clipName, 1);
    }

    /// <summary>
    /// ����һ������
    /// </summary>
    /// <param name="_clipName"></param>
    /// <param name="_volume"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(string _clipName, float _volume)
    {
        AudioClip clip = GetClip(_clipName);
        AudioSource bgmPlayer = BGMPlayer;
        if (clip != null && bgmPlayer.clip != clip)
        {
            mSaveBGMVolume = _volume;

            bgmPlayer.clip = clip;
            bgmPlayer.volume = OpenMusicEffect ? GlobalMusicVolume * mSaveBGMVolume : 0;
            bgmPlayer.pitch = 1;
            bgmPlayer.loop = true;
            bgmPlayer.Play();

            return bgmPlayer;
        }

        return null;
    }

    /// <summary>
    /// ����һ����Ч
    /// </summary>
    /// <param name="_clipName"></param>
    /// <returns></returns>
    public AudioSource PlaySound(string _clipName)
    {
        return PlaySound(_clipName, 1);
    }

    /// <summary>
    /// ����һ����Ч
    /// </summary>
    /// <param name="_clipName"></param>
    /// <param name="_volume"></param>
    /// <returns></returns>
    public AudioSource PlaySound(string _clipName, float _volume)
    {
        AudioClip clip = GetClip(_clipName);
        int sePlayerID = FreeSEPlayer;
        if (clip != null && sePlayerID != -1)
        {
            mSaveSEVolume[sePlayerID] = _volume;

            SEPlayers[sePlayerID].clip = clip;
            SEPlayers[sePlayerID].volume = OpenSoundEffect ? GlobalSoundVolume * mSaveSEVolume[sePlayerID] : 0;
            SEPlayers[sePlayerID].pitch = 1;
            SEPlayers[sePlayerID].Play();

            return BGMPlayer;
        }

        return null;
    }

    /// <summary>
    /// ֹͣ������Ч
    /// </summary>
    public void StopAllSound()
    {
        foreach (AudioSource se in SEPlayers)
        {
            se.Stop();
            se.clip = null;
        }
    }

    int FreeSEPlayer
    {
        get
        {
            for (int i = 0; i < SEPlayers.Length; i++)
            {
                AudioSource source = SEPlayers[i];
                if (source.clip == null || !source.isPlaying)
                {
                    return i;
                }
            }

            return -1;
        }
    }

    AudioClip GetClip(string _clipName)
    {
        return Resources.Load("Sounds/" + _clipName) as AudioClip;
    }

    void ClearSoundResources()
    {
        foreach (AudioSource source in SEPlayers)
        {
            if (source.clip != null && !source.isPlaying)
            {
                source.clip = null;
            }
        }
    }
}
