﻿using System;
using UnityEngine;

public class ThickenSetup : ChickenAbilitySetup
{
    [SerializeField] float breedTimeMultiplier = 0.7f;
    [SerializeField] float moveSpeedMultiplier = 0.7f;
    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);
        
        BaseChickenController bcc = chicken.GetComponent<BaseChickenController>();

        
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