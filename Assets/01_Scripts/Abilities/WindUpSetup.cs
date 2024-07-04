using ECM.Components;
using UnityEngine;

public class WindUpSetup : ChickenAbilitySetup
{
    [SerializeField] float startChargeTime = 10;

    [SerializeField] float chargeTimeToUnchargeTimeRatio = 2f;
    [SerializeField] float chargeTimeToSpeedMultiplierRatio = 0.5f;

    float timeCharged;
    bool charging;

    public float test;
    bool testBool;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        print("subscribing noew");

        BaseChickenController bcc = chicken.GetComponent<BaseChickenController>();
        bcc.OnSitDown += StartCharging;

        // CharacterMovement movement = bcc.movement;
        // 
        // print("StartChargeTime: " + startChargeTime);
        // timeCharged = startChargeTime;
        // test = startChargeTime;
        // 
        // movement.maxLateralSpeed = Mathf.Infinity;
        // bcc.speed = 100;
        // 
        // 
        // bcc.OnStandUp += StopCharging;
        // 
        // bcc.OnAddSpeedMultiplier += TurnChargeIntoSpeedMultiplier;
    }
    // 
    // private float TurnChargeIntoSpeedMultiplier()
    // {
    //     if (charging) return 0;
    //     
    //     timeCharged -= Time.deltaTime * chargeTimeToUnchargeTimeRatio;
    //     if (timeCharged < 0) timeCharged = 0;
    //     
    //     return timeCharged * chargeTimeToSpeedMultiplierRatio;
    // }
    // 
    private void Update()
    {
        if (charging)
        {
            print("charging");
            timeCharged += Time.deltaTime;
        }

        if (testBool) print("testBooL");
    }

    private void StartCharging()
    {
        test += 1;
        print("Start Chargenin: test = " + test);
        testBool = true;
        charging = true;
        if (testBool) print("set charging to true");
    }

    private void StopCharging()
    {
        print("Stop Chargenin");
        charging = false;
    }
}