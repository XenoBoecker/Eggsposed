﻿using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ChaseState : BaseState
{
    private Transform _target;
    private float _catchupSpeedMultiplier;
    private float _catchupMinDistance;
    private float _maxSpeedBoostDuration;
    private float _disallowHidingMultiplier;
    private float _speedBoostStartTime;
    private float _speedBoostCooldown;
    float _speedBoostCDTimer;

    public ChaseState(FarmerStateMachine stateMachine, float catchupSpeedMultiplier, float catchupMinDistance, float maxSpeedBoostDuration, float disallowHidingMultiplier) : base(stateMachine)
    {
        _catchupSpeedMultiplier = catchupSpeedMultiplier;
        _catchupMinDistance = catchupMinDistance;
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

        if (_target == GameManager.Instance.Player.transform)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.farmerSFX.collectingPlayersEggWarningSound, _stateMachine.Audiosource);
        }
    }

    private void Chase()
    {
        float distanceToTarget = Vector3.Distance(_stateMachine.transform.position, _target.position);

        if (distanceToTarget >= _catchupMinDistance && _speedBoostStartTime == 0f && _speedBoostCDTimer >= _speedBoostCooldown)
        {
            StartSpeedBoost();
        }

        if (_speedBoostStartTime > 0f)
        {
            if (distanceToTarget <= _catchupMinDistance || Time.time - _speedBoostStartTime >= _maxSpeedBoostDuration)
            {
                EndSpeedBoost();
            }
        }

        if (distanceToTarget <= _stateMachine.CollectionRange) OnEnterCollectionRange();
        else if (_stateMachine.HasReachedDestination())
        {
            _stateMachine.SetNoHiding(true);
        }
        else
        {
            _stateMachine.SetNoHiding(false);
        }

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
