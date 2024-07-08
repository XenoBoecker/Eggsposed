using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        musicToggle.onValueChanged.AddListener(ToggleMusic);
        sfxToggle.onValueChanged.AddListener(ToggleSFX);

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
    }

    public void ToggleMusic(bool isOn)
    {
        if (isOn) PlayerPrefs.SetInt("MusicOn", 1);
        else PlayerPrefs.SetInt("MusicOn", 0);

        ReloadSoundManager();
    }

    public void ToggleSFX(bool isOn)
    {
        if (isOn) PlayerPrefs.SetInt("SFXOn", 1);
        else PlayerPrefs.SetInt("SFXOn", 0);

        ReloadSoundManager();
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);

        ReloadSoundManager();
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);

        ReloadSoundManager();
    }

    void ReloadSoundManager()
    {
        if (soundManager == null) return;

        soundManager.Reload();
    }
}