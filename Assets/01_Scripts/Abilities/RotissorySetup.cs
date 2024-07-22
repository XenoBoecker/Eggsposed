using System;
using System.Collections;
using static Leaderboard;
using UnityEngine;

public class RotissorySetup : ChickenAbilitySetup
{
    [SerializeField] float forceScale = 50;
    [SerializeField] float abilityDuration = 2f;
    [SerializeField] AnimationCurve addForceCurve;

    [SerializeField] float maxRotSpeed = 1000;
    [SerializeField] AnimationCurve rotSpeedCurve;
    float curveScaleValue;
    Transform chickenVisual;

    Rigidbody rb;
    AudioSource audioSource;
    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        chickenVisual = transform.GetChild(0);

        curveScaleValue = CalculateCurveScale(rotSpeedCurve, abilityDuration);
    }

    public override void Call()
    {
        base.Call();

        StartCoroutine(AirDash());
    }

    IEnumerator AirDash()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.rotisserieCallSpinSound, audioSource);

        bcc.enabled = false;
        movement.enabled = false;
        GetComponent<ChickenTrailVFX>().StartTrailEffect();

        // Get dash direction and distance based on player input
        Vector3 dashDir = Vector3.up;

        // Check if the destination is valid (not colliding with any obstacles)

        // Smoothly move the player
        float elapsedTime = 0f;

        while (elapsedTime < abilityDuration)
        {
            float t = elapsedTime / abilityDuration;
            float curveValue = addForceCurve.Evaluate(t);

            rb.AddForce(curveValue * forceScale * dashDir);

            float rotCurveValue = rotSpeedCurve.Evaluate(t) * curveScaleValue;

            chickenVisual.Rotate(Vector3.up, Time.deltaTime * maxRotSpeed * rotCurveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // chickenVisual.rotation = Quaternion.identity;

        bcc.enabled = true;
        movement.enabled = true;

        GetComponent<ChickenTrailVFX>().StopTrailEffect();
    }

    float CalculateCurveScale(AnimationCurve curve, float duration)
    {
        float integral = 0f;
        int steps = (int)maxRotSpeed; // Increase for more accuracy
        float stepSize = duration / steps;

        for (int i = 0; i < steps; i++)
        {
            float t = i * stepSize / duration;
            integral += curve.Evaluate(t) * stepSize * maxRotSpeed;
        }

        // Find the closest multiple of 360
        float closestMultiple = Mathf.Round(integral / 360f) * 360f;

        return closestMultiple / integral;
    }
}
