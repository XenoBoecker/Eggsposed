using ECM.Controllers;
using System;
using UnityEngine;

public class BaseChickenController : BaseCharacterController
{
    [Header("Chicken Controller")]

    [SerializeField]
    float _glideGravityMultiplier = 0.5f;

    [SerializeField]
    float _glideMaxFallSpeed = 3.0f;

    [SerializeField]
    float _breedTime = 20f;

    bool _breeding;
    float _breedTimer = 0;

    float _baseMaxFallSpeed;

    public event Action OnFinishBreeding;

    public bool breeding
    {
        get { return _breeding; }
        set { _breeding = value; }
    }

    public float breedPercentage
    {
        get { return _breedTimer / _breedTime; }
    }

    protected virtual void Glide()
    {
        if (_jump && movement.velocity.y < 0)
        {
            //movement.glideGravityMultiplier = _glideGravityMultiplier;
            movement.maxFallSpeed = _glideMaxFallSpeed;
        }
        else
        {
            //movement.glideGravityMultiplier = 1.0f;
            movement.maxFallSpeed = _baseMaxFallSpeed;
        }

    }

    protected override Vector3 CalcDesiredVelocity()
    {
        if (breeding) return Vector3.zero;
        
        return transform.forward * speed * Mathf.Max(0, moveDirection.z);
    }
    
    public delegate Vector3 UpdateRotationDelegate(Vector3 moveDirection);
    public event UpdateRotationDelegate OnUpdateRotationStart;

    protected override void UpdateRotation()
    {
        if (OnUpdateRotationStart != null)
        {
            print("change direction: " + moveDirection);
            moveDirection = OnUpdateRotationStart.Invoke(moveDirection);
            print("new direction: " + moveDirection);
        }
        
        if (breeding) return;
        movement.Rotate(transform.right * moveDirection.x, angularSpeed);
    }

    protected override void Move()
    {
        // Apply movement

        // If using root motion and root motion is being applied (eg: grounded),
        // move without acceleration / deceleration, let the animation takes full control

        var desiredVelocity = CalcDesiredVelocity();

        if (useRootMotion && applyRootMotion)
            movement.Move(desiredVelocity, speed, !allowVerticalMovement);
        else
        {
            // Move with acceleration and friction

            var currentFriction = isGrounded ? groundFriction : airFriction;
            var currentBrakingFriction = useBrakingFriction ? brakingFriction : currentFriction;

            movement.Move(desiredVelocity, speed, acceleration, deceleration, currentFriction,
                currentBrakingFriction, !allowVerticalMovement);
        }

        Breed();
        // Jump logic

        Jump();
        Glide();
        MidAirJump();
        UpdateJumpTimer();

        // Update root motion state,
        // should animator root motion be enabled? (eg: is grounded)

        applyRootMotion = useRootMotion && movement.isGrounded;
    }

    private void Breed()
    {
        crouch = breeding;
        if (breeding) _breedTimer += Time.deltaTime;

        if (_breedTimer >= _breedTime)
        {
            breeding = false;
            _breedTimer = 0;

            HatchNewChicken();
        }
    }

    private void HatchNewChicken()
    {
        print("Hatching new chicken");
        OnFinishBreeding?.Invoke();
    }

    protected override void HandleInput()
    {
        
    }

    void Start()
    {
        _baseMaxFallSpeed = movement.maxFallSpeed;
    }
}
