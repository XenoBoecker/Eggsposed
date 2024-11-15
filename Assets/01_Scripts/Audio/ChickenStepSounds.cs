using ECM.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStepSounds : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip[] stepSounds;
    AudioClip[] grassStepSounds;
    float stepInterval; // Interval between step sounds

    ChickenStateTracker cst;

    private List<AudioClip> currentStepSounds;
    private int currentSoundIndex = 0;
    private float stepTimer = 0f;

    bool walkingOnGrass;

    bool isWalking;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cst = GetComponent<ChickenStateTracker>();
        cst.OnStartWalking += () => isWalking = true;
        cst.OnStopWalking += () => isWalking = false;

        stepSounds = SoundManager.Instance.chickenSFX.stepSounds;
        grassStepSounds = SoundManager.Instance.chickenSFX.grassStepSounds;

        stepInterval = stepSounds[0].length;

        if (stepSounds == null || stepSounds.Length == 0)
        {
            Debug.LogError("Step sounds list is empty!");
            return;
        }
        if (grassStepSounds == null)
        {
            Debug.LogError("Grass step sounds list is null!");
            return;
        }

        if (grassStepSounds.Length == 0)
        {
            Debug.LogError("Grass step sounds list is empty!");
            return;
        }

        ShuffleStepSounds();
    }

    void Update()
    {
        if (isWalking)
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
        if (grassStepSounds == null)
        {
            print("no grass step sounds");
            grassStepSounds = SoundManager.Instance.chickenSFX.grassStepSounds;
        }
        if (walkingOnGrass) currentStepSounds = new List<AudioClip>(grassStepSounds);
        else currentStepSounds = new List<AudioClip>(stepSounds);
        
        for (int i = 0; i < currentStepSounds.Count; i++)
        {
            AudioClip temp = currentStepSounds[i];
            
            int randomIndex = UnityEngine.Random.Range(i, currentStepSounds.Count);
            currentStepSounds[i] = currentStepSounds[randomIndex];
            currentStepSounds[randomIndex] = temp;
        }
    }

    internal void SetWalkingOnGrass(bool v)
    {
        walkingOnGrass = v;

        ShuffleStepSounds();
    }
}
