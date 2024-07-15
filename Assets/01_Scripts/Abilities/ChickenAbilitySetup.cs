using ECM.Components;
using System;
using TMPro.EditorUtilities;
using UnityEngine;

public class ChickenAbilitySetup : MonoBehaviour
{
    [SerializeField] float callRange = 10;

    [SerializeField] float callCD = 5;
    float timeSinceLastCall = Mathf.Infinity;

    FarmerStateMachine farmer;

    protected Chicken chicken;
    protected BaseChickenController bcc;
    protected CharacterMovement movement;

    protected virtual void Start()
    {
        Setup(GetComponent<Chicken>());
    }

    protected virtual void Update()
    {
        timeSinceLastCall += Time.deltaTime;
    }

    public virtual void Setup(Chicken chicken)
    {
        bcc = chicken.GetComponent<BaseChickenController>();
        movement = bcc.movement;
        farmer = FindObjectOfType<FarmerStateMachine>();

        chicken.OnCheckCanCallEvent += CanCallInt;
        chicken.OnCall += Call;
        chicken.OnGetCallCooldown += SetChickenCallCDPercentage;

        this.chicken = chicken;
    }

    private void SetChickenCallCDPercentage()
    {
        float percentage = timeSinceLastCall / callCD;

        float currentPercentage = chicken.CurrentCallCooldownPercentage;

        if (percentage > currentPercentage)
        {
            chicken.CurrentCallCooldownPercentage = percentage;
        }
    }

    public virtual void Call()
    {
        SetCallOnCooldown();

        if (Vector3.Distance(transform.position, farmer.transform.position) < callRange - farmer.HearingDistance)
        {
            farmer.HearCall(transform.position);
        }
    }

    int CanCallInt()
    {
        if (CanCall()) return 1;
        else return 0;
    }

    public virtual bool CanCall()
    {
        if (timeSinceLastCall >= callCD) return true;
        else return false;
    }

    protected virtual void SetCallOnCooldown()
    {
        timeSinceLastCall = 0;
    }
}
