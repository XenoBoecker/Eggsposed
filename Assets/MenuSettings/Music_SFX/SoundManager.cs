using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioClip[] music;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    bool musicOn = true;
    bool sfxOn = true;
    int currentTrack = 0;

    public event Action onSoundReload;

    [Header("SFX")]
    [SerializeField] UISFX uiSFX;
    [SerializeField] ArtifactSFX artifactSFX;
    [SerializeField] StationSFX stationSFX;
    [SerializeField] ItemSFX itemSFX;

    private void Awake()
    {
        if (instance == null) instance = this;
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

    public void PlaySound(AudioClip clip)
    {
        if (!sfxOn) return;

        sfxAudioSource.PlayOneShot(clip);
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
        [SerializeField] AudioClip buttonClickSound;
        [SerializeField] AudioClip buttonHoverSound;
        [SerializeField] AudioSource source;
    }

    [System.Serializable]
    struct ArtifactSFX
    {
        [SerializeField] AudioClip dashSound;
        [SerializeField] AudioClip tunnelDashSound;
        [SerializeField] AudioClip stringOfTimeUseSound;
    }

    [System.Serializable]
    struct StationSFX
    {
        public AudioClip breakStationSound;
        public AudioClip destroyStationSound;
        public AudioClip repairStationSound;

        // task completed sounds
        public AudioClip uploadRPSound;
        public AudioClip collectNewMissionsSound;
        public AudioClip buyArtifactSound;


        public AudioClip moveShipUpSound;

        public AudioClip analyzingAnomalySound;
        public AudioClip anomalyAnalyzedSound;

        public AudioClip newMissionAvailableSound;
        public AudioClip missionCompletedSound;
    }

    [System.Serializable]
    struct ItemSFX
    {
        [SerializeField] AudioClip itemPickupSound;
        [SerializeField] AudioClip itemDropSound;
        [SerializeField] AudioClip useAnomalyScanner;
    }
}

