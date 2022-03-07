using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    const string BGM_KEY = "IsBGM";
    const string EFF_KEY = "IsEFF";
    const float EFFECT_DELAY = 0.2f;

    public enum AUDIO_TYPE { BGM, EFFECT }
    public enum SOUND_TRACK
    {
        BG = 0,
        SLIDE,
        BUTTON,
        ITEM_BUY,
        BANANA,
        GET_STAR,
        SHOOT,
        COLLISION,
        COLLISION_RECV,
        DESTROY,
        BONUS,
        SNOW,
        RAND_ITEM,
        CONTINUE,
        CLEAR,
        FAILED,
        End
    }

    Dictionary<SOUND_TRACK, AudioClip> _dicSoundClips = new Dictionary<SOUND_TRACK, AudioClip>();
    [SerializeField] AudioSource _bgmSource = null;
    [SerializeField] AudioSource _effectSource = null;

    #region Singleton
    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<SoundManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    _instance = obj.AddComponent<SoundManager>();

                    _instance._effectSource = new GameObject("Effect").AddComponent<AudioSource>();
                    _instance._effectSource.transform.SetParent(_instance.transform);

                    _instance._bgmSource = new GameObject("BG").AddComponent<AudioSource>();
                    _instance._bgmSource.transform.SetParent(_instance.transform);

                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    //딜레이 시간 재기 
    float _effectTime = 0.0f;

    public bool IsBGM_Silence { get { return _isBGM_Silence; } }
    [SerializeField] bool _isBGM_Silence = false;

    public bool IsEFF_Silence { get { return _isEFF_Silence; } }
    [SerializeField]  bool _isEFF_Silence = false;

    [SerializeField] AudioClip[] _clips;


    private void Start()
    {
        LoadSound();
        SetSilence(AUDIO_TYPE.BGM, PlayerPrefs.GetInt(BGM_KEY, 1) == 0 ? true : false);
        SetSilence(AUDIO_TYPE.EFFECT, PlayerPrefs.GetInt(EFF_KEY, 1) == 0 ? true : false);
    }

    public void LoadSound()
    {
        if (_clips == null)
        {
            const string PATH = "Sound";
            _clips = Resources.LoadAll<AudioClip>(PATH);
        }

        _dicSoundClips.Clear();
        
        int len = (int)SOUND_TRACK.End;
        for (int i = 0; i < len; ++i)
            _dicSoundClips.Add((SOUND_TRACK)i, _clips[i]);
    }

    public void SetSilence(AUDIO_TYPE type, bool isSilence)
    {

        switch (type)
        {
            case AUDIO_TYPE.BGM:
                _isBGM_Silence = isSilence;

                if (_isBGM_Silence)
                    _bgmSource.volume = 0.0f;
                else
                    _bgmSource.volume = 1.0f;

                PlayerPrefs.SetInt(BGM_KEY, _isBGM_Silence ? 0 : 1);
                break;

            case AUDIO_TYPE.EFFECT:
                _isEFF_Silence = isSilence;
                if (_isEFF_Silence)
                    _effectSource.volume = 0.0f;
                else
                    _effectSource.volume = 1.0f;

                PlayerPrefs.SetInt(EFF_KEY, _isEFF_Silence ? 0 : 1);
                break;
        }
    }


    public void SetVolume(AUDIO_TYPE type, float volume)
    {
        switch (type)
        {
            case AUDIO_TYPE.BGM:
                _bgmSource.volume = volume;
                break;

            case AUDIO_TYPE.EFFECT:
                _effectSource.volume = volume;
                break;
        }
    }


    public void Play(SOUND_TRACK track, float volume = 1.0f)
    {
        switch (track)
        {
            case SOUND_TRACK.BG:
                if (_isBGM_Silence) return;
                if (_dicSoundClips.ContainsKey(track))
                {
                    _bgmSource.PlayOneShot(_dicSoundClips[track]);
                    _bgmSource.volume = volume;
                    _bgmSource.loop = true;
                }
                else
                {
                    Debug.LogWarning("None Sound");
                    LoadSound();
                    Play(track);
                }

                break;

            default:
                if (_isEFF_Silence) return;
                if (Time.time > _effectTime + EFFECT_DELAY )
                {
                    if (_dicSoundClips.ContainsKey(track))
                    {
                        _effectSource.PlayOneShot(_dicSoundClips[track]);
                        _effectSource.volume = volume;
                        _effectSource.loop = false;
                        _effectTime = Time.time;
                    }
                    else
                    {
                        Debug.LogWarning("None Sound");
                        LoadSound();
                        Play(track);
                    }
                }

                break;

        }
    }







}
