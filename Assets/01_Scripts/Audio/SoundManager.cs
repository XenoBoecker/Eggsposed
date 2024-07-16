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
    public UISFX uiSFX;
    public ChickenSFX chickenSFX;
    public FarmerSFX farmerSFX;
    public OtherSFX otherSFX;

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
        if (clip == null) return;

        if (!sfxOn) return;


        if (source == null) sfxAudioSource.PlayOneShot(clip);
        else
        {
            source.volume = sfxAudioSource.volume;
            source.PlayOneShot(clip);
        }
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
    public struct UISFX
    {
        [SerializeField] AudioSource source;
    }

    [System.Serializable]
    public struct ChickenSFX
    {

       public AudioClip[] screams;
       public AudioClip[] stepSounds;
       public AudioClip jumpSound;
       public AudioClip glideSound;
       
       public AudioClip standUpSound;
       public AudioClip sitDownSound;
       
       public AudioClip dropEggSound;
       public AudioClip pickUpEggSound;
       public AudioClip eggHatchingSound;

        // special chicken sounds

        // Hive Mind

        // Hydra

        // Lightning

        // Robot

        public AudioClip robotWalkSound;
        public AudioClip indikatorBeeping;

        // Rocket

        // Rotissory

        // Super Hot

        public AudioClip superHotSlowDown;
        public AudioClip superHotSpeedUp;
        
        public AudioClip breedingSound;

        // Thicken

        // Tortured

        public AudioClip torturedChickenScream;

        // Wind Up

        public AudioClip windupSound;
        public AudioClip unwindSound;
    }

    [System.Serializable]
    public struct FarmerSFX
    {
        public AudioClip scanSound;
        public AudioClip warnSound;
        public AudioClip chaseSpedSound;
        public AudioClip normalSpeedSound;
    }

    [System.Serializable]
    public struct OtherSFX
    {
        
    }
}

