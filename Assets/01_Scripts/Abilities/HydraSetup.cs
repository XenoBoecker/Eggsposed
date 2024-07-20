using UnityEngine;

public class HydraSetup : ChickenAbilitySetup
{
    [SerializeField] int callCharges = 3;

    [SerializeField] float callChargeCD = 5;

    float currentChargeTime;

    protected override void Update()
    {
        base.Update();
        

        if(currentChargeTime < callChargeCD * callCharges) currentChargeTime += Time.deltaTime;
    }

    public override bool CanCall()
    {
        base.CanCall();

        if (currentChargeTime >= callChargeCD) return true;
        else return false;
    }

    protected override void SetCallOnCooldown()
    {
        currentChargeTime -= callChargeCD;
    }

    protected override void SetChickenCallCDPercentage()
    {
        base.SetChickenCallCDPercentage();

        if (currentChargeTime > callChargeCD)
        {
            chicken.CurrentCallCooldownPercentage = 1;
        }
        else
        {
            chicken.CurrentCallCooldownPercentage = currentChargeTime / callChargeCD;
        }
    }
}