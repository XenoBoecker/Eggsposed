using UnityEngine;

public class TorturedSetup : ChickenAbilitySetup
{

    [SerializeField] GameObject bloodEffect;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        Instantiate(bloodEffect, transform);
    }

    protected override void Update()
    {
        base.Update();


        if (CanCall())
        {
            chicken.Call(false);
        }
    }
}