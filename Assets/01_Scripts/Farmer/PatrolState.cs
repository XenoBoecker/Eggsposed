using UnityEngine;
using System.Collections.Generic;

public class PatrolState : BaseState
{
    public List<Transform> PatrolPoints;
    private Queue<Transform> _patrolQueue;
    private Transform _currentTarget;

    public PatrolState(StateMachine stateMachine, List<Transform> patrolPoints) : base(stateMachine)
    {
        PatrolPoints = patrolPoints;
        InitializePatrolQueue();
    }

    private void InitializePatrolQueue()
    {
        _patrolQueue = new Queue<Transform>(PatrolPoints);
        RandomizePatrolQueue();
    }

    private void RandomizePatrolQueue()
    {
        List<Transform> points = new List<Transform>(_patrolQueue);
        _patrolQueue.Clear();
        while (points.Count > 0)
        {
            int index = Random.Range(0, points.Count);
            _patrolQueue.Enqueue(points[index]);
            points.RemoveAt(index);
        }
    }

    public override void Enter()
    {
        base.Enter();

        if (_patrolQueue.Count == 0)
        {
            InitializePatrolQueue();
        }

        MoveToNextPatrolPoint();
    }

    public override void Update()
    {
        base.Update();
        if (_stateMachine.HasReachedDestination())
        {
            _patrolQueue.Dequeue();
            _stateMachine.OnReachDestination();
        }

        _stateMachine.CheckCanBlockBreedingSpot();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnReachDestination()
    {
        base.OnReachDestination();
        _stateMachine.ChangeState(_stateMachine.ScanState);
    }

    private void MoveToNextPatrolPoint()
    {
        if (_patrolQueue.Count == 0)
        {
            InitializePatrolQueue();
        }
        _currentTarget = _patrolQueue.Dequeue();
        _stateMachine.MoveTo(_currentTarget.position);
    }
}
