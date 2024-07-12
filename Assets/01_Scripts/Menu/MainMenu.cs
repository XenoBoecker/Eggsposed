using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    KinectInputs inputs;

    PlayerControls controls;


    [SerializeField] int buttonCount = 5;
    public int ButtonCount => buttonCount;

    [SerializeField] float timeBetweenInputs = 0.5f;

    float timeSinceLastInput;

    int currentButtonIndex;
    public int CurrentButtonIndex => currentButtonIndex;

    public event Action OnInput;

    private void Awake()
    {
        inputs = FindObjectOfType<KinectInputs>();

        controls = new PlayerControls();
        controls.Enable();

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
        currentButtonIndex++;

        if (currentButtonIndex > buttonCount-1 ) currentButtonIndex = 0;
        
        OnInput?.Invoke();
    }

    public void InputLeft()
    {
        currentButtonIndex--;

        if (currentButtonIndex < 0) currentButtonIndex = buttonCount-1;
        
        OnInput?.Invoke();
    }

    public void InputConfirm()
    {
        if (currentButtonIndex == 0) SceneManager.LoadScene("Game");
        else if (currentButtonIndex == 1) SceneManager.LoadScene("Leaderboard");
        else if (currentButtonIndex == 2) SceneManager.LoadScene("Options");
        else if (currentButtonIndex == 3) SceneManager.LoadScene("Calibration_X");
        else if (currentButtonIndex == 4) Application.Quit();
    }
}
