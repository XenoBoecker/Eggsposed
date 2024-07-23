using UnityEngine;

public class HydraSetup : ChickenAbilitySetup
{
    [SerializeField] int callCharges = 3;

    int currentCharges;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        currentCharges = callCharges;
    }

    protected override void SetCallOnCooldown()
    {
        if (currentCharges > 1) currentCharges -= 1;
        else
        {
            base.SetCallOnCooldown();
            currentCharges = callCharges;
        }
    }
}