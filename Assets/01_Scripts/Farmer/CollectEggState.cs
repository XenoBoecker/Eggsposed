using UnityEngine;

public class CollectEggState : BaseState
{
    private Transform _target;
    private float _collectionRange;
    private float _timeoutRange;
    private float _decayRate;
    private float _minimumDistance;
    private float _collectionProgress;

    public CollectEggState(FarmerStateMachine stateMachine, Transform target, float collectionRange, float timeoutRange, float decayRate, float minimumDistance) : base(stateMachine)
    {
        _target = target;
        _collectionRange = collectionRange;
        _timeoutRange = timeoutRange;
        _decayRate = decayRate;
        _minimumDistance = minimumDistance;
    }

    public override void Enter()
    {
        base.Enter();
        _collectionProgress = 0f;
    }

    public override void Update()
    {
        base.Update();
        Collect();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnCollectFailed()
    {
        base.OnCollectFailed();
        _stateMachine.ChangeState(_stateMachine.ChaseState);
    }

    public override void OnCollectSucceded()
    {
        base.OnCollectSucceded();
        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    private void Collect()
    {
        float distanceToTarget = Vector3.Distance(_stateMachine.transform.position, _target.position);

        if (distanceToTarget <= _minimumDistance)
        {
            _collectionProgress += Time.deltaTime;
            if (_collectionProgress >= 1f)
            {
                OnCollectSucceded();
            }
        }
        else if (distanceToTarget <= _collectionRange)
        {
            _collectionProgress += Time.deltaTime;
        }
        else if (distanceToTarget <= _timeoutRange)
        {
            // Collection progress stays constant
        }
        else
        {
            _collectionProgress -= _decayRate * Time.deltaTime;
            if (_collectionProgress <= 0f)
            {
                OnCollectFailed();
            }
        }

        // Visual representation logic can be added here
    }
}
