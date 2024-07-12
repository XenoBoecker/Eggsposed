using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotator : MonoBehaviour
{
    MainMenu mainMenu;

    float lastRotation;
    float currentTargetRotation;


    [SerializeField] float rotationSpeed = 5;


    [SerializeField] AnimationCurve rotationCurve;

    private void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
        
        mainMenu.OnInput += UpdateTaretRotation;
        
    }

    private void Update()
    {
        float curvePercentage = (transform.rotation.y - lastRotation) / (currentTargetRotation - lastRotation);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, currentTargetRotation, 0), Time.deltaTime * rotationSpeed);
    }

    private void UpdateTaretRotation()
    {
        lastRotation = transform.rotation.y;

        currentTargetRotation = 360f * mainMenu.CurrentButtonIndex / mainMenu.ButtonCount;
    }
}