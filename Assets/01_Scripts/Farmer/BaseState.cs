public abstract class BaseState
{
    protected StateMachine _stateMachine;

    public BaseState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    // Define transition methods
    public virtual void OnReachDestination() { }
    public virtual void OnScanFinished() { }
    public virtual void OnCanBlockBreedingSpot() { }
    public virtual void OnBlockingFinished() { }
    public virtual void OnTargetDetected() { }
    public virtual void OnEnterCollectionRange() { }
    public virtual void OnCollectFailed() { }
    public virtual void OnCollectSucceded() { }
    public virtual void OnTargetVisionLoss() { }
    public virtual void OnReceiveCall() { }
}
