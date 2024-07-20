using System;
using UnityEngine;

public class ChickenSFX : MonoBehaviour
{
    AudioSource audioSource;

    ChickenStateTracker cst;

    bool wasSittingLastFrame;
    bool hadEggLastFrame;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cst = GetComponent<ChickenStateTracker>();

        cst.OnFinishBreeding += PlayHatchingSound;

        cst.OnSitDown += PlaySitDownSound;
        cst.OnStandUp += PlayStandUpSound;

        cst.OnDropEgg += PlayDropEggSound;
        cst.OnPickupEgg += PlayPickupEggSound;
    }

    private void PlayStandUpSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.standUpSound, audioSource);
    }

    private void PlaySitDownSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.sitDownSound, audioSource);
    }

    private void PlayHatchingSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.eggHatchingSound, audioSource);
    }

    private void PlayDropEggSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.dropEggSound, audioSource);
    }

    private void PlayPickupEggSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.pickUpEggSound, audioSource);
    }
}
