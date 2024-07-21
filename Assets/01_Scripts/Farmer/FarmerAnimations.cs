using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimations : MonoBehaviour
{
    FarmerStateMachine _stateMachine;


    [SerializeField] Animator anim;


    [SerializeField] Transform wheelsFront, wheelBack, wheelsTop;

    [SerializeField] float wheelRotSpeed = 360f;

    bool charging;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<FarmerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stateMachine.HasReachedDestination())
        {
            RollWheels(_stateMachine.CurrentSpeedMultiplier);

            if (anim == null) return;
            if (_stateMachine.CurrentSpeedMultiplier > 1)
            {
                charging = true;
                anim.SetBool("Charging", true);
            }
            else
            {
                charging = false;
                anim.SetBool("Charging", false);
            }
        }
    }

    void RollWheels(float speed)
    {
        wheelsFront.Rotate(Vector3.right, speed * Time.deltaTime * wheelRotSpeed);
        wheelBack.Rotate(Vector3.right, speed * Time.deltaTime * wheelRotSpeed);

        if (charging) wheelsTop.Rotate(Vector3.right, speed * Time.deltaTime * wheelRotSpeed);
    }
}
