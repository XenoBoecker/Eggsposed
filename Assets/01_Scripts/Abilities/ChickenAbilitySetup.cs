using ECM.Components;
using TMPro.EditorUtilities;
using UnityEngine;

public class ChickenAbilitySetup : MonoBehaviour
{
    [SerializeField] float callRange = 10;

    [SerializeField] float callCD = 5;
    float timeSinceLastCall = Mathf.Infinity;

    FarmerAutoInput farmer;

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
        farmer = FindObjectOfType<FarmerAutoInput>();

        chicken.OnCheckCanCallEvent += CanCallInt;
        chicken.OnCall += Call;
    }

    public virtual void Call()
    {
        SetCallOnCooldown();

        if (Vector3.Distance(transform.position, farmer.transform.position) < callRange)
        {
            farmer.HearCall(transform.position);
        }
    }

    int CanCallInt()
    {
        if (CanCall()) return 1;
        else return 0;
    }

    protected virtual bool CanCall()
    {
        print("Checking Can Call");

        if (timeSinceLastCall >= callCD) return true;
        else return false;
    }

    protected virtual void SetCallOnCooldown()
    {
        timeSinceLastCall = 0;
    }
}
