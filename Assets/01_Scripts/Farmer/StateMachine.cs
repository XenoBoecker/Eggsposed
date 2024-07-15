using UnityEngine;
using System.Collections.Generic;
using ECM.Components;
using System.Collections;
using System;

public class StateMachine : MonoBehaviour
{
    FarmerAgentController agentController;
    CharacterMovement movement;
    Chicken playerChicken;

    List<Egg> allEggs = new List<Egg>();

    private BaseState _currentState;

    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    public BreedingSpot targetBreedingSpot;

    // State instances
    public PatrolState PatrolState { get; private set; }
    public ScanState ScanState { get; private set; }
    public BlockBreedingSpotsState BlockBreedingSpotsState { get; private set; }
    public CollectEggState CollectEggState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public SearchState SearchState { get; private set; }

    // Inspector variables

    [Header("Vision")]
    [SerializeField] Transform eyes;
    [SerializeField] LayerMask detectionMask;
    [SerializeField] float maxViewAngle = 45f;
    [SerializeField] float detectionRange = 30;


    [Header("Patrol State")]
    [SerializeField] private List<Transform> patrolPoints;

    [Header("Scan State")]
    [SerializeField] private float scanAngle = 90f;
    [SerializeField] private float scanTurnSpeed = 30f;

    [Header("Block Breeding Spots State")]
    [SerializeField] private List<BreedingSpot> breedingSpots;
    [SerializeField] private float blockCooldownTime = 30f;
    [SerializeField] private int minimumUnblockedSpots = 2;
    float blockCooldownTimer;

    [SerializeField] float blockRange = 3f;

    [SerializeField] float blockingTime = 3f;

    [Header("Collect Egg State")]
    [SerializeField] private float collectionRange = 10f;
    public float CollectionRange => collectionRange;
    [SerializeField] private float timeoutRange = 20f;
    [SerializeField] private float collectionDecayRate = 0.5f;
    [SerializeField] private float minimumCollectionDistance = 2f;

    [Header("Chase State")]
    [SerializeField] private float catchupSpeedMultiplier = 1.5f;
    [SerializeField] private float catchupDistance = 15f;
    [SerializeField] private float maxSpeedBoostDuration = 10f;
    [SerializeField] private float disallowHidingMultiplier = 1.2f;
    [SerializeField] private float initialSpeed = 5f;

    [Header("Search State")]
    [SerializeField] private float xRayTrackingTime = 5f;
    [SerializeField] private float minSearchTime = 2f;

    public float InitialCollectionRange { get; private set; }
    float currentSpeedMultiplier;
    public void SetSpeedMultiplier(float multiplier)
    {
        currentSpeedMultiplier = multiplier;

    }

    private void Awake()
    {
        agentController = GetComponent<FarmerAgentController>();
        movement = GetComponent<CharacterMovement>();

        // Initialize states
        PatrolState = new PatrolState(this, patrolPoints);
        ScanState = new ScanState(this, scanAngle, scanTurnSpeed);
        BlockBreedingSpotsState = new BlockBreedingSpotsState(this, blockRange, blockingTime);
        CollectEggState = new CollectEggState(this, target, collectionRange, timeoutRange, collectionDecayRate, minimumCollectionDistance);
        ChaseState = new ChaseState(this, catchupSpeedMultiplier, catchupDistance, maxSpeedBoostDuration, disallowHidingMultiplier);
        SearchState = new SearchState(this, xRayTrackingTime, minSearchTime);

        InitialCollectionRange = collectionRange;
    }

    private void Start()
    {
        StartCoroutine(RefreshEggListRoutine());
        RefreshPlayerChicken();
        GameManager.Instance.OnSpawnChicken += RefreshPlayerChicken;

        // Set initial state
        ChangeState(PatrolState);
    }

    private void Update()
    {
        _currentState?.Update();

        blockCooldownTimer += Time.deltaTime;
    }

    public void ChangeState(BaseState newState)
    {
        if(_currentState != null) print("Change State from " + _currentState.ToString() + " to " + newState.ToString());

        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    // Transition methods
    public void OnReachDestination() => _currentState?.OnReachDestination();
    public void OnScanFinished() => _currentState?.OnScanFinished();
    public void OnCanBlockBreedingSpot() => _currentState?.OnCanBlockBreedingSpot();
    public void OnBlockingFinished() => _currentState?.OnBlockingFinished();
    public void OnTargetDetected() => _currentState?.OnTargetDetected();
    public void OnEnterCollectionRange() => _currentState?.OnEnterCollectionRange();
    public void OnCollectFailed() => _currentState?.OnCollectFailed();
    public void OnCollectSucceded() => _currentState?.OnCollectSucceded();
    public void OnTargetVisionLoss() => _currentState?.OnTargetVisionLoss();
    public void OnReceiveCall()
    {
        _currentState?.OnReceiveCall();

        ChangeState(SearchState);
    }

    // Placeholder methods for moving and speed changes, to be implemented
    public void MoveTo(Vector3 position)
    {
        agentController.SetDestination(position);
    }

    public void RotateTowards(Vector3 targetDirection, float turnSpeed)
    {
        movement.Rotate(targetDirection, turnSpeed, true);
    }

    public void CheckTargetInSight()
    {
        target = TargetInSight();

        Debug.DrawRay(Vector3.zero, Vector3.up * 100);

        if (target != null) ChangeState(ChaseState);
    }
    public Transform TargetInSight()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        print("eggs. " + allEggs.Count);

        foreach (Egg egg in allEggs)
        {
            if (egg == null) continue;

            print("Check egg");

            if (CanSee(egg.transform))
            {
                float distance = Vector3.Distance(transform.position, egg.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = egg.transform;
                }
            }
        }

        if (playerChicken != null && playerChicken.HasEgg && CanSee(playerChicken.transform))
        {
            print("check player");
            float distance = Vector3.Distance(transform.position, playerChicken.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = playerChicken.transform;
            }
        }

        return closestTarget;
    }

    LineRenderer lr;

    public bool CanSee(Transform target)
    {
        if (target == null) return false;

        Vector3 direction = target.position - eyes.position;
        direction.y = 0; // Ignore the vertical component

        Vector3 forward = eyes.forward;
        forward.y = 0; // Ignore the vertical component

        float angle = Vector3.Angle(forward, direction);

        print("angle:" + maxViewAngle / 2);

        if (angle > maxViewAngle / 2) return false;

        print("ASASAS");

        RaycastHit hit;
        if (Physics.Raycast(eyes.position, direction.normalized, out hit, detectionRange, detectionMask))
        {
            if (hit.collider.transform == target)
            {
                print("Can see " + target.name);

                return true;
            }
        }

        if (lr == null) lr = GetComponent<LineRenderer>();

        print("jallojei§");

        lr.SetPosition(0, eyes.position);
        lr.SetPosition(1, target.position);

        Debug.DrawRay(eyes.position, direction.normalized * 30, Color.red);
        Debug.DrawLine(eyes.position, target.position, Color.green);
        Debug.DrawRay(eyes.position, transform.forward * 30, Color.cyan);
        return false;
    }
    private void RefreshPlayerChicken()
    {
        playerChicken = GameManager.Instance.Player;
    }

    private IEnumerator RefreshEggListRoutine()
    {
        while (true)
        {
            RefreshEggList();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void RefreshEggList()
    {
        allEggs = new List<Egg>(FindObjectsOfType<Egg>());
    }

    public bool HasReachedDestination()
    {
        return agentController.HasReachedDestination();
    }

    internal void CheckCanBlockBreedingSpot()
    {
        if (blockCooldownTimer < blockCooldownTime) return;

        int currentlyBlockedBreedingSpots = 0;

        float maxBreedTime = 0;
        BreedingSpot targetSpot = null;

        foreach (BreedingSpot spot in breedingSpots)
        {
            if (spot.IsBlocked()) currentlyBlockedBreedingSpots++;
            else
            {
                if(spot.GetTimeBred() > maxBreedTime)
                {
                    maxBreedTime = spot.GetTimeBred();
                    targetSpot = spot;
                }
            }

            if (breedingSpots.Count - currentlyBlockedBreedingSpots <= minimumUnblockedSpots) return;
        }

        if (targetSpot == null) return;

        targetBreedingSpot = targetSpot;
        ChangeState(BlockBreedingSpotsState);
    }
}