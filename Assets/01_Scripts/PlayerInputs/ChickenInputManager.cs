using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public class ChickenInputManager : MonoBehaviour
{
    Chicken chicken;
    BaseChickenController chickenController;
    private void Start()
    {
        chicken = GetComponent<Chicken>();
        chickenController = GetComponent<BaseChickenController>();
    }

    public void Move(Vector2 dir)
    {
        if(chickenController == null) chickenController = GetComponent<BaseChickenController>();

        chickenController.moveDirection = new Vector3(dir.x, 0, dir.y);
    }

    public void Jump()
    {
        chickenController.jump = true;
    }

    public void StopJump()
    {
        chickenController.jump = false;
    }

    public void SitDown()
    {
        chickenController.SitDown();
    }

    public void StandUp()
    {
        if (chickenController == null) chickenController = GetComponent<BaseChickenController>();
        
        chickenController.StandUp();
    }

    public void PickupDropEgg()
    {
        chicken.PickupDropEgg();
    }

    public void Call()
    {
        if (chicken == null) chicken = GetComponent<Chicken>();

        chicken.Call();
    }
}