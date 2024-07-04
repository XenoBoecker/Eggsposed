using UnityEngine;

public class RotatorChickenSetup : ChickenAbilitySetup
{
    int dir = 1;
    public override void Setup(Chicken chicken)
    {
        chicken.GetComponent<BaseChickenController>().OnUpdateRotationStart += ChangeRotationInput;
        
        if (Random.Range(0, 2) == 0) dir = -1;
    }

    private Vector3 ChangeRotationInput(Vector3 moveDirection)
    {
        return new Vector3(dir, moveDirection.y, moveDirection.z);
    }
}
