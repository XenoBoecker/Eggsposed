using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    KinectInputs inputs;

    [SerializeField] float timeBetweenInputs = 0.5f;

    float timeSinceLastInput;

    private void Awake()
    {
        inputs = FindObjectOfType<KinectInputs>();

        inputs.OnSitDown += InputConfirm;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastInput += Time.deltaTime;

        if (timeSinceLastInput < timeBetweenInputs) return;

        if (inputs.MoveInput.x < 0) InputLeft();
        else if (inputs.MoveInput.x > 0) InputRight();
    }

    public void InputRight()
    {
        print("Right");
    }

    public void InputLeft()
    {
        print("Left");
    }

    public void InputConfirm()
    {
        print("Confirm");
    }
}
