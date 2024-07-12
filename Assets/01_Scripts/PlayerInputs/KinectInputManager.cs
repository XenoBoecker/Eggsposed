using System;

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

        inputs.OnSitDown += SitDown;
        inputs.OnStandUp += StandUp;

        inputs.OnJump += Jump;
        inputs.OnStopJump += StopJump;

        inputs.OnDropEgg += PickupDropEgg;
    }
}