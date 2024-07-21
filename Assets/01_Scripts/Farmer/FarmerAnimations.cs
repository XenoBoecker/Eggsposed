using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimations : MonoBehaviour
{
    FarmerStateMachine _stateMachine;

    [SerializeField] Transform wheelsFront, wheelBack, wheelsTop;

    [SerializeField] Transform rotateBodyForCharge, rotateArmsForCharge;

    [SerializeField] float bodyRotationForCharge, armsRotationForCharge;

    [SerializeField] float intoChargeRotSpeed = 1.0f;

    [SerializeField] float outOfChargeRotSpeed = 1f;

    [SerializeField] float wheelRotSpeed = 360f;

    bool charging;
    bool lastFrameCharging;

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
            
            if (_stateMachine.CurrentSpeedMultiplier > 1)
            {
                charging = true;
                if (!lastFrameCharging)
                {
                    StopAllCoroutines();
                    StartCoroutine(RotateBodyAndArmsForCharge());
                }
            }
            else
            {
                charging = false;

                if (lastFrameCharging)
                {
                    StopAllCoroutines();
                    StartCoroutine(RotateBodyAndArmsOutOfCharge());
                }
            }
        }

        lastFrameCharging = charging;
    }

    private IEnumerator RotateBodyAndArmsForCharge()
    {
        while (charging)
        {
            rotateBodyForCharge.localRotation = Quaternion.Euler(Mathf.LerpAngle(rotateBodyForCharge.localRotation.eulerAngles.z, bodyRotationForCharge, Time.deltaTime * intoChargeRotSpeed), 0, 0);
            rotateArmsForCharge.localRotation = Quaternion.Euler(Mathf.LerpAngle(rotateArmsForCharge.localRotation.eulerAngles.z, armsRotationForCharge, Time.deltaTime * intoChargeRotSpeed),0, 0);
            yield return null;
        }

    }

    private IEnumerator RotateBodyAndArmsOutOfCharge()
    {
        while (!charging)
        {
            rotateBodyForCharge.localRotation = Quaternion.Euler(Mathf.LerpAngle(rotateBodyForCharge.localRotation.eulerAngles.z, 0, Time.deltaTime * outOfChargeRotSpeed), 0, 0);
            rotateArmsForCharge.localRotation = Quaternion.Euler(Mathf.LerpAngle(rotateArmsForCharge.localRotation.eulerAngles.z, 0, Time.deltaTime * outOfChargeRotSpeed), 0, 0);
            yield return null;
        }
    }

    void RollWheels(float speed)
    {
        wheelsFront.Rotate(Vector3.right, speed * Time.deltaTime * wheelRotSpeed);
        wheelBack.Rotate(Vector3.right, speed * Time.deltaTime * wheelRotSpeed);

        if (charging) wheelsTop.Rotate(Vector3.right, speed * Time.deltaTime * wheelRotSpeed);
    }
}
