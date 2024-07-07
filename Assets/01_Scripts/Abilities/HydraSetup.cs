using UnityEngine;

public class HydraSetup : ChickenAbilitySetup
{
    [SerializeField] int callCharges = 3;

    [SerializeField] float callChargeCD = 5;

    float currentChargeTime;

    protected override void Update()
    {
        base.Update();

        currentChargeTime += Time.deltaTime;
    }

    protected override bool CanCall()
    {
        base.CanCall();

        if (currentChargeTime >= callChargeCD) return true;
        else return false;
    }

    protected override void SetCallOnCooldown()
    {
        currentChargeTime -= callChargeCD;
    }
}