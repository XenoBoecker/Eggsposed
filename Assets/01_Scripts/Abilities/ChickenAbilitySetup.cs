using System;
using UnityEngine;

public class ChickenAbilitySetup : MonoBehaviour
{
    [SerializeField] float callRange = 10;

    FarmerAutoInput farmer;

    public virtual void Setup(Chicken chicken) 
    {
        farmer = FindObjectOfType<FarmerAutoInput>();
    }

    public virtual void Call()
    {
        if (Vector3.Distance(transform.position, farmer.transform.position) < callRange)
        {
            farmer.HearCall(transform.position);
        }
    }
}
