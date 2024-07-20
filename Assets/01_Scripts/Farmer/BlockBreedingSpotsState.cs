using UnityEngine;
using System.Collections;

public class BlockBreedingSpotsState : BaseState
{
    float _blockRange;
    float _blockDuration;

    bool _isBlocking;
    float blockingTime;
    float blockingTimer;

    public BlockBreedingSpotsState(FarmerStateMachine stateMachine, float blockRange, float blockingTime, float blockDuration) : base(stateMachine)
    {
        _blockRange = blockRange;
        this.blockingTime = blockingTime;
        _blockDuration = blockDuration;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_isBlocking) return;

        _stateMachine.MoveTo(_stateMachine.targetBreedingSpot.transform.position);

        if (Vector3.Distance(_stateMachine.transform.position, _stateMachine.targetBreedingSpot.transform.position) < _blockRange)
        {
            _stateMachine.StartCoroutine(BlockBreedingSpot());
        }

        _stateMachine.CheckTargetInSight();
    }

    public override void Exit()
    {
        base.Exit();

        _stateMachine.targetBreedingSpot = null;
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

        SoundManager.Instance.PlaySound(SoundManager.Instance.farmerSFX.blockBreedingSpotSound, _stateMachine.Audiosource);

        while (blockingTimer < blockingTime)
        {
            blockingTimer += Time.deltaTime;
            yield return null;
        }
        _stateMachine.targetBreedingSpot.BlockSpot(_blockDuration);
    }
}
