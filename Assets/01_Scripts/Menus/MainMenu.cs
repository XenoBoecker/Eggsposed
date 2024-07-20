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
        controls = new PlayerControls();
        controls.Enable();

        OnInput += () => timeSinceLastInput = 0;

        timeSinceLastInput = -0.5f;
    }

    private void Start()
    {
        inputs = FindObjectOfType<KinectInputs>();

        if (inputs != null) inputs.OnSitDown += InputConfirm;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputs == null)
        {
            inputs = FindObjectOfType <KinectInputs>();
            inputs.OnSitDown += InputConfirm;
        }

        timeSinceLastInput += Time.deltaTime;

        if (timeSinceLastInput < timeBetweenInputs) return;

        if (inputs != null) 
        {
            if (inputs.MoveInput.x < 0) InputLeft();
            else if (inputs.MoveInput.x > 0) InputRight();
        }

        if (controls.Player.Move.ReadValue<Vector2>().x < 0) InputLeft();
        else if (controls.Player.Move.ReadValue<Vector2>().x > 0) InputRight();

        if (controls.Player.Breed.triggered) InputConfirm();
    }

    public void InputRight()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.changeButtonSound, SoundManager.Instance.uiSFX.source);

        currentButtonIndex++;

        if (currentButtonIndex > buttonCount-1 ) currentButtonIndex = 0;
        
        OnInput?.Invoke();
    }

    public void InputLeft()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.changeButtonSound, SoundManager.Instance.uiSFX.source);
        currentButtonIndex--;

        if (currentButtonIndex < 0) currentButtonIndex = buttonCount-1;
        
        OnInput?.Invoke();
    }

    public void InputConfirm()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.confirmSound, SoundManager.Instance.uiSFX.source);
        if (inputs != null) inputs.OnSitDown -= InputConfirm;
        
        if (currentButtonIndex == 0) SceneManager.LoadScene("Game");
        else if (currentButtonIndex == 1) Application.Quit();
        else if (currentButtonIndex == 2)
        {
            PlayerPrefs.SetInt("OnlyShowLeaderboard", 1);
            SceneManager.LoadScene("GameOverScreenScene");
        }
        else if (currentButtonIndex == 3) SceneManager.LoadScene("Calibration_X");
    }
}
