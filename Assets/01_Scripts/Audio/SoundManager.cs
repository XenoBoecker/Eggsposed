using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioClip[] music;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    bool musicOn = true;
    bool sfxOn = true;
    int currentTrack = 0;

    public event Action onSoundReload;

    [Header("SFX")]
    [SerializeField] UISFX uiSFX;
    [SerializeField] ChickenSFX chickenSFX;
    [SerializeField] FarmerSFX farmerSFX;
    [SerializeField] OtherSFX otherSFX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource.loop = true;
        musicAudioSource.clip = music[currentTrack];
        if(musicOn && music.Length > 0) musicAudioSource.Play();

        Reload();
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public float GetSFXVolume()
    {
        if (!sfxOn) return 0;

        return sfxAudioSource.volume;
    }

    public void PlaySound(AudioClip clip, AudioSource source = null)
    {
        if (!sfxOn) return;

        if (source == null) sfxAudioSource.PlayOneShot(clip);
        else source.PlayOneShot(clip);
    }

    internal void Reload()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        bool wasMusicOn = musicOn;
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        if (musicOn && !wasMusicOn) musicAudioSource.Play();
        else if(!musicOn) musicAudioSource.Stop();

        sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        onSoundReload?.Invoke();
    }

    [System.Serializable]
    struct UISFX
    {
        [SerializeField] AudioSource source;
        [SerializeField] AudioClip buttonClickSound;
        [SerializeField] AudioClip buttonHoverSound;


    }

    [System.Serializable]
    struct ChickenSFX
    {

        [SerializeField] AudioClip[] screams;
        [SerializeField] AudioClip walkingSound;
        [SerializeField] AudioClip jumpSound;
        [SerializeField] AudioClip glideSound;

        [SerializeField] AudioClip standUpSound;
        [SerializeField] AudioClip sitDownSound;

        [SerializeField] AudioClip dropEggSound;
        [SerializeField] AudioClip pickUpEggSound;
        [SerializeField] AudioClip eggHatchingSound;

        // special chicken sounds

        // Hive Mind

        // Hydra

        // Lightning

        // Robot

        [SerializeField] AudioClip robotWalkSound;
        [SerializeField] AudioClip indikatorBeeping;

        // Rocket

        // Rotissory

        // Super Hot

        [SerializeField] AudioClip superHotSlowDown;
        [SerializeField] AudioClip superHotSpeedUp;

        [SerializeField] AudioClip breedingSound;

        // Thicken

        // Tortured

        [SerializeField] AudioClip torturedChickenScream;

        // Wind Up

        [SerializeField] AudioClip windupSound;
        [SerializeField] AudioClip unwindSound;
    }

    [System.Serializable]
    struct FarmerSFX
    {
        public AudioClip scanSound;
        public AudioClip warnSound;
        public AudioClip walkSound;
        public AudioClip catchSound;
    }

    [System.Serializable]
    struct OtherSFX
    {
        
    }
}

