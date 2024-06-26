using System;
using UnityEngine;

public class ThickenSetup : ChickenAbilitySetup
{
    [SerializeField] float breedTimeMultiplier = 0.7f;
    [SerializeField] float moveSpeedMultiplier = 0.7f;
    public override void Setup(Chicken chicken)
    {
        BaseChickenController bcc = chicken.BaseChickenController;

        
        bcc.OnAddSpeedMultiplier += SpeedMultiplier;
        bcc.OnAddBreedMultiplier += BreedTimeMultiplier;
    }

    private float SpeedMultiplier()
    {
        return moveSpeedMultiplier;
    }

    private float BreedTimeMultiplier()
    {
        return breedTimeMultiplier;
    }
}