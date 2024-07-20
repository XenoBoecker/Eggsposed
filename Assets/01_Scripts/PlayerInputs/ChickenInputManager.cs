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
        if (Time.timeScale == 0) return;
        if (chickenController == null) chickenController = GetComponent<BaseChickenController>();

        chickenController.moveDirection = new Vector3(dir.x, 0, dir.y);
    }

    public void Jump()
    {
        if (Time.timeScale == 0) return;
        if (chickenController == null) chickenController = GetComponent<BaseChickenController>();
        chickenController.jump = true;
    }

    public void StopJump()
    {
        if (Time.timeScale == 0) return;
        if (chickenController == null) chickenController = GetComponent<BaseChickenController>();
        chickenController.jump = false;
    }

    public void SitDown()
    {
        if (Time.timeScale == 0) return;
        if (chickenController == null) chickenController = GetComponent<BaseChickenController>();
        chickenController.SitDown();
    }

    public void StandUp()
    {
        if (Time.timeScale == 0) return;
        if (chickenController == null) chickenController = GetComponent<BaseChickenController>();
        
        chickenController.StandUp();
    }

    public void PickupDropEgg()
    {
        if (Time.timeScale == 0) return;
        if (chicken == null) chicken = GetComponent<Chicken>();

        chicken.DropEgg();
    }

    public void Call()
    {
        if (Time.timeScale == 0) return;
        if (chicken == null) chicken = GetComponent<Chicken>();

        chicken.Call();
    }
}