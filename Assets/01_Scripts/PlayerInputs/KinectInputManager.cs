using System;
using UnityEngine;

public class KinectInputManager : ChickenInputManager
{
    KinectInputs inputs;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (inputs == null) Setup();

        if (Time.timeScale == 0) return;

        Move(inputs.MoveInput);

        // if (controls.Player.PickupDrop.triggered) PickupDropEgg();
        // 
        // if (controls.Player.Call.triggered) Call();

        // if (controls.Player.Repair.ReadValue<float>() > 0) isRepairing = true;
        // else isRepairing = false;

    }

    private void Setup()
    {
        inputs = FindObjectOfType<KinectInputs>();

        if (inputs == null) return;

        inputs.OnSitDown += SitDown;
        inputs.OnStandUp += StandUp;

        inputs.OnJump += Jump;
        inputs.OnStopJump += StopJump;

        inputs.OnDropEgg += PickupDropEgg;
    }
}