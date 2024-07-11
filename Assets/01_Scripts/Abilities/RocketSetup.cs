using System.Collections;
using UnityEngine;

public class RocketSetup : ChickenAbilitySetup
{
    [SerializeField] float forceScale = 1000;
    [SerializeField] float abilityDuration = 0.5f;
    [SerializeField] AnimationCurve addForceCurve;

    Rigidbody rb;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        rb = GetComponent<Rigidbody>();
    }

    public override void Call()
    {
        base.Call();

        StartCoroutine(AirDash());
    }

    IEnumerator AirDash()
    {

        // Get dash direction and distance based on player input
        Vector3 dashDir = Vector3.up + transform.forward;

        // Check if the destination is valid (not colliding with any obstacles)

        // Smoothly move the player
        float elapsedTime = 0f;

        while (elapsedTime < abilityDuration)
        {
            float t = elapsedTime / abilityDuration;
            float curveValue = addForceCurve.Evaluate(t);

            rb.AddForce(curveValue * forceScale * dashDir);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // chickenVisual.rotation = Quaternion.identity;

        bcc.enabled = true;
        movement.enabled = true;
    }
}