using System;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] bool _isControlledByPlayer;

    [SerializeField] float pickupRange = 5f;

    [SerializeField] GameObject headParent, bodyParent, tailParent;
    [SerializeField] GameObject head, body, tail;

    BaseChickenController baseChickenController;
    public BaseChickenController BaseChickenController => baseChickenController;
    ChickenInputManager chickenInputManager;
    
    ChickenAutoInput chickenAutoInput;
    ChickenAgentController chickenAgentController;

    NavMeshAgent navMeshAgent;

    Egg myEgg;

    bool hasEgg;
    public bool HasEgg => hasEgg;

    public event Action OnFinishBreeding;

    private void Awake()
    {
        baseChickenController = GetComponent<BaseChickenController>();
        chickenInputManager = GetComponent<ChickenInputManager>();
        chickenAutoInput = GetComponent<ChickenAutoInput>();
        chickenAgentController = GetComponent<ChickenAgentController>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        baseChickenController.OnFinishBreeding += HatchEgg;

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
            chickenAutoInput.enabled = false;
            chickenAgentController.enabled = false;
            navMeshAgent.enabled = false;
            baseChickenController.enabled = true;
            chickenInputManager.enabled = true;
        }
        else
        {
            chickenAutoInput.enabled = true;
            chickenAgentController.enabled = true;
            navMeshAgent.enabled = true;
            baseChickenController.enabled = false;
            chickenInputManager.enabled = false;
            
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
}
