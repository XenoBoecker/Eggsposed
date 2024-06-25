using ECM.Controllers;
using UnityEngine;

public class FarmerAgentController : BaseAgentController
{

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

}
