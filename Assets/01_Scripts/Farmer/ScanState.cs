using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;

public class ScanState : BaseState
{
    public float ScanAngle;
    public float TurnSpeed;

    int rotStep;

    Vector3 startScanForwardDirection;
    Vector3 targetDirection;

    public ScanState(StateMachine stateMachine, float scanAngle, float turnSpeed) : base(stateMachine)
    {
        ScanAngle = scanAngle;
        TurnSpeed = turnSpeed;
    }

    public override void Enter()
    {
        base.Enter();
        rotStep = 0;

        startScanForwardDirection = _stateMachine.transform.forward;
    }

    public override void Update()
    {
        base.Update();
        Scan();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnScanFinished()
    {
        base.OnScanFinished();
        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    private void Scan()
    {
        _stateMachine.RotateTowards(targetDirection, TurnSpeed);

        if (rotationCompleted())
        {
            switch (rotStep)
            {
                case 0:
                    rotStep = 1;
                    targetDirection = GetVectorRotated(startScanForwardDirection, -ScanAngle/2);
                    break;
                case 1:
                    rotStep = 2;
                    targetDirection = GetVectorRotated(startScanForwardDirection, ScanAngle / 2); ;
                    break;
                case 2:
                    targetDirection = startScanForwardDirection;
                    break;
                case 3:
                    OnScanFinished();
                    break;
            }
            rotStep++;
        }

        _stateMachine.CheckTargetInSight();
    }

    private bool rotationCompleted()
    {
        return AreVectorsApproximatelyEqual(_stateMachine.transform.forward, targetDirection, 0.1f);
    }

    Vector3 GetVectorRotated(Vector3 inputVector, float yRotation)
    {
        return Quaternion.Euler(0, yRotation, 0) * inputVector;
    }
    bool AreVectorsApproximatelyEqual(Vector3 vec1, Vector3 vec2, float tolerance)
    {
        return Vector3.SqrMagnitude(vec1 - vec2) < tolerance * tolerance;
    }
}