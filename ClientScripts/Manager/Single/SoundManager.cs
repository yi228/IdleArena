using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioClip[] clips;

    private List<AudioSource> sources = new List<AudioSource>();

    public AudioSource bgmSource { get; private set; }
    private bool loadingBGMPlayed = false;
    private bool ingameBGMPlayed = false;
    public bool effectMute { get; private set; }

    void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion
    }
    void Start()
    {
        AddClip();
        bgmSource = GetComponent<AudioSource>();
        effectMute = false;
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "LoadingScene" && !loadingBGMPlayed)
        {
            loadingBGMPlayed = true;
            ingameBGMPlayed = false;
            PlayClip(bgmSource, "BGM_LOADING", true, 50f);
        }
        if(SceneManager.GetActiveScene().name == "MainScene" && !ingameBGMPlayed)
        {
            ingameBGMPlayed = true;
            loadingBGMPlayed = false;
            PlayClip(bgmSource, "BGM_INGAME", true, 50f);
        }
    }
    private void AddClip()
    {
        for (int i = 0; i < clips.Length; i++)
            audioClips.Add(clips[i].name, clips[i]);
    }
    public void PlayClip(AudioSource _audioSource, string _clipKey, bool _loop = false, float _volume = 100f)
    {
        _audioSource.clip = audioClips[_clipKey];
        _audioSource.volume = _volume / 100;
        _audioSource.loop = _loop;
        _audioSource.Play();

        if(!sources.Contains(_audioSource))
            sources.Add(_audioSource);
    }
    public void ShiftOnOffBGM()
    {
        bgmSource.mute = !bgmSource.mute;
    }
    public void ShiftOnOffEffect()
    {
        effectMute = !effectMute;
        for(int i=0; i<sources.Count; i++)
            if (sources[i] != null && sources[i] != bgmSource)
                sources[i].mute = effectMute;
    }
}
