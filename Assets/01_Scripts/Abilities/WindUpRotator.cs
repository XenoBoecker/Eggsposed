using System;
using UnityEngine;

public class WindUpRotator : MonoBehaviour
{

    [SerializeField] ChickenData windUpChickenData;

    [SerializeField] GameObject headRotator, bodyRotator;

    [SerializeField] float rotationPerCharge = 50;

    float unwindRotSpeed;
    float windUpRotSpeed;

    WindUpSetup windUpSetup;
        
    Rigidbody rb;

    bool isHead;

    float lastFrameCharge;

    private void Start()
    {
        if (GameManager.Instance.PreviousChickenData(0) == windUpChickenData)
        {
            headRotator.SetActive(false);
            bodyRotator.SetActive(true);

            isHead = false;
        }
        else
        {
            headRotator.SetActive(true);
            bodyRotator.SetActive(false);

            isHead = true;
        }
    }

    private void Update()
    {
        if (isHead) Rotate(headRotator);
        else Rotate(bodyRotator);
    }

    private void Rotate(GameObject rotator)
    {
        float currentCharge = windUpSetup.CurrentCharge;

        rotator.transform.Rotate(Vector3.up, rotationPerCharge * (currentCharge - lastFrameCharge));

        lastFrameCharge = currentCharge;

        // if (rb.velocity.magnitude > 0.1f) rotator.transform.Rotate(Vector3.up, unwindRotSpeed * Time.deltaTime);
        // else rotator.transform.Rotate(Vector3.down, windUpRotSpeed * Time.deltaTime);
    }

    internal void SetAbility(WindUpSetup windUpSetup)
    {
        unwindRotSpeed = windUpSetup.ChargeLossPerSecond * rotationPerCharge;

        windUpRotSpeed = windUpSetup.ChargeGainPerSecond * rotationPerCharge;


        rb = windUpSetup.GetComponent<Rigidbody>();

        this.windUpSetup = windUpSetup;
    }
}