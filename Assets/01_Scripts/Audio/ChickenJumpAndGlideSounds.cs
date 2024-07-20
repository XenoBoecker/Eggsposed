using System;
using UnityEngine;

public class ChickenJumpAndGlideSounds : MonoBehaviour
{
    AudioSource audioSource;

    ChickenStateTracker cst;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        cst = GetComponent<ChickenStateTracker>();
        cst.OnStartGlide += () => PlayGlideSound();
        cst.OnStopGlide += () => StopGlideSound();
        
        cst.OnJump += PlayJumpSound;
    }

    private void PlayJumpSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.jumpSounds, audioSource);
    }

    private void PlayGlideSound()
    {
        if (!audioSource.isPlaying || audioSource.clip != SoundManager.Instance.chickenSFX.glideSound)
        {
            audioSource.clip = SoundManager.Instance.chickenSFX.glideSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopGlideSound()
    {
        if (audioSource.clip == SoundManager.Instance.chickenSFX.glideSound)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }
}
