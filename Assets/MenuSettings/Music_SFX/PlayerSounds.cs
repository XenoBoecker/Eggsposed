using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    /*
    PlayerMovement playerMovement;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip movementStepSound;

    bool isWalking = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        SoundManager.instance.onSoundReload += Reload;
    }

    void Start()
    {
        if (playerMovement != null)
        {
            playerMovement.OnStartWalking += OnPlayerStartWalking;
            playerMovement.OnStopWalking += OnPlayerStopWalking;
        }
    }

    private void Reload()
    {
        audioSource.volume = SoundManager.instance.GetSFXVolume();
    }

    private void OnPlayerStartWalking()
    {
        if (isWalking) return;
        isWalking = true;
        audioSource.Play();
    }

    private void OnPlayerStopWalking()
    {
        if (!isWalking) return;
        isWalking = false;
        audioSource.Stop();
    }*/
}