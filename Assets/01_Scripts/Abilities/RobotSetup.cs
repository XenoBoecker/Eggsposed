using UnityEngine;

public class RobotSetup : ChickenAbilitySetup
{
    [SerializeField] FarmerIndicator farmerIndicatorPrefab;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        Instantiate(farmerIndicatorPrefab, transform);
    }
}
