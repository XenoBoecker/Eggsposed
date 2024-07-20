using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BreedUI : MonoBehaviour
{
    BaseChickenController playerChickenController;


    [SerializeField] GameObject eggUI;

    [SerializeField] Image crackingEggImage;

    [SerializeField] CanvasGroup fadeCanvasGroup;

    [SerializeField] Sprite[] crackingEggSprites;

    [SerializeField] private float wiggleSpeed = 1f; // Speed of the wiggle
    [SerializeField] private float wiggleAngle = 15f; // Maximum angle of the wiggle

    [SerializeField] float fadeDuration = 0.5f;

    bool isBreeding = false;

    private void Start()
    {
        playerChickenController = GameManager.Instance.Player.GetComponent<BaseChickenController>();
        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;

        eggUI.SetActive(false);
    }

    private void OnSpawnChicken()
    {
        playerChickenController = GameManager.Instance.Player.GetComponent<BaseChickenController>();
    }

    void Update()
    {
        if (playerChickenController.breeding)
        {
            if (!isBreeding)
            {
                StopAllCoroutines();
                StartCoroutine(FadeInEgg());
            }
            UpdateEggSprite();
            WiggleEgg();
        }
        else
        {
            if (isBreeding)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOutEgg());
            }
        }
    }

    private void WiggleEgg()
    {
        float angle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAngle;
        crackingEggImage.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateEggSprite()
    {
        int index = Mathf.FloorToInt(playerChickenController.breedPercentage * (crackingEggSprites.Length - 1));
        crackingEggImage.sprite = crackingEggSprites[index];
    }

    IEnumerator FadeInEgg()
    {
        isBreeding = true;
        eggUI.SetActive(true);
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            float percentage = i / fadeDuration;

            fadeCanvasGroup.alpha = percentage;

            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
    }

    IEnumerator FadeOutEgg()
    {
        isBreeding = false;

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            float percentage = i / fadeDuration;

            fadeCanvasGroup.alpha = 1-percentage;
            yield return null;
        }
        
        fadeCanvasGroup.alpha = 0;


        eggUI.SetActive(false);
    }
}
