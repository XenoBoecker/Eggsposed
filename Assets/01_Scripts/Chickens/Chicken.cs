using ECM.Components;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] bool _isControlledByPlayer;
    public bool IsControlledByPlayer => _isControlledByPlayer;

    [SerializeField] float pickupRange = 5f;

    [SerializeField] float withEggSpeedMultiplier = 1, withoutEggSpeedMultiplier = 1.2f;
    
    [SerializeField] SkinnedMeshRenderer eyeL, eyeR, head, torso, wings, tail;

    [SerializeField] Behaviour[] playerControlComponents;
    [SerializeField] Behaviour[] aiControlComponents;


    [SerializeField] Transform eggCarryPosition, eggDropPosition;
    public Transform EggDropPosition => eggDropPosition;

    BaseChickenController bcc;
    CharacterMovement movement;

    ChickenData headData, bodyData;
    public ChickenData HeadData => headData;
    public ChickenData BodyData => bodyData;

    Egg myEgg;
    public Egg Egg => myEgg;

    bool hasEgg;


    [SerializeField] float timeDelayAfterDropEggBeforePickupPossible = 3f;
    float timeSinceEggDropped;

    public bool HasEgg => hasEgg;

    public event Action OnFinishBreeding;

    public float CurrentCallCooldownPercentage;
    public event Action OnGetCallCooldown;

    public delegate int OnCheckCanCall();
    public event OnCheckCanCall OnCheckCanCallEvent;
    public event Action OnCall;

    // Start is called before the first frame update
    void Start()
    {
        bcc = GetComponent<BaseChickenController>();
        bcc.OnFinishBreeding += HatchEgg;
        bcc.OnAddSpeedMultiplier += CarryEggSpeedMultiplier;
        bcc.OnAddMaxSpeedMultiplier += CarryEggSpeedMultiplier;

        movement = bcc.movement;
        movement.OnAddMaxSpeedMultiplier += CarryEggSpeedMultiplier;


        SetControlledByPlayer(_isControlledByPlayer);
    }

    private float CarryEggSpeedMultiplier()
    {
        if (hasEgg)
        {
            return withEggSpeedMultiplier;
        }
        else return withoutEggSpeedMultiplier;
    }

    private void Update()
    {
        timeSinceEggDropped += Time.deltaTime;

        if (myEgg != null && !hasEgg && timeSinceEggDropped > timeDelayAfterDropEggBeforePickupPossible)
        {
            if (Vector3.Distance(transform.position, myEgg.transform.position) < pickupRange)
            {
                PickUpEgg();
            }
        }
    }

    public void SetControlledByPlayer(bool isControlledByPlayer)
    {
        _isControlledByPlayer = isControlledByPlayer;

        if (_isControlledByPlayer)
        {
            ActivateComponents(playerControlComponents);
            DeactivateComponents(aiControlComponents);

            SetInputMethod(GameManager.Instance.KinectInputs);
        }
        else
        {
            ActivateComponents(aiControlComponents);
            DeactivateComponents(playerControlComponents);

        }
    }

    private void ActivateComponents(Behaviour[] behaviours)
    {
        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour != null)
            {
                behaviour.enabled = true;
            }
        }
    }

    // Method to deactivate a list of components
    private void DeactivateComponents(Behaviour[] behaviours)
    {
        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour != null)
            {
                behaviour.enabled = false;
            }
        }
    }

    void SetInputMethod(bool kinectInputs)
    {
        if (kinectInputs)
        {
            foreach (Behaviour component in playerControlComponents)
            {
                if (component is PCPlayerInputManager) component.enabled = false;
            }
        }
        else
        {
            foreach (Behaviour component in playerControlComponents)
            {
                if (component is KinectInputManager) component.enabled = false;
            }
        }
    }

    public void SetEgg(Egg egg)
    {
        myEgg = egg;
        egg.SetPlayersEgg(_isControlledByPlayer);
        PickUpEgg();
    }

    void HatchEgg()
    {
        OnFinishBreeding?.Invoke();
        Destroy(myEgg.gameObject);
    }

    public void DropEgg()
    {
        if (myEgg == null) return;

        if (!hasEgg) return;
        
        myEgg.transform.position = eggDropPosition.position;
        myEgg.transform.parent = null;

        myEgg.GetComponentInChildren<Collider>().enabled = true;
        myEgg.GetComponent<Rigidbody>().isKinematic = false;

        hasEgg = false;

        timeSinceEggDropped = 0;
    }

    private void PickUpEgg()
    {
        myEgg.transform.position = eggCarryPosition.position;
        myEgg.transform.rotation = eggCarryPosition.rotation;

        myEgg.transform.parent = eggCarryPosition;

        myEgg.GetComponentInChildren<Collider>().enabled = false;
        myEgg.GetComponent<Rigidbody>().isKinematic = true;

        hasEgg = true;
    }

    public void TakeEgg()
    {
        if (hasEgg)
        {
            hasEgg = false;
            myEgg.Pickup();
        }
    }

    internal void SetChickenVisuals(ChickenData headTailChickenData, ChickenData bodyChickenData)
    {
        headData = headTailChickenData;
        bodyData = bodyChickenData;

        SetHeadVisuals(headTailChickenData);

        SetBodyVisual(bodyChickenData);

        if (headTailChickenData.tail == null)
        {
            SetTailVisuals(bodyChickenData);
        }
        else
        {
            SetTailVisuals(headTailChickenData);
        }
    }

    private void SetHeadVisuals(ChickenData data)
    {
        SetMeshAndMaterial(eyeL, data.eyeL);

        SetMeshAndMaterial(eyeR, data.eyeR);

        SetMeshAndMaterial(head, data.head);
    }

    private void SetBodyVisual(ChickenData data)
    {
        SetMeshAndMaterial(torso, data.torso);

        SetMeshAndMaterial(wings, data.wings);
    }

    private void SetTailVisuals(ChickenData data)
    {
        SetMeshAndMaterial(tail, data.tail);
    }

    void SetMeshAndMaterial(SkinnedMeshRenderer bodyPart, SkinnedMeshRenderer newBodyPart)
    {
        bodyPart.sharedMesh = newBodyPart.sharedMesh;
        bodyPart.sharedMaterial = newBodyPart.sharedMaterial;
    }

    internal float GetCallCDPercentage()
    {
        OnGetCallCooldown?.Invoke();

        return CurrentCallCooldownPercentage;
    }

    internal bool CheckCanCall()
    {
        if (OnCheckCanCallEvent == null) return false;
        
        int canCall = 0;
        canCall += OnCheckCanCallEvent.Invoke();

        if (canCall > 0) return true;
        else return false;
    }

    internal void Call()
    {
        if (CheckCanCall())
        {
            CurrentCallCooldownPercentage = 0;
            print("Can Call");
            OnCall?.Invoke();
        }
    }
}