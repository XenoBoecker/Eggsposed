using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCPlayerInputManager : ChickenInputManager
{
    PlayerControls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
    }

    private void Update()
    {
        Move(controls.Player.Move.ReadValue<Vector2>());
        
        if (controls.Player.Breed.triggered) SitDown();
        if (controls.Player.Breed.phase == InputActionPhase.Waiting) StandUp();

        if (controls.Player.Jump.triggered) Jump();
        if (controls.Player.Jump.phase == InputActionPhase.Waiting) StopJump();

        if (controls.Player.PickupDrop.triggered) PickupDropEgg();

        if (controls.Player.Call.triggered) Call();

        // if (controls.Player.Repair.ReadValue<float>() > 0) isRepairing = true;
        // else isRepairing = false;

    }
}
