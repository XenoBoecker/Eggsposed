using System.Collections.Generic;
using UnityEngine;

public class FarmerAutoInput : MonoBehaviour
{
    [SerializeField] float randomPositionRange = 10;

    [SerializeField] float minWaitTime = 1;
    [SerializeField] float maxWaitTime = 3;
    [SerializeField] float followOutOfSightTargetTime = 5f;

    [SerializeField] float detectionRange = 30;
    [SerializeField] float pickupRange = 3;
    [SerializeField] LayerMask detectionMask;
    [SerializeField] Transform eyes;

    List<Egg> allEggs;
    Chicken playerChicken;
    Transform currentTarget;

    FarmerAgentController agentController;

    // timer to wait before setting a new random destination has random range

    float waitToGetNewDestinationTimer;
    float findNewTargetTimer = 0;



    void Start()
    {
        agentController = GetComponent<FarmerAgentController>();
        agentController.SetDestination(Random.insideUnitSphere * randomPositionRange);

        InvokeRepeating("RefreshEggList", 0, 0.5f);

        playerChicken = GameManager.Instance.Player;
        GameManager.Instance.OnSpawnChicken += RefreshPlayerChicken;
    }

    private void RefreshPlayerChicken()
    {
        playerChicken = GameManager.Instance.Player;
    }

    void Update()
    {

        waitToGetNewDestinationTimer -= Time.deltaTime;

        if (agentController.HasReachedDestination() && waitToGetNewDestinationTimer <= 0)
        {
            agentController.SetDestination(Random.insideUnitSphere * randomPositionRange);
            waitToGetNewDestinationTimer = Random.Range(minWaitTime, maxWaitTime);
        }

        ScanForEggs();

        PickupEggs();
    }

    private void PickupEggs()
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

    private void ScanForEggs()
    {
        if(currentTarget != null)
        {
            Vector3 direction = currentTarget.position - eyes.position;
            RaycastHit hit;
            if (Physics.Raycast(eyes.position, direction, out hit, detectionRange, detectionMask))
            {
                if (hit.collider.transform == currentTarget)
                {
                    print("Can see: " + currentTarget.name);
                    agentController.SetDestination(currentTarget.position);

                    findNewTargetTimer = 0;
                    
                    return;
                }
                else
                {
                    findNewTargetTimer += Time.deltaTime;
                    if (findNewTargetTimer < followOutOfSightTargetTime || agentController.HasReachedDestination())
                    {
                        return;
                    }
                    else
                    {
                        currentTarget = null;
                        findNewTargetTimer = 0;
                    }
                }
            }
        }
        
        List<Vector3> eggPositions = new List<Vector3>();

        foreach (Egg egg in allEggs)
        {
            if (egg == null) continue;
            // cast ray from eyes to egg
            Vector3 direction = egg.transform.position - eyes.position;
            RaycastHit hit;
            if (Physics.Raycast(eyes.position, direction, out hit, detectionRange, detectionMask))
            {
                Egg foundEgg = hit.collider.GetComponent<Egg>();
                if (foundEgg != null)
                {
                    currentTarget = foundEgg.transform;
                    eggPositions.Add(hit.collider.transform.position);
                }
            }
        }

        if (playerChicken != null)
        {
            Vector3 direction = playerChicken.transform.position - eyes.position;
            RaycastHit hit;
            if (Physics.Raycast(eyes.position, direction, out hit, detectionRange, detectionMask))
            {
                Chicken found = hit.collider.GetComponent<Chicken>();
                if (found != null && found.HasEgg)
                {
                    print("Found Player Chicken Egg: " + found.name);
                    currentTarget = found.transform;
                    eggPositions.Add(hit.collider.transform.position);
                }
            }
        }

        float closestDistance = Mathf.Infinity;

        foreach (Vector3 eggPos in eggPositions)
        {
            float distance = Vector3.Distance(transform.position, eggPos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                agentController.SetDestination(eggPos);
            }

        }
    }

    void RefreshEggList()
    {
        allEggs = new List<Egg>(FindObjectsOfType<Egg>());
    }
}