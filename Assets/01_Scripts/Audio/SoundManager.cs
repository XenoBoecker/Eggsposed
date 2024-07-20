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

    public void PlaySound(AudioClip[] clips, AudioSource source = null)
    {
        if (clips.Length == 0) return;
        
        int rand = UnityEngine.Random.Range(0, clips.Length);

        PlaySound(clips[rand], source);
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

    internal void StartLoopingSound(AudioClip clip, AudioSource audioSource)
    {
        if(audioSource == null) return;
        if(clip == null) return;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    internal void EndLoopingSound(AudioSource audioSource)
    {
        audioSource?.Stop();
    }

    [System.Serializable]
    public struct UISFX
    {
        public AudioSource source;
        public AudioClip changeButtonSound;
        public AudioClip confirmSound;
        public AudioClip CountdownSound;
    }

    [System.Serializable]
    public struct ChickenSFX
    {
        // SCREAMS ARE IN CHICKEN SCRIPTABLE OBJECTS!!!!!

       public AudioClip[] stepSounds; //
       public AudioClip[] grassStepSounds; //
       public AudioClip[] jumpSounds; //
       public AudioClip glideSound; //
       
       public AudioClip standUpSound; //
       public AudioClip sitDownSound; //
       
       public AudioClip dropEggSound; //
       public AudioClip pickUpEggSound; //
       public AudioClip eggHatchingSound; //

        // special chicken sounds

        // Hive Mind

        // Hydra

        // Lightning

        public AudioClip electricalSittingSound; //

        // Robot
        
        public AudioClip sonarBeeping; //

        // Rocket

        public AudioClip rocketBoostSound; //

        // Rotissory

        public AudioClip rotisserieCallSpinSound; //

        // Super Hot

        public AudioClip superHotSlowDown; //
        public AudioClip superHotSpeedUp; //
        
        public AudioClip superHotBreedingSound; //

        // Thicken

        // Wind Up

        public AudioClip windupSound; //
        public AudioClip unwindSound; //
    }

    [System.Serializable]
    public struct FarmerSFX
    {
        public AudioClip scanSound; //
        public AudioClip chaseSpeedSound; //
        public AudioClip normalSpeedSound; //
        
        public AudioClip collectingPlayersEggWarningSound; //
        public AudioClip inCollectRangeSound; //
        public AudioClip eggAlmostCollectedWarningSound; //

        public AudioClip blockBreedingSpotSound; //
    }

    [System.Serializable]
    public struct OtherSFX
    {
        
    }
}

