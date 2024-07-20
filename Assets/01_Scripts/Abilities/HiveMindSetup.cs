using System;
using UnityEngine;

public class HiveMindSetup : ChickenAbilitySetup
{
    Chicken[] allChickens;

    [SerializeField] GameObject seismicWavesEffect;
    
    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        allChickens = FindObjectsOfType<Chicken>();

        foreach (Chicken chick in allChickens)
        {
            chick.SetControlledByPlayer(true);
        }

        bcc.OnFinishBreeding += GiveUpControl;

        Instantiate(seismicWavesEffect, transform);
    }

    private void GiveUpControl()
    {
        foreach (Chicken chick in allChickens)
        {
            chick.SetControlledByPlayer(false);
        }
    }
}