using ECM.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ChickenVisualParts
{
    public ChickenData data;

    public GameObject[] headObjectsToEnable, bodyObjectsToEnable, tailObjectsToEnable;
}
public class Chicken : MonoBehaviour
{
    [SerializeField] bool _isControlledByPlayer;
    public bool IsControlledByPlayer => _isControlledByPlayer;

    [SerializeField] float pickupRange = 5f;

    [SerializeField] float withEggSpeedMultiplier = 1, withoutEggSpeedMultiplier = 1.2f;
    
    [SerializeField] SkinnedMeshRenderer eyeL, eyeR, head, torso, wings, tail;

    [SerializeField] Behaviour[] playerControlComponents;
    [SerializeField] Behaviour[] aiControlComponents;


    [SerializeField]
    ChickenVisualParts[] chickenVisualParts;


    [SerializeField] Transform eggCarryPosition, eggDropPosition;


    [SerializeField] ChickenData torturedChickenData;
    public Transform EggDropPosition => eggDropPosition;

    BaseChickenController bcc;
    CharacterMovement movement;

    ChickenData headData, bodyData;
    public ChickenData HeadData => headData;
    public ChickenData BodyData => bodyData;

    public List<ChickenAbilitySetup> abilities = new List<ChickenAbilitySetup>();

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
    public event Action<bool> OnCall;

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

            if(GameManager.Instance != null) SetInputMethod(GameManager.Instance.KinectInputs);
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

        Collider[] colliders = myEgg.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        myEgg.GetComponent<Rigidbody>().isKinematic = false;

        hasEgg = false;

        bcc.StopBreeding();

        timeSinceEggDropped = 0;
    }

    private void PickUpEgg()
    {
        print("pickup egg");
        myEgg.transform.position = eggCarryPosition.position;
        myEgg.transform.rotation = eggCarryPosition.rotation;

        myEgg.transform.parent = eggCarryPosition;

        Collider[] colliders = myEgg.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
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
        ChickenVisualParts headTailParts = null, bodyParts = null;

        foreach (ChickenVisualParts parts in chickenVisualParts)
        {
            if (parts.data == headTailChickenData)
            {
                headTailParts = parts;
            }
            if(parts.data == bodyChickenData)
            {
                bodyParts = parts;
            }
        }

        headData = headTailChickenData;
        bodyData = bodyChickenData;


        EnableParts(headTailParts.headObjectsToEnable);

        EnableParts(bodyParts.bodyObjectsToEnable);

        if (headTailParts.tailObjectsToEnable.Length == 0) EnableParts(bodyParts.tailObjectsToEnable);
        else if (bodyParts.tailObjectsToEnable.Length == 0) { }
        else EnableParts(headTailParts.tailObjectsToEnable);

        //SetHeadVisuals(headTailChickenData);
        //
        //SetBodyVisual(bodyChickenData);
        //
        //if (headTailChickenData.tail == null)
        //{
        //    SetTailVisuals(bodyChickenData);
        //}else if(bodyChickenData.tail == null)
        //{
        //    // no tail (only for tortured chicken body)
        //}
        //else
        //{
        //    SetTailVisuals(headTailChickenData);
        //}
    }

    private void EnableParts(GameObject[] partsToEnable)
    {
        foreach (GameObject part in partsToEnable)
        {
            part.SetActive(true);
        }

    }

    // private void SetHeadVisuals(ChickenData data)
    // {
    //     SetMeshAndMaterial(eyeL, data.eyeL);
    // 
    //     SetMeshAndMaterial(eyeR, data.eyeR);
    // 
    //     SetMeshAndMaterial(head, data.head);
    // }
    // 
    // private void SetBodyVisual(ChickenData data)
    // {
    //     SetMeshAndMaterial(torso, data.torso);
    // 
    //     SetMeshAndMaterial(wings, data.wings);
    // }
    // 
    // private void SetTailVisuals(ChickenData data)
    // {
    //     SetMeshAndMaterial(tail, data.tail);
    // }
    // 
    // void SetMeshAndMaterial(SkinnedMeshRenderer bodyPart, SkinnedMeshRenderer newBodyPart)
    // {
    //     bodyPart.sharedMesh = newBodyPart.sharedMesh;
    //     bodyPart.sharedMaterial = newBodyPart.sharedMaterial;
    // }

    internal float GetCallCDPercentage()
    {
        OnGetCallCooldown?.Invoke();

        return CurrentCallCooldownPercentage;
    }

    internal bool CheckCanCall()
    {
        if (OnCheckCanCallEvent == null) return false;
        
        int canCall = 0;

        foreach (ChickenAbilitySetup ability in abilities)
        {
            if (ability.CanCall()) canCall++;
        }   

        // canCall += OnCheckCanCallEvent.Invoke();

        print("Check Can Call:" + canCall);

        if (canCall > 0) return true;
        else return false;
    }

    internal void Call(bool activateOnCDCalls = true)
    {
        if (CheckCanCall())
        {
            CurrentCallCooldownPercentage = 0;
            
            OnCall?.Invoke(activateOnCDCalls);
        }
    }
}
