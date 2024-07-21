using ECM.Components;
using System;
using UnityEngine;

public class LightningMcChickSetup : ChickenAbilitySetup
{
    [SerializeField] float accelerationPerSecond = 0.2f;

    [SerializeField] float resetAccelerationThreshold = 0.7f;


    [SerializeField] GameObject lightningEffects;

    float currentSpeedMultiplier = 1.0f;

    float currentSpeed = 0.0f;

    ChickenStateTracker cst;
    AudioSource audioSource;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        bcc.OnCalcDesiredVelocityStart += SetMoveInputToForward;

        bcc.OnAddSpeedMultiplier += GetCurrentSpeedMultiplier;
        bcc.OnAddMaxSpeedMultiplier += GetCurrentSpeedMultiplier;
        movement.OnAddMaxSpeedMultiplier += GetCurrentSpeedMultiplier;
        cst = GetComponent<ChickenStateTracker>();

        cst.OnSitDown += StartSittingSound;
        cst.OnStandUp += StopSittingSound;

        audioSource = new GameObject().AddComponent<AudioSource>();
        audioSource.transform.position = transform.position;
        audioSource.transform.SetParent(transform);

        Instantiate(lightningEffects, transform);
    }

    private void StartSittingSound()
    {
        SoundManager.Instance.StartLoopingSound(SoundManager.Instance.chickenSFX.electricalSittingSound, audioSource);
    }

    private void StopSittingSound()
    {
        SoundManager.Instance.EndLoopingSound(audioSource);
    }

    protected override void Update()
    {
        base.Update();

        currentSpeedMultiplier += accelerationPerSecond * Time.deltaTime;

        if (movement.velocity.magnitude >= currentSpeed * resetAccelerationThreshold)
        {
            currentSpeed = movement.velocity.magnitude;
        }
        else
        {
            currentSpeedMultiplier = 1.0f;
            currentSpeed = 0;
        }
    }

    private float GetCurrentSpeedMultiplier()
    {
        return currentSpeedMultiplier;
    }

    private Vector3 SetMoveInputToForward(Vector3 moveDirection)
    {
        return new Vector3(moveDirection.x, moveDirection.y, 1);
    }
}
