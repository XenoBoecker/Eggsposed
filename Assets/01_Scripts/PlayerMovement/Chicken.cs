using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Chicken : MonoBehaviour
{
    [SerializeField] bool _isControlledByPlayer;

    [SerializeField] float pickupRange = 5f;
    Egg myEgg;

    bool hasEgg;
    public bool HasEgg => hasEgg;

    public event Action OnFinishBreeding;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BaseChickenController>().OnFinishBreeding += HatchEgg;

        SetControlledByPlayer(_isControlledByPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetControlledByPlayer(bool isControlledByPlayer)
    {
        _isControlledByPlayer = isControlledByPlayer;

        if (_isControlledByPlayer)
        {
            GetComponent<ChickenAutoInput>().enabled = false;
            GetComponent<ChickenAgentController>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<BaseChickenController>().enabled = true;
            GetComponent<ChickenInputManager>().enabled = true;
        }
        else
        {
            GetComponent<ChickenAutoInput>().enabled = true;
            GetComponent<ChickenAgentController>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<BaseChickenController>().enabled = false;
            GetComponent<ChickenInputManager>().enabled = false;
            
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
}
