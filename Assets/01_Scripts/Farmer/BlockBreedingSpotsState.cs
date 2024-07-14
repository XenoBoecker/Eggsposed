using UnityEngine;
using System.Collections;

public class BlockBreedingSpotsState : BaseState
{
    float _blockRange;

    bool _isBlocking;
    float blockingTime;
    float blockingTimer;

    public BlockBreedingSpotsState(StateMachine stateMachine, float blockRange, float blockingTime) : base(stateMachine)
    {
        _blockRange = blockRange;
        this.blockingTime = blockingTime;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_isBlocking) return;

        _stateMachine.MoveTo(_stateMachine.Target.position);

        if (Vector3.Distance(_stateMachine.transform.position, _stateMachine.Target.position) < _blockRange)
        {
            _stateMachine.StartCoroutine(BlockBreedingSpot());
        }

        _stateMachine.CheckTargetInSight();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnBlockingFinished()
    {
        base.OnBlockingFinished();
        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    public IEnumerator BlockBreedingSpot()
    {
        _isBlocking = true;
        blockingTimer = 0f;

        while (blockingTimer < blockingTime)
        {
            blockingTimer += Time.deltaTime;
            yield return null;
        }
        _stateMachine.Target.GetComponent<BreedingSpot>().BlockSpot();
    }
}
