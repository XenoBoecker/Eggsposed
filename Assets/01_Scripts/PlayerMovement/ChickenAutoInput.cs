using UnityEngine;

public class ChickenAutoInput : MonoBehaviour
{
    [SerializeField] float randomPositionRange = 10;

    [SerializeField] float minWaitTime = 1;
    [SerializeField] float maxWaitTime = 5;

    ChickenAgentController agentController;

    // timer to wait before setting a new random destination has random range

    float timer;

    

    void Start()
    {
        agentController = GetComponent<ChickenAgentController>();
        agentController.SetDestination(Random.insideUnitSphere * randomPositionRange);
    }

    void Update()
    {

        timer -= Time.deltaTime;

        if (agentController.HasReachedDestination() && timer <= 0)
        {
            agentController.SetDestination(Random.insideUnitSphere * randomPositionRange);
            timer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

}