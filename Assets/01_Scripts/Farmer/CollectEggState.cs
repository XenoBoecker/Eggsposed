using UnityEngine;

public class CollectEggState : BaseState
{
    private Transform _target;
    float _collectionTime;
    private float _collectionRange;
    private float _timeoutRange;
    private float _decayRate;
    private float _minimumDistance;
    private float _collectionProgress;

    bool lastFrameInCollectionRange;
    bool lastFrameAlmostCollected;

    public CollectEggState(FarmerStateMachine stateMachine, float collectionTime, float collectionRange, float timeoutRange, float decayRate, float minimumDistance) : base(stateMachine)
    {
        _collectionTime = collectionTime;
        _collectionRange = collectionRange;
        _timeoutRange = timeoutRange;
        _decayRate = decayRate;
        _minimumDistance = minimumDistance;
    }

    public override void Enter()
    {
        base.Enter();
        _collectionProgress = 0f;

        _target = _stateMachine.Target;
    }

    public override void Update()
    {
        base.Update();

        Collect();
    }

    public override void Exit()
    {
        base.Exit();

        _stateMachine.SetNoHiding(false);
    }

    public override void OnCollectFailed()
    {
        base.OnCollectFailed();
        _stateMachine.ChangeState(_stateMachine.ChaseState);
    }

    public override void OnCollectSucceded()
    {
        base.OnCollectSucceded();

        Egg targetEgg = _target.GetComponent<Egg>();

        if (targetEgg != null)
        {
            targetEgg.Pickup();
        }
        else
        {
            Chicken chicken = _target.GetComponent<Chicken>();
            if (chicken != null) chicken.TakeEgg();
        }
        
        

        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    private void Collect()
    {
        float distanceToTarget = Vector3.Distance(_stateMachine.transform.position, _target.position);

        if (distanceToTarget > _minimumDistance) _stateMachine.MoveTo(_target.position);
        else _stateMachine.StopMoving();

        if (distanceToTarget <= _collectionRange)
        {
            _collectionProgress += Time.deltaTime;

            if (_collectionProgress >= _collectionTime)
            {
                OnCollectSucceded();
            }
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

        bool almostCollected = _collectionProgress > _collectionTime * 0.75f;

        if (distanceToTarget <= _collectionRange)
        {
            if (!lastFrameInCollectionRange) SoundManager.Instance.PlaySound(SoundManager.Instance.farmerSFX.inCollectRangeSound, _stateMachine.Audiosource);
            if(!lastFrameAlmostCollected && almostCollected) SoundManager.Instance.PlaySound(SoundManager.Instance.farmerSFX.eggAlmostCollectedWarningSound, _stateMachine.Audiosource);


            lastFrameInCollectionRange = true;
            lastFrameAlmostCollected = almostCollected;
        }
        else
        {
            lastFrameInCollectionRange = false;
        }


        // Visual representation logic can be added here
    }
}
