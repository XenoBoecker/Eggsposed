using ECM.Controllers;
using System;
using System.Collections.Generic;
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

    List<BreedingSpot> breedingSpots = new List<BreedingSpot>();

    float _baseMaxFallSpeed;

    public event Action OnFinishBreeding;


    public delegate Vector3 UpdateRotationDelegate(Vector3 moveDirection);
    public event UpdateRotationDelegate OnUpdateRotationStart;

    public delegate Vector3 CalcDesiredVelocityDelegate(Vector3 moveDirection);
    public event CalcDesiredVelocityDelegate OnCalcDesiredVelocityStart;

    public delegate float AddSpeedMultiplierDelegate();
    public event AddSpeedMultiplierDelegate OnAddSpeedMultiplier;

    public delegate float AddBreedMultiplierDelegate();
    public event AddBreedMultiplierDelegate OnAddBreedMultiplier;

    public bool breeding
    {
        get { return _breeding; }
    }

    public float breedTime
    {
        get { return _breedTime; }
        set { _breedTime = value; }
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

    public void TryStartBreeding()
    {
        if (breedingSpots.Count == 0) breedingSpots.AddRange(FindObjectsOfType<BreedingSpot>());

        foreach (BreedingSpot spot in breedingSpots)
        {
            if (spot.IsCloseEnough(transform.position))
            {
                print("Go Breed");
                _breeding = true;
                break;
            }
        }
    }

    public void StopBreeding()
    {
        _breeding = false;
    }

    protected override Vector3 CalcDesiredVelocity()
    {
        if (breeding) return Vector3.zero;

        if (OnCalcDesiredVelocityStart != null)
        {
            moveDirection = OnCalcDesiredVelocityStart(moveDirection);
        }

        float speedMultiplier = 1.0f;

        if (OnAddSpeedMultiplier != null)
        {
            speedMultiplier *= OnAddSpeedMultiplier();
        }

        return transform.forward * speed * speedMultiplier * Mathf.Max(0, moveDirection.z);
    }

    protected override void UpdateRotation()
    {
        if (OnUpdateRotationStart != null) moveDirection = OnUpdateRotationStart.Invoke(moveDirection);
        
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
        if (!breeding) return;
        
        float breedTimeMultiplier = 1;

        if (OnAddBreedMultiplier != null)
        {
            breedTimeMultiplier *= OnAddBreedMultiplier();
        }
            
        _breedTimer += Time.deltaTime / breedTimeMultiplier;




        if (_breedTimer >= _breedTime)
        {
            _breeding = false;
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
