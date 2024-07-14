using UnityEngine;

public class SearchState : BaseState
{
    private float _xRayTrackingTime;
    private float _minTrackingTime;

    Transform target;
    private Vector3 _lastKnownPosition;
    private float _currentTrackingTime;

    public SearchState(StateMachine stateMachine, float xRayTrackingTime, float minTrackingTime) : base(stateMachine)
    {
        _xRayTrackingTime = xRayTrackingTime;
        _minTrackingTime = minTrackingTime;
    }

    public override void Enter()
    {
        base.Enter();
        _currentTrackingTime = 0f;
        target = _stateMachine.Target;
        _lastKnownPosition = target.position;
    }

    public override void Update()
    {
        base.Update();
        Search();

        if(_currentTrackingTime > _minTrackingTime) _stateMachine.CheckTargetInSight();

        if (_stateMachine.HasReachedDestination())
        {
            _stateMachine.ChangeState(_stateMachine.PatrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnReceiveCall()
    {
        base.OnReceiveCall();
        // Additional logic for receiving call
    }

    private void Search()
    {
        if (_currentTrackingTime < _xRayTrackingTime)
        {
            _currentTrackingTime += Time.deltaTime;
            // Logic to track the movement of the target
            _lastKnownPosition = target.position;
        }

        _stateMachine.MoveTo(_lastKnownPosition); // Assuming there's a MoveTo method
    }
}
