using ECM.Components;
using UnityEngine;

public class WindUpSetup : ChickenAbilitySetup
{
    [SerializeField] float startChargeTime = 10;

    [SerializeField] float chargeTimeToUnchargeTimeRatio = 2f;
    [SerializeField] float chargeTimeToSpeedMultiplierRatio = 0.5f;

    float movementMaxLateralSpeed;
    float bccSpeed;

    float timeCharged;
    bool charging;

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
        
        timeCharged -= Time.deltaTime * chargeTimeToUnchargeTimeRatio;
        if (timeCharged < 0) timeCharged = 0;

        float multiplier = timeCharged * chargeTimeToSpeedMultiplierRatio;

        movement.maxLateralSpeed = movementMaxLateralSpeed * multiplier;
        bcc.speed = bccSpeed * multiplier;

        return multiplier;
    }
    
    protected override void Update()
    {
        base.Update();

        if (charging)
        {
            timeCharged += Time.deltaTime;
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