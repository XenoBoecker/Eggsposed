using ECM.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStateTracker : MonoBehaviour
{

    Rigidbody rb;
    CharacterMovement movement;
    BaseChickenController bcc;
    Chicken chicken;

    public event Action OnJump;
    public event Action OnStartGlide;
    public event Action OnStopGlide;
    public event Action OnStartFalling;
    public event Action OnStopFalling;
    
    public event Action OnStartWalking;
    public event Action OnStopWalking;
    
    public event Action OnStandUp;
    public event Action OnSitDown;
    
    public event Action OnDropEgg;
    public event Action OnPickupEgg;

    public event Action OnLeanLeft;
    public event Action OnLeanRight;
    public event Action OnStopLeaning;

    public event Action OnFinishBreeding;

    public event Action<bool> OnCall;

    public float LeaningDirection => bcc.moveDirection.x;


    bool wasWalkingLastFrame;
    bool wasJumpingLastFrame;
    bool wasFallingLastFrame;
    bool wasGlidingLastFrame;
    bool wasSittingLastFrame;
    bool hadEggLastFrame;
    float moveXInputLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<CharacterMovement>();
        bcc = GetComponent<BaseChickenController>();
        chicken = GetComponent<Chicken>();

        chicken.OnCall += (bool v) => OnCall?.Invoke(v);

        bcc.OnJump += () => OnJump?.Invoke();
        bcc.OnFinishBreeding += () => OnFinishBreeding?.Invoke();

        if (chicken.IsControlledByPlayer) PrintStates();
    }

    private void PrintStates()
    {
        OnJump += () => Debug.Log("Jumped");
        OnStartGlide += () => Debug.Log("Started Gliding");
        OnStopGlide += () => Debug.Log("Stopped Gliding");
        OnStartFalling += () => Debug.Log("Started Falling");
        OnStopFalling += () => Debug.Log("Stopped Falling");
        OnStartWalking += () => Debug.Log("Started Walking");
        OnStopWalking += () => Debug.Log("Stopped Walking");
        OnStandUp += () => Debug.Log("Standing Up");
        OnSitDown += () => Debug.Log("Sitting Down");
        OnDropEgg += () => Debug.Log("Dropped Egg");
        OnPickupEgg += () => Debug.Log("Picked up Egg");
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckWalking();
        CheckJumpingGlidingAndFalling();
        CheckSitAndStand();
        CheckDropPickupEgg();
        CheckLeaning();
        

        wasWalkingLastFrame = IsWalking();
        wasJumpingLastFrame = bcc.jump;
        wasGlidingLastFrame = IsGliding();
        wasFallingLastFrame = IsFalling();
        wasSittingLastFrame = bcc.sitting;
        hadEggLastFrame = chicken.HasEgg;
        moveXInputLastFrame = bcc.moveDirection.x;
    }

    private void CheckLeaning()
    {
        if (bcc.moveDirection.x < 0 && moveXInputLastFrame >= 0)
        {
            OnLeanLeft?.Invoke();
        }
        else if (bcc.moveDirection.x > 0 && moveXInputLastFrame <= 0)
        {
            OnLeanRight?.Invoke();
        }
        else if (bcc.moveDirection.x == 0 && moveXInputLastFrame != 0)
        {
            OnStopLeaning?.Invoke();
        }
    }

    private void CheckDropPickupEgg()
    {
        if (hadEggLastFrame && !chicken.HasEgg)
        {
            OnDropEgg?.Invoke();
        }
        else if (!hadEggLastFrame && chicken.HasEgg)
        {
            OnPickupEgg?.Invoke();
        }
    }

    private void CheckSitAndStand()
    {
        if (bcc.sitting && !wasSittingLastFrame)
        {
            OnSitDown?.Invoke();
        }
        else if (!bcc.sitting && wasSittingLastFrame)
        {
            OnStandUp?.Invoke();
        }
    }

    private bool IsFalling()
    {
        return !movement.isGrounded && rb.velocity.y < -0.1f && !bcc.jump;
    }

    bool IsGliding()
    {
        return rb.velocity.y < -0.1f && bcc.jump;
    }
    
    private void CheckJumpingGlidingAndFalling()
    {
        if (bcc.jump)
        {
            if (!wasJumpingLastFrame)
            {
                OnJump?.Invoke();
            }
            if (!wasGlidingLastFrame && rb.velocity.y < 0) OnStartGlide?.Invoke();
        }
        else
        {
            if (wasGlidingLastFrame)
            {
                OnStopGlide?.Invoke();
            }
            if (IsFalling() && !wasFallingLastFrame)
            {
                OnStartFalling?.Invoke();
            }
        }

        if (!IsFalling() && wasFallingLastFrame)
        {
            OnStopFalling?.Invoke();
        }

    }

    private void CheckWalking()
    {
        if (!wasWalkingLastFrame && IsWalking())
        {
            OnStartWalking?.Invoke();
        }
        else if (wasWalkingLastFrame && !IsWalking())
        {
            OnStopWalking?.Invoke();
        }
    }

    private bool IsWalking()
    {
        return movement.isOnGround && rb.velocity.magnitude > 0;
    }
}
