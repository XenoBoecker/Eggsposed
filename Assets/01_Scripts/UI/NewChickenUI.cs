using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText, explanationText;

    [SerializeField] Image newChickenImage;

    PlayerControls controls;

    KinectInputs inputs;

    bool chickenPanelPause;

    [SerializeField] float waitTimeBeforeContinueIsAllowed = 1f;

    bool canContinue;

    private void Start()
    {
        showChickenPanel.SetActive(false);

        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;

        if (GameManager.Instance.KinectInputs)
        {
            inputs = FindObjectOfType<KinectInputs>();
            inputs.OnSitDown += Continue;
        }

        controls = new PlayerControls();
        controls.Enable();
    }

    private void Update()
    {
        if (GameManager.Instance.KinectInputs && inputs == null)
        {
            inputs = FindObjectOfType<KinectInputs>();
            inputs.OnSitDown += Continue;
        }

        if (!chickenPanelPause) return;

        if (controls.Player.Breed.triggered) Continue();
    }

    private void OnSpawnChicken()
    {
        GameManager.Instance.PauseGame();
        showChickenPanel.SetActive(true);
        chickenPanelPause = true;

        canContinue = false;
        StartCoroutine(WaitTimer());

        ChickenData data = GameManager.Instance.CurrentChickenData;

        newChickenNameText.text = data.name;
        explanationText.text = data.chickenSpawnScreenExplanation;
        if(data.chickenSpawnScreenImage != null) newChickenImage.sprite = data.chickenSpawnScreenImage;
    }

    public void Continue()
    {
        if (!chickenPanelPause) return;

        if (!canContinue) return;

        showChickenPanel.SetActive(false);
        chickenPanelPause = false;
        GameManager.Instance.ResumeGame();
    }

    IEnumerator WaitTimer()
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + waitTimeBeforeContinueIsAllowed)
        {
            yield return null;
        }
        canContinue = true;
    }
}