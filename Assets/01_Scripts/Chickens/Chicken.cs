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

    internal void SetChickenVisuals(ChickenData newChickenData, ChickenData oldChickenData, int count)
    {
        if (count % 2 == 0)
        {
            // head and tail new

            SetHeadVisuals(newChickenData);

            SetBodyVisual(oldChickenData);

            if (newChickenData.tail == null)
            {
                SetTailVisuals(oldChickenData);
            }
            else
            {
                SetTailVisuals(newChickenData);
            }
        }
        else
        {
            // body new

            SetHeadVisuals(oldChickenData);

            SetBodyVisual(newChickenData);

            if (oldChickenData.tail == null)
            {
                SetTailVisuals(newChickenData);
            }
            else
            {
                SetTailVisuals(oldChickenData);
            }
        }
    }

    private void SetHeadVisuals(ChickenData data)
    {
        eyeL.sharedMesh = data.eyeL.sharedMesh;
        eyeL.material = data.eyeL.material;

        eyeR.sharedMesh = data.eyeR.sharedMesh;
        eyeR.material = data.eyeR.material;

        head.sharedMesh = data.head.sharedMesh;
        head.material = data.head.material;
    }

    private void SetBodyVisual(ChickenData data)
    {
        torso.sharedMesh = data.torso.sharedMesh;
        torso.material = data.torso.material;

        wings.sharedMesh = data.wings.sharedMesh;
        wings.material = data.wings.material;
    }

    private void SetTailVisuals(ChickenData data)
    {
        tail.sharedMesh = data.tail.sharedMesh;
        tail.material = data.tail.material;
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
