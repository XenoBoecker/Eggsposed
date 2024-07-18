using System;
using UnityEngine;

public class ChickenJumpAndGlideSounds : MonoBehaviour
{
    AudioSource audioSource;

    BaseChickenController bcc;
    Rigidbody rb;

    bool wasJumpingLastFrame = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bcc = GetComponent<BaseChickenController>();

        bcc.OnJump += PlayJumpSound;
    }

    private void PlayJumpSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.jumpSound, audioSource);
    }

    void Update()
    {
        if (bcc.jump)
        {
            if(rb.velocity.y < 0) PlayGlideSound();
        }
        else if (wasJumpingLastFrame)
        {
            StopGlideSound();
        }

        wasJumpingLastFrame = bcc.jump;
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
