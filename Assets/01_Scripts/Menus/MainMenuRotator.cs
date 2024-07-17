using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotator : MonoBehaviour
{
    MainMenu mainMenu;

    float lastRotation;
    float currentTargetRotation;


    [SerializeField] float rotationDuration;

    float rotationTimer;


    [SerializeField] AnimationCurve rotationCurve;

    private void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
        
        mainMenu.OnInput += UpdateTaretRotation;
        
    }

    private void Update()
    {
        if (rotationTimer > rotationDuration)
        {
            transform.localRotation = Quaternion.Euler(0, currentTargetRotation, 0);
            return;
        }

        rotationTimer += Time.deltaTime;

        float curvePercentage = rotationTimer / rotationDuration;

        float rotValue = rotationCurve.Evaluate(curvePercentage);

        float rotDiff = currentTargetRotation - lastRotation;

        if (Mathf.Abs(rotDiff) > 90) rotDiff = -90 * Mathf.Sign(rotDiff);

        transform.localRotation = Quaternion.Euler(0, lastRotation + rotValue * rotDiff, 0);
    }

    private void UpdateTaretRotation()
    {
        lastRotation = transform.localRotation.eulerAngles.y;

        currentTargetRotation = 360f * mainMenu.CurrentButtonIndex / mainMenu.ButtonCount;
        
        rotationTimer = 0;
    }
}