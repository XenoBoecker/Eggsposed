using ECM.Components;
using UnityEngine;

public class LightningMcChickSetup : ChickenAbilitySetup
{
    [SerializeField] float accelerationPerSecond = 1.0f;
    [SerializeField] float decelerateThreshold = 1;
    [SerializeField] float maxSpeed = 100.0f;
    
    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        bcc.OnCalcDesiredVelocityStart += SetMoveInputToForward;

        movement.maxLateralSpeed = Mathf.Infinity;
        bcc.speed = 100;
        bcc.acceleration = accelerationPerSecond;
    }

    private Vector3 SetMoveInputToForward(Vector3 moveDirection)
    {
        return new Vector3(moveDirection.x, moveDirection.y, 1);
    }
}
