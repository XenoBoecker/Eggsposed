using ECM.Components;
using UnityEngine;

public class WindUpSetup : ChickenAbilitySetup
{
    [SerializeField] float startChargeTime = 10;

    [SerializeField] float chargeGainPerSecond;
    [SerializeField] float chargeLossPerSecond;

    [SerializeField] float maxCharge = 10;
    [SerializeField] float maxSpeedMultiplier = 3;

    [SerializeField] AnimationCurve SpeedPerCharge;

    float movementMaxLateralSpeed;
    float bccSpeed;

    float timeCharged;
    bool charging;

    float currentCharge;


    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        bcc = chicken.GetComponent<BaseChickenController>();
        bcc.OnSitDown += StartCharging;

        movement = bcc.movement;
        
        timeCharged = startChargeTime;

        movementMaxLateralSpeed = movement.maxLateralSpeed;
        bccSpeed = bcc.speed;
        
        
        bcc.OnStandUp += StopCharging;
        
        bcc.OnAddSpeedMultiplier += TurnChargeIntoSpeedMultiplier;
    }
    
    private float TurnChargeIntoSpeedMultiplier()
    {
        if (charging) return 0;

        currentCharge -= Time.deltaTime * chargeLossPerSecond;
        if (timeCharged < 0) timeCharged = 0;

        float multiplier = SpeedPerCharge.Evaluate(currentCharge / maxCharge) * maxSpeedMultiplier;

        movement.maxLateralSpeed = movementMaxLateralSpeed * multiplier;
        bcc.speed = bccSpeed * multiplier;

        return multiplier;
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