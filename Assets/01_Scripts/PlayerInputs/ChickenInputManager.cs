using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;

public class ChickenInputManager : MonoBehaviour
{
    Chicken chicken;
    BaseChickenController chickenController;
    private void Awake()
    {
        chicken = GetComponent<Chicken>();
        chickenController = GetComponent<BaseChickenController>();
    }

    public void Move(Vector2 dir)
    {
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

    public void StartBreed()
    {
        chickenController.TryStartBreeding();
    }

    public void StopBreed()
    {
        chickenController.StopBreeding();
    }

    public void PickupDropEgg()
    {
        chicken.PickupDropEgg();
    }
}