using System;
using UnityEngine;

public class WindUpRotator : MonoBehaviour
{

    [SerializeField] ChickenData windUpChickenData;

    [SerializeField] GameObject headRotator, bodyRotator;

    [SerializeField] float rotationPerCharge = 50;

    WindUpSetup windUpSetup;

    float lastFrameCharge;

    private void Update()
    {
        Rotate(headRotator);
        Rotate(bodyRotator);
    }

    private void Rotate(GameObject rotator)
    {
        if (!rotator.activeInHierarchy) return;

        float currentCharge = windUpSetup.CurrentCharge;

        rotator.transform.Rotate(Vector3.up, rotationPerCharge * (currentCharge - lastFrameCharge));

        lastFrameCharge = currentCharge;

        // if (rb.velocity.magnitude > 0.1f) rotator.transform.Rotate(Vector3.up, unwindRotSpeed * Time.deltaTime);
        // else rotator.transform.Rotate(Vector3.down, windUpRotSpeed * Time.deltaTime);
    }

    internal void SetAbility(WindUpSetup windUpSetup)
    {
        this.windUpSetup = windUpSetup;
    }
}