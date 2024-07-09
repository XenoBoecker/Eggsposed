using System;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] bool _isControlledByPlayer;
    public bool IsControlledByPlayer => _isControlledByPlayer;

    [SerializeField] float pickupRange = 5f;

    [SerializeField] GameObject headParent, bodyParent, tailParent;
    [SerializeField] GameObject head, body, tail;

    [SerializeField] Behaviour[] playerControlComponents;
    [SerializeField] Behaviour[] aiControlComponents;

    Egg myEgg;

    bool hasEgg;
    public bool HasEgg => hasEgg;

    public event Action OnFinishBreeding;

    public delegate int OnCheckCanCall();
    public event OnCheckCanCall OnCheckCanCallEvent;
    public event Action OnCall;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BaseChickenController>().OnFinishBreeding += HatchEgg;

        SetControlledByPlayer(_isControlledByPlayer);
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
                if (component is KinectInputs) component.enabled = false;
            }
        }
    }

    public void SetEgg(Egg egg)
    {
        myEgg = egg;
        egg.SetPlayersEgg(_isControlledByPlayer);
        PickupDropEgg();
    }

    void HatchEgg()
    {
        OnFinishBreeding?.Invoke();
        Destroy(myEgg.gameObject);
    }

    public void PickupDropEgg()
    {
        if (myEgg == null) return;

        if (hasEgg)
        {
            myEgg.transform.position = transform.position - transform.forward * 2 + Vector3.up; 
            myEgg.gameObject.SetActive(true);
            hasEgg = false;
        }
        else
        {
            if (Vector3.Distance(transform.position, myEgg.transform.position) < pickupRange)
            {
                myEgg.gameObject.SetActive(false);
                hasEgg = true;
            }
        }
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
        head.SetActive(false);
        body.SetActive(false);
        tail.SetActive(false);

        if(count == 2)
        {
            Instantiate(newChickenData.chickenVisualHead, headParent.transform);
            Instantiate(newChickenData.chickenVisualBody, bodyParent.transform);
            Instantiate(newChickenData.chickenVisualTail, tailParent.transform);
        }
        else if (count % 2 == 0)
        {
            Instantiate(newChickenData.chickenVisualHead, headParent.transform);
            Instantiate(oldChickenData.chickenVisualBody, bodyParent.transform);
            Instantiate(newChickenData.chickenVisualTail, tailParent.transform);
        }
        else
        {
            Instantiate(oldChickenData.chickenVisualHead, headParent.transform);
            Instantiate(newChickenData.chickenVisualBody, bodyParent.transform);
            Instantiate(oldChickenData.chickenVisualTail, tailParent.transform);
        }
    }

    internal void Call()
    {
        int canCall = 0;
        canCall += OnCheckCanCallEvent.Invoke();

        if(canCall > 0) OnCall?.Invoke();
    }
}
