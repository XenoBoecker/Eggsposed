using UnityEngine;

public class ChaseState : BaseState
{
    private Transform _target;
    private float _catchupSpeedMultiplier;
    private float _catchupDistance;
    private float _maxSpeedBoostDuration;
    private float _disallowHidingMultiplier;
    private float _speedBoostStartTime;
    private float _speedBoostCooldown;
    float _speedBoostCDTimer;

    public ChaseState(FarmerStateMachine stateMachine, float catchupSpeedMultiplier, float catchupDistance, float maxSpeedBoostDuration, float disallowHidingMultiplier) : base(stateMachine)
    {
        _catchupSpeedMultiplier = catchupSpeedMultiplier;
        _catchupDistance = catchupDistance;
        _maxSpeedBoostDuration = maxSpeedBoostDuration;
        _disallowHidingMultiplier = disallowHidingMultiplier;
    }

    public override void Enter()
    {
        base.Enter();
        _speedBoostStartTime = 0f;
        _target = _stateMachine.Target;
    }

    public override void Update()
    {
        base.Update();
        Chase();

        _speedBoostCDTimer += Time.deltaTime;

        if (!_stateMachine.CanSee(_target)) OnTargetVisionLoss();
    }

    public override void Exit()
    {
        base.Exit();
        ResetSpeed();
    }

    public override void OnTargetVisionLoss()
    {
        base.OnTargetVisionLoss();
        _stateMachine.ChangeState(_stateMachine.SearchState);
    }

    public override void OnEnterCollectionRange()
    {
        base.OnEnterCollectionRange();
        _stateMachine.ChangeState(_stateMachine.CollectEggState);
    }

    private void Chase()
    {
        float distanceToTarget = Vector3.Distance(_stateMachine.transform.position, _target.position);

        if (distanceToTarget >= _catchupDistance && _speedBoostStartTime == 0f && _speedBoostCDTimer >= _speedBoostCooldown)
        {
            StartSpeedBoost();
        }

        if (_speedBoostStartTime > 0f)
        {
            if (distanceToTarget <= _catchupDistance || Time.time - _speedBoostStartTime >= _maxSpeedBoostDuration)
            {
                EndSpeedBoost();
            }
        }

        if (distanceToTarget <= _stateMachine.CollectionRange) OnEnterCollectionRange();

        _stateMachine.MoveTo(_target.position);
    }

    private void StartSpeedBoost()
    {
        _speedBoostStartTime = Time.time;
        _stateMachine.SetSpeedMultiplier(_catchupSpeedMultiplier);
        _speedBoostCDTimer = 0;
    }

    private void EndSpeedBoost()
    {
        _stateMachine.SetSpeedMultiplier(1f);
        _speedBoostStartTime = 0f;
    }

    private void ResetSpeed()
    {
        _stateMachine.SetSpeedMultiplier(1f);
    }
}
