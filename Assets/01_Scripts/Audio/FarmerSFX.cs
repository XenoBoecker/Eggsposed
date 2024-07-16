using System;
using UnityEngine;

public class FarmerSFX : MonoBehaviour
{
    private FarmerStateMachine farmer;
    private AudioSource audioSource;
    private Rigidbody rb;

    private void Start()
    {
        farmer = GetComponent<FarmerStateMachine>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();  // Ensure to assign rb in Start or Awake
    }

    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f && !audioSource.isPlaying)
        {
            if (farmer.CurrentSpeedMultiplier == 1)
            {
                PlayWalkSound();
            }
            else
            {
                PlayRunSound();
            }
        }
        else if (rb.velocity.magnitude < 0.1f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayWalkSound()
    {
        audioSource.clip = SoundManager.Instance.farmerSFX.normalSpeedSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void PlayRunSound()
    {
        audioSource.clip = SoundManager.Instance.farmerSFX.chaseSpedSound;
        audioSource.loop = true;
        audioSource.Play();
    }
}
