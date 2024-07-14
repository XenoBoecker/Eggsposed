using ECM.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAutoInput : MonoBehaviour
{
    enum FarmerState
    {
        Patrol,
        Scan,
        Chase,
        Search,
        Collect
    }

    [SerializeField] float randomPositionRange = 10;

    [SerializeField] float scanRotSpeed = 60;

    [SerializeField] float callReactionProbability = 1f;
    [SerializeField] float searchTime = 5f;

    [SerializeField] float detectionRange = 30;

    [SerializeField] float maxViewAngle;

    [SerializeField] float pickupRange = 3;
    [SerializeField] LayerMask detectionMask;
    [SerializeField] Transform eyes;

    List<Egg> allEggs = new List<Egg>();
    Chicken playerChicken;
    Transform currentTarget;

    FarmerAgentController agentController;
    CharacterMovement movement;

    FarmerState currentState;

    // timer to wait before setting a new random destination has random range

    Vector3 targetLastSeenPosition;

    Vector3 startScanForwardDirection;
    Vector3 targetDirection;
    int rotStep;

    float searchTimer;


    void Start()
    {
        agentController = GetComponent<FarmerAgentController>();
        movement = GetComponent<CharacterMovement>();
        agentController.SetDestination(transform.position + Random.insideUnitSphere * randomPositionRange);

        StartCoroutine(RefreshEggListRoutine());

        playerChicken = GameManager.Instance.Player;
        GameManager.Instance.OnSpawnChicken += RefreshPlayerChicken;

        SetState(FarmerState.Scan);
    }

    private void RefreshPlayerChicken()
    {
        playerChicken = GameManager.Instance.Player;
    }

    void Update()
    {
        switch (currentState)
        {
            case FarmerState.Patrol:
                Patrol();
                break;
            case FarmerState.Scan:
                Scan();
                break;
            case FarmerState.Chase:
                Chase();
                break;
            case FarmerState.Search:
                Search();
                break;
            case FarmerState.Collect:
                Collect();
                break;
        }
        
        PickupEggsInRange();
    }

    private void Patrol()
    {
        if (agentController.HasReachedDestination())
        {
            SetState(FarmerState.Scan);
        }
        else
        {
            currentTarget = GetClosestEggInSight();
            if (currentTarget != null) SetState(FarmerState.Chase);
        }
    }

    private void Scan()
    {
        RotateTowardsTargetDirection();

        if (rotationCompleted())
        {
            switch (rotStep)
            {
                case 0:
                    rotStep = 1;
                    targetDirection = GetVectorRotated(startScanForwardDirection, -120);
                    break;
                case 1:
                    rotStep = 2;
                    targetDirection = startScanForwardDirection;
                    break;
                case 2:
                    SetState(FarmerState.Patrol);
                    break;
            }
        }

        currentTarget = GetClosestEggInSight();
        if (currentTarget != null) SetState(FarmerState.Chase);
    }

    private void RotateTowardsTargetDirection()
    {
        movement.Rotate(targetDirection, scanRotSpeed, true);
    }

    private bool rotationCompleted()
    {
        return AreVectorsApproximatelyEqual(transform.forward, targetDirection, 0.1f);
    }

    Vector3 GetVectorRotated(Vector3 inputVector, float yRotation)
    {
        return Quaternion.Euler(0, yRotation, 0) * inputVector;
    }
    bool AreVectorsApproximatelyEqual(Vector3 vec1, Vector3 vec2, float tolerance)
    {
        return Vector3.SqrMagnitude(vec1 - vec2) < tolerance * tolerance;
    }

    private void Chase()
    {
        if (CanSee(currentTarget))
        {
            targetLastSeenPosition = currentTarget.position;
        }
        else
        {
            if (agentController.HasReachedDestination())
            {
                SetState(FarmerState.Scan);
            }
        }

        agentController.SetDestination(targetLastSeenPosition);
    }

    private void Search()
    {
        searchTimer += Time.deltaTime;

        if (searchTimer > searchTime || agentController.HasReachedDestination())
        {
            SetState(FarmerState.Patrol);
        }
    }

    private void Collect()
    {
        throw new System.NotImplementedException();
    }

    void SetState(FarmerState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;

        switch (newState)
        {
            case FarmerState.Patrol:
                SetRandomDestination();
                break;
            case FarmerState.Scan:
                InitScanState();
                break;
            case FarmerState.Search:
                searchTimer = 0;
                break;
        }
    }

    private void SetRandomDestination()
    {
        agentController.SetDestination(Random.insideUnitSphere * randomPositionRange);
    }

    private void InitScanState()
    {
        startScanForwardDirection = transform.forward;
        rotStep = 0;
        targetDirection = GetVectorRotated(startScanForwardDirection, 120);
    }

    private void PickupEggsInRange()
    {
        foreach (Egg egg in allEggs)
        {
            if (egg == null) continue;
            if (Vector3.Distance(transform.position, egg.transform.position) < pickupRange)
            {
                currentTarget = null;
                egg.Pickup();
                RefreshEggList();
            }
        }

        if (playerChicken != null && Vector3.Distance(transform.position, playerChicken.transform.position) < pickupRange)
        {
            playerChicken.TakeEgg();
            currentTarget = null;
        }
    }

    private Transform GetClosestEggInSight()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Egg egg in allEggs)
        {
            if (egg == null) continue;

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
            float distance = Vector3.Distance(transform.position, playerChicken.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = playerChicken.transform;
            }
        }

        return closestTarget;
    }

    private bool CanSee(Transform target)
    {
        if (target == null) return false;

        Vector3 direction = target.position - eyes.position;
        direction.y = 0; // Ignore the vertical component

        Vector3 forward = eyes.forward;
        forward.y = 0; // Ignore the vertical component

        float angle = Vector3.Angle(forward, direction);

        if (angle > maxViewAngle / 2) return false;

        RaycastHit hit;
        if (Physics.Raycast(eyes.position, direction.normalized, out hit, detectionRange, detectionMask))
        {
            if (hit.collider.transform == target)
            {
                return true;
            }
        }
        return false;
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

    internal void HearCall(Vector3 position)
    {
        if (Random.Range(0, 1f) > callReactionProbability) return;

        agentController.SetDestination(position);
        SetState(FarmerState.Search);
    }
}