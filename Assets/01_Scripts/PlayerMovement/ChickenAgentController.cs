using ECM.Controllers;
using UnityEngine.AI;
using UnityEngine;

public class ChickenAgentController : BaseAgentController
{
    /// <summary>
    /// Overrides 'BaseCharacterController' HandleInput method,
    /// to perform custom input code, in this case, click-to-move.
    /// </summary>

    protected override void HandleInput()
    {
        
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
