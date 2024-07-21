using ECM.Components;
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
        chicken.OnCall += CallMaybe;
        chicken.OnGetCallCooldown += SetChickenCallCDPercentage;

        this.chicken = chicken;
    }

    protected virtual void SetChickenCallCDPercentage()
    {
        float percentage = timeSinceLastCall / callCD;

        float currentPercentage = chicken.CurrentCallCooldownPercentage;

        if (percentage > currentPercentage)
        {
            chicken.CurrentCallCooldownPercentage = percentage;
        }
    }
    
    protected virtual void CallMaybe(bool activateOnCDCalls)
    {
        if (activateOnCDCalls || CanCall()) Call();
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
        if (Time.timeScale == 0) return false;
        if (timeSinceLastCall >= callCD) return true;
        else return false;
    }

    protected virtual void SetCallOnCooldown()
    {
        timeSinceLastCall = 0;
    }
}
