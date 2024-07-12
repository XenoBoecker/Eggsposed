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
    }

    private float SpeedMultiplier()
    {
        return SpeedPerCharge.Evaluate(currentCharge / maxCharge) * maxSpeedMultiplier;
    }

    private float TurnChargeIntoSpeedMultiplier()
    {
        if (charging) return 0;

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
    }

    private void StartCharging()
    {
        charging = true;
    }

    private void StopCharging()
    {
        charging = false;
    }
}