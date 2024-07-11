using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    KinectInputs inputs;

    [SerializeField] float timeBetweenInputs = 0.5f;

    float timeSinceLastInput;

    public event Action OnInputRight;
    public event Action OnInputLeft;
    public event Action OnInputConfirm;

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
        OnInputRight?.Invoke();
    }

    public void InputLeft()
    {
        OnInputLeft?.Invoke();
    }

    public void InputConfirm()
    {
        OnInputConfirm?.Invoke();
    }
}
