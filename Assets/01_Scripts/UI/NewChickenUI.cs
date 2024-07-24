using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText, oldChickenNameText, combinedChickenNameText, oldChickenExplanationText, newChickenExplanationText;

    [SerializeField] Image[] oldChickenImages, newChickenImages, combinedChickenImages;

    [SerializeField] CanvasGroup fadeCanvasGroup;

    [SerializeField] VisualEffect poofEffect;

    [SerializeField] float poofEffectDuration = 1f;

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
        // StartCoroutine(PoofEffectRoutine());
        GameManager.Instance.PauseGame();
        StartCoroutine(FadeIn(fadeCanvasGroup));
        chickenPanelPause = true;

        canContinue = false;
        StartCoroutine(WaitTimer());

        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.hatchNewChickenRiff);

        UpdateNewChickenUI();
    }

    private IEnumerator PoofEffectRoutine()
    {
        // Ensure the Visual Effect is enabled and played
        poofEffect.enabled = true;
        poofEffect.Play();

        float elapsed = 0f;

        // Continue updating the effect until the duration is reached
        while (elapsed < poofEffectDuration)
        {
            // Increase the elapsed time by the unscaled delta time
            elapsed += Time.unscaledDeltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Optionally stop or disable the Visual Effect after the duration
        poofEffect.Stop();
        poofEffect.enabled = false;


    }

    private void UpdateNewChickenUI()
    {
        ChickenData data = GameManager.Instance.CurrentChickenData;
        ChickenData previousData = GameManager.Instance.PreviousChickenData(1);


        // newChickenNameText.text = data.name;
        // oldChickenNameText.text = previousData.name;
        oldChickenExplanationText.text = data.chickenSpawnScreenExplanation;
        newChickenExplanationText.text = previousData.chickenSpawnScreenExplanation;

        combinedChickenNameText.text = GetCombinedChickenName(data, previousData);

        SetChickenImages(oldChickenImages, previousData);

        SetChickenImages(newChickenImages, data);

        if(GameManager.Instance.BredEggCount % 2 == 1) SetChickenImages(combinedChickenImages, data, previousData);
        else SetChickenImages(combinedChickenImages, previousData, data);
    }

    private string GetCombinedChickenName(ChickenData data, ChickenData previousData)
    {
        string firstHalf = data.combinedNameFirstHalf;
        if (firstHalf == "") firstHalf = data.name;
        string secondHalf = previousData.combinedNameSecondHalf;
        if (secondHalf == "") secondHalf = previousData.name;

        return firstHalf + " - " + secondHalf + " Chicken";
    }

    private void SetChickenImages(Image[] chickenImages, ChickenData data, ChickenData chickenData = null)
    {
        if (data.chickenHeadSprite == null) return; // TODO: remove this line when all chickens have sprites

        chickenImages[2].sprite = data.chickenHeadSprite;
        chickenImages[1].sprite = data.chickenBodySprite;
        chickenImages[0].sprite = data.chickenTailSprite;

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