using UnityEngine;

public class RobotSetup : ChickenAbilitySetup
{
    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        GetComponentInChildren<RobotFarmerIndicator>().Activate();
    }
}
