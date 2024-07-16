using ECM.Components;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStepSounds : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip[] stepSounds;
    float stepInterval; // Interval between step sounds

    Rigidbody rb;
    CharacterMovement movement;

    private List<AudioClip> currentStepSounds;
    private int currentSoundIndex = 0;
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<CharacterMovement>();

        stepSounds = SoundManager.Instance.chickenSFX.stepSounds;

        stepInterval = stepSounds[0].length;

        if (stepSounds == null || stepSounds.Length == 0)
        {
            Debug.LogError("Step sounds list is empty!");
            return;
        }

        currentStepSounds = new List<AudioClip>(stepSounds);
        ShuffleStepSounds();
    }

    void Update()
    {
        if (IsWalking())
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayNextStepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private bool IsWalking()
    {
        return movement.isOnGround && rb.velocity.magnitude > 0;
    }

    private void PlayNextStepSound()
    {
        if (currentStepSounds == null || currentStepSounds.Count == 0)
            return;

        SoundManager.Instance.PlaySound(currentStepSounds[currentSoundIndex], audioSource);
        currentSoundIndex++;

        if (currentSoundIndex >= currentStepSounds.Count)
        {
            AudioClip lastSound = currentStepSounds[currentSoundIndex - 1];
            ShuffleStepSounds();
            if (currentStepSounds[0] == lastSound)
            {
                // If the first sound of the new shuffled list is the same as the last sound of the previous list, swap it with another sound
                int swapIndex = UnityEngine.Random.Range(1, currentStepSounds.Count);
                AudioClip temp = currentStepSounds[0];
                currentStepSounds[0] = currentStepSounds[swapIndex];
                currentStepSounds[swapIndex] = temp;
            }
            currentSoundIndex = 0;
        }
    }

    private void ShuffleStepSounds()
    {
        for (int i = 0; i < currentStepSounds.Count; i++)
        {
            AudioClip temp = currentStepSounds[i];
            int randomIndex = UnityEngine.Random.Range(i, currentStepSounds.Count);
            currentStepSounds[i] = currentStepSounds[randomIndex];
            currentStepSounds[randomIndex] = temp;
        }
    }
}
