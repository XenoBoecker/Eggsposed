using ECM.Components;
using System;
using UnityEngine;

public class WindUpSetup : ChickenAbilitySetup
{
    [SerializeField] float startCharge = 10;

    [SerializeField] float chargeGainPerSecond;
    public float ChargeGainPerSecond => chargeGainPerSecond;
    [SerializeField] float chargeLossPerSecond;
    public float ChargeLossPerSecond => chargeLossPerSecond;

    [SerializeField] float maxCharge = 10;
    [SerializeField] float maxSpeedMultiplier = 3;

    [SerializeField] AnimationCurve SpeedPerCharge;


    [SerializeField] WindUpRotator rotatorPrefab;
    bool charging;

    float currentCharge;
    public float CurrentCharge => currentCharge;

    AudioSource audioSource;

    ChickenStateTracker cst;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        bcc = chicken.GetComponent<BaseChickenController>();
        bcc.OnSitDown += StartCharging;

        currentCharge = startCharge;

        movement = bcc.movement;

        movement.OnAddMaxSpeedMultiplier += SpeedMultiplier;
        bcc.OnAddMaxSpeedMultiplier += SpeedMultiplier;  
        
        bcc.OnStandUp += StopCharging;
        
        bcc.OnAddSpeedMultiplier += TurnChargeIntoSpeedMultiplier;

        WindUpRotator rotator = Instantiate(rotatorPrefab, transform);
        rotator.SetAbility(this);
        audioSource = rotator.GetComponent<AudioSource>();

        cst = GetComponent<ChickenStateTracker>();
        cst.OnStopWalking += () => SoundManager.Instance.EndLoopingSound(audioSource);
    }

    private float SpeedMultiplier()
    {
        return SpeedPerCharge.Evaluate(currentCharge / maxCharge) * maxSpeedMultiplier;
    }

    private float TurnChargeIntoSpeedMultiplier()
    {
        if (charging) return 0;

        if (!audioSource.isPlaying) SoundManager.Instance.StartLoopingSound(SoundManager.Instance.chickenSFX.unwindSound, audioSource);

        currentCharge -= Time.deltaTime * chargeLossPerSecond;
        if (currentCharge < 0) currentCharge = 0;

        return SpeedMultiplier();
    }
    
    protected override void Update()
    {
        base.Update();

        if (charging && currentCharge < maxCharge)
        {
            currentCharge += Time.deltaTime * chargeGainPerSecond;
        }
        else
        {
            SoundManager.Instance.EndLoopingSound(audioSource);
        }
    }

    private void StartCharging()
    {
        charging = true;

        SoundManager.Instance.StartLoopingSound(SoundManager.Instance.chickenSFX.windupSound, audioSource);
    }

    private void StopCharging()
    {
        charging = false;

        SoundManager.Instance.EndLoopingSound(audioSource);
    }
}