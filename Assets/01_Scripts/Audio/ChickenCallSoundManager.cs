using System.Collections;
using UnityEngine;

public class ChickenCallSoundManager : MonoBehaviour
{
    AudioSource audioSource;
    Chicken chicken;


    [SerializeField] ChickenData hydraChicken;
    [SerializeField] private float[] pitches = { 0.9f, 1.0f, 1.1f }; // Array of different pitches
    [SerializeField] private float[] delays = { 0.0f, 0.1f, 0.2f }; // Delays for each sound


    [SerializeField] ChickenData torturedChicken;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        chicken = GetComponent<Chicken>();

        chicken.OnCall += PlayCallSound;
    }

    private void PlayCallSound(bool activateOnCDCalls)
    {
        if (chicken.BodyData == hydraChicken || chicken.HeadData == hydraChicken)
        {
            StartCoroutine(PlaySoundMultipleTimes());
        }
        else if (chicken.BodyData == torturedChicken)
        {
            if(chicken.IsControlledByPlayer) SoundManager.Instance.PlaySound(chicken.BodyData.callSound, audioSource);
            else SoundManager.Instance.PlaySound(chicken.BodyData.callSound, audioSource, 0.3f);
        }
        else
        {
            if(!chicken.IsControlledByPlayer && chicken.HeadData == torturedChicken) SoundManager.Instance.PlaySound(chicken.HeadData.callSound, audioSource, 0.3f);
            else SoundManager.Instance.PlaySound(chicken.HeadData.callSound,audioSource);
        }

    }
    private IEnumerator PlaySoundMultipleTimes()
    {
        for (int i = 0; i < pitches.Length; i++)
        {
            StartCoroutine(PlaySoundWithDelay(chicken.BodyData.callSound, pitches[i], delays[i]));
        }
        yield return null; // End the coroutine
    }

    private IEnumerator PlaySoundWithDelay(AudioClip clip, float pitch, float delay)
    {
        if (clip != null)
        {
            yield return new WaitForSeconds(delay);

            // Create a new GameObject for each sound to avoid overlapping issues
            GameObject soundObject = new GameObject("SoundObject");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.pitch = pitch;
            audioSource.Play();

            // Destroy the GameObject after the clip has finished playing
            Destroy(soundObject, clip.length / pitch);
        }

    }
}