using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText, oldChickenExplanationText, newChickenExplanationText;

    [SerializeField] Image[] oldChickenImages, newChickenImages, combinedChickenImages;

    [SerializeField] CanvasGroup fadeCanvasGroup;

    PlayerControls controls;

    KinectInputs inputs;

    bool chickenPanelPause;

    [SerializeField] float waitTimeBeforeContinueIsAllowed = 1f;


    [SerializeField] float fadeDuration;

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
        StartCoroutine(FadeIn(fadeCanvasGroup));
        chickenPanelPause = true;

        canContinue = false;
        StartCoroutine(WaitTimer());


        UpdateNewChickenUI();
    }

    private void UpdateNewChickenUI()
    {
        ChickenData data = GameManager.Instance.CurrentChickenData;
        
        newChickenNameText.text = data.name;
        oldChickenExplanationText.text = data.chickenSpawnScreenExplanation;
        newChickenExplanationText.text = GameManager.Instance.PreviousChickenData(1).chickenSpawnScreenExplanation;

        SetChickenImages(oldChickenImages, GameManager.Instance.PreviousChickenData(1));

        SetChickenImages(newChickenImages, data);

        if(GameManager.Instance.BredEggCount % 2 == 0) SetChickenImages(combinedChickenImages, data, GameManager.Instance.PreviousChickenData(1));
        else SetChickenImages(combinedChickenImages, GameManager.Instance.PreviousChickenData(1), data);
    }

    private void SetChickenImages(Image[] chickenImages, ChickenData data, ChickenData chickenData = null)
    {
        if (data.chickenHeadSprite == null) return; // TODO: remove this line when all chickens have sprites

        chickenImages[0].sprite = data.chickenHeadSprite;
        chickenImages[1].sprite = data.chickenBodySprite;
        chickenImages[2].sprite = data.chickenTailSprite;

        if (chickenData != null) chickenImages[1].sprite = chickenData.chickenBodySprite;
    }

    public void Continue()
    {
        if (!chickenPanelPause) return;

        if (!canContinue) return;

        StartCoroutine(FadeOut(fadeCanvasGroup));
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

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        showChickenPanel.SetActive(true);
        for (float i = 0; i < fadeDuration; i += Time.unscaledDeltaTime)
        {
            float percentage = i / fadeDuration;

            canvasGroup.alpha = percentage;

            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        for (float i = 0; i < fadeDuration; i += Time.unscaledDeltaTime)
        {
            float percentage = i / fadeDuration;

            canvasGroup.alpha = 1 - percentage;

            yield return null;
        }

        canvasGroup.alpha = 0;
        showChickenPanel.SetActive(false);
    }

}