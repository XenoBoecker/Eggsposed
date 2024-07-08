using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenCloseSettings : MonoBehaviour
{
    protected PlayerControls controls;
    
    TimeManager timeManager;

    bool settingsOpen;
    int settingsStartIndex = 0;
    public int SettingsStartIndex => settingsStartIndex;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void Update()
    {
        if (controls.Player.ToggleSettings.triggered)
        {
            if (settingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    public void OpenSettings(int index = 0)
    {
        settingsOpen = true;
        settingsStartIndex = index;
            
        // Pause the game
        if (timeManager != null) timeManager.Pause();

        controls.Player.Disable();
        controls.UI.Enable();

        // Start a coroutine to wait for the scene to load before setting the selected button
        StartCoroutine(WaitForSceneLoadAndSetSelectedButton("SettingsEventSystem", index));
    }

    private IEnumerator WaitForSceneLoadAndSetSelectedButton(string eventSystemName, int index)
    {
        // Wait until the "Settings" scene is fully loaded
        yield return SceneManager.LoadSceneAsync("Settings", LoadSceneMode.Additive);

        // Find the EventSystem in the specified scene
        EventSystem eventSystem = GameObject.Find(eventSystemName).GetComponent<EventSystem>();

        // Get the Selectable component of the button at the specified index
        Selectable selectable = eventSystem.firstSelectedGameObject.GetComponent<Selectable>();

        // Set the selected button
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(selectable.gameObject);
    }

    public void CloseSettings()
    {
        settingsOpen = false;

        // Unpause the game
        if (timeManager != null) timeManager.Unpause();

        controls.UI.Disable();
        controls.Player.Enable();

        // Unload the "Settings" scene
        SceneManager.UnloadSceneAsync("Settings");

        SetSelectedButtonToFirstSelected("EventSystem");
    }
    private void SetSelectedButtonToFirstSelected(string eventSystemName)
    {
        // Find the EventSystem in the specified scene
        EventSystem eventSystem = GameObject.Find(eventSystemName).GetComponent<EventSystem>();

        if (eventSystem.firstSelectedGameObject == null) return;

        // Get the Selectable component of the button at the specified index
        Selectable selectable = eventSystem.firstSelectedGameObject.GetComponent<Selectable>();

        // Set the selected button
        eventSystem.SetSelectedGameObject(selectable.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
