﻿using ECM.Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseChickenController : BaseCharacterController
{
    [Header("Chicken Controller")]

    [SerializeField]
    float _glideMaxFallSpeed = 3.0f;

    [SerializeField] float _glideSpeedMultiplier = 2f;

    [SerializeField] float _glideRotateMultiplier = 0.5f;

    [SerializeField]
    float _breedTime = 20f;

    bool _sitting;
    bool _breeding;
    float _breedTimer = 0;

    List<BreedingSpot> breedingSpots = new List<BreedingSpot>();

    float _baseMaxFallSpeed;
    float _baseSpeed;
    float currentGlideSpeedMultiplier = 1f;
    float currentGlideRotateMultiplier = 1f;
    
    bool wasGlidingLastFrame;
    bool lastFrameJump;

    public event Action OnFinishBreeding;
    public event Action OnSitDown;
    public event Action OnStandUp;

    public delegate Vector3 UpdateRotationDelegate(Vector3 moveDirection);
    public event UpdateRotationDelegate OnUpdateRotationStart;

    public delegate Vector3 CalcDesiredVelocityDelegate(Vector3 moveDirection);
    public event CalcDesiredVelocityDelegate OnCalcDesiredVelocityStart;

    public delegate float AddSpeedMultiplierDelegate();
    public event AddSpeedMultiplierDelegate OnAddSpeedMultiplier;

    public delegate float AddMaxSpeedMultiplierDelegate();
    public event AddMaxSpeedMultiplierDelegate OnAddMaxSpeedMultiplier;

    public delegate float AddBreedMultiplierDelegate();
    public event AddBreedMultiplierDelegate OnAddBreedMultiplier;

    public bool sitting
    {
        get { return _sitting; }
    }

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
            currentGlideSpeedMultiplier = _glideSpeedMultiplier;
            currentGlideRotateMultiplier = _glideRotateMultiplier;

            if (!wasGlidingLastFrame)
            {
                speed *= currentGlideSpeedMultiplier;
                movement.maxLateralSpeed *= currentGlideSpeedMultiplier;
                angularSpeed *= currentGlideRotateMultiplier;
            }

            wasGlidingLastFrame = true;
        }
        else
        {
            if (wasGlidingLastFrame)
            {
                speed /= currentGlideSpeedMultiplier;
                movement.maxLateralSpeed /= currentGlideSpeedMultiplier;
                angularSpeed /= currentGlideRotateMultiplier;
            }
            
            //movement.glideGravityMultiplier = 1.0f;
            movement.maxFallSpeed = _baseMaxFallSpeed;
            currentGlideSpeedMultiplier = 1f;
            currentGlideRotateMultiplier = 1f;


            wasGlidingLastFrame = false;
        }

    }

    void Start()
    {
        _baseMaxFallSpeed = movement.maxFallSpeed;
        _baseSpeed = speed;
    }

    public void SitDown()
    {
        Debug.Log("SitDown");
        _sitting = true;


        if (GetComponent<Chicken>().HasEgg)
        {
            if (breedingSpots.Count == 0) breedingSpots.AddRange(FindObjectsOfType<BreedingSpot>());

            foreach (BreedingSpot spot in breedingSpots)
            {
                if (spot.CanBreed(transform.position))
                {
                    spot.StartBreeding(this);
                    _breeding = true;
                    break;
                }
            }
        }
        
        OnSitDown?.Invoke();
    }

    public void StandUp()
    {
        Debug.Log("Stand Up");
        _sitting = false;
        _breeding = false;
        
        OnStandUp?.Invoke();
    }

    protected override Vector3 CalcDesiredVelocity()
    {
        if (sitting)
        {
            Debug.Log("Chicken is sitting");
            return Vector3.zero;
        }

        if (OnCalcDesiredVelocityStart != null)
        {
            moveDirection = OnCalcDesiredVelocityStart(moveDirection);
        }

        if (moveDirection.z == 0) return Vector3.zero;

        float speedMultiplier = 1.0f;

        if (OnAddSpeedMultiplier != null)
        {
            speedMultiplier *= OnAddSpeedMultiplier();
        }
        speedMultiplier *= currentGlideSpeedMultiplier;

        return transform.forward * speed * speedMultiplier * Mathf.Max(0, moveDirection.z);
    }

    protected override void UpdateRotation()
    {
        if (OnUpdateRotationStart != null) moveDirection = OnUpdateRotationStart.Invoke(moveDirection);
        
        movement.Rotate(transform.right * moveDirection.x, angularSpeed * Mathf.Abs(moveDirection.x));
    }

    protected override void Move()
    {
        // Apply movement

        speed = _baseSpeed;
            
        if (OnAddMaxSpeedMultiplier != null)
        {
            speed *= OnAddMaxSpeedMultiplier();
        }
        speed *= currentGlideSpeedMultiplier;

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


        lastFrameJump = _jump;
    }

    private void Breed()
    {
        crouch = sitting;
        if (!breeding) return;
        
        float breedTimeMultiplier = 1;

        if (OnAddBreedMultiplier != null)
        {
            breedTimeMultiplier *= OnAddBreedMultiplier();
        }
            
        _breedTimer += Time.deltaTime / breedTimeMultiplier;




        if (_breedTimer >= _breedTime)
        {
            _sitting = false;
            _breedTimer = 0;

            HatchNewChicken();
        }
    }

    protected override void Jump()
    {
        if (lastFrameJump) return;

        base.Jump();

    }

    private void HatchNewChicken()
    {
        print("Hatching new chicken");
        OnFinishBreeding?.Invoke();
    }

    protected override void HandleInput()
    {
        
    }

    internal void StopBreeding()
    {
        _breeding = false;
    }
}
