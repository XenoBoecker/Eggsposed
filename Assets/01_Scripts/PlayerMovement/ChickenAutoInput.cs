using UnityEngine;

public enum ChickenState
{
    Idle,
    Patrol,
    Flee
}

public class ChickenAutoInput : MonoBehaviour
{
    [SerializeField] float randomPositionRange = 10;

    [SerializeField] float farmerDetectionRadius = 10;

    [SerializeField] float minWaitTime = 1;
    [SerializeField] float maxWaitTime = 5;

    [SerializeField] float fleeDistance = 6f;


    [SerializeField] float layingEggCD = 11f;

    [SerializeField] float callCD = 13f;

    ChickenAgentController agentController;

    FarmerAutoInput farmer;

    // timer to wait before setting a new random destination has random range

    float idleTimer;

    ChickenState currentState;

    static float lastEggLayingTime = 0;

    static float lastCallTime = 0;

    void Start()
    {
        if (lastEggLayingTime == 0) lastEggLayingTime = Time.time;
        if (lastCallTime == 0) lastCallTime = Time.time;
        

        farmer = FindObjectOfType<FarmerAutoInput>();

        agentController = GetComponent<ChickenAgentController>();
        agentController.SetDestination(transform.position + Random.insideUnitSphere * randomPositionRange);

        SetState(ChickenState.Idle);
    }

    void Update()
    {

        idleTimer -= Time.deltaTime;

        if (currentState == ChickenState.Idle)
        {
            if(idleTimer <= 0) SetState(ChickenState.Patrol);
        }
        else if (currentState == ChickenState.Flee || currentState == ChickenState.Patrol)
        {
            if (agentController.HasReachedDestination()) SetState(ChickenState.Idle);
        }
        
        if (Vector3.Distance(transform.position, farmer.transform.position) < fleeDistance) SetState(ChickenState.Flee);
    }

    void SetState(ChickenState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case ChickenState.Idle:
                agentController.SetDestination(transform.position);
                idleTimer = Random.Range(minWaitTime, maxWaitTime);

                CheckLayEgg();
                CheckCall();
                break;
            case ChickenState.Patrol:
                agentController.SetDestination(transform.position + Random.insideUnitSphere * randomPositionRange);
                break;
            case ChickenState.Flee:
                Vector3 fleeDirection = transform.position - farmer.transform.position;
                fleeDirection.y = 0;
                fleeDirection.Normalize();
                agentController.SetDestination(transform.position + fleeDirection * fleeDistance);
                break;
        }
    }

    private void CheckLayEgg()
    {
        if (Time.time - lastEggLayingTime > layingEggCD)
        {
            lastEggLayingTime = Time.time;
            GameManager.Instance.SpawnEgg(GetComponent<Chicken>().EggDropPosition.position);
        }
    }

    private void CheckCall()
    {
        if (Time.time - lastCallTime > callCD)
        {
            lastCallTime = Time.time;
            GetComponent<Chicken>().Call();
        }
    }
}