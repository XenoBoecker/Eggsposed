using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BreedUI : MonoBehaviour
{
    BaseChickenController playerChickenController;


    [SerializeField] GameObject eggUI;

    [SerializeField] Image crackingEggImage, eggBaseImage, nestUeberEiImage;

    [SerializeField] Sprite[] crackingEggSprites;


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
            if(!isBreeding) StartCoroutine(FadeInEgg());
            UpdateEggSprite();
        }
        else
        {
            if(isBreeding) StartCoroutine(FadeOutEgg());
        }
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

            crackingEggImage.color = new Color(crackingEggImage.color.r, crackingEggImage.color.g, crackingEggImage.color.b, percentage);
            eggBaseImage.color = new Color(eggBaseImage.color.r, eggBaseImage.color.g, eggBaseImage.color.b, percentage);
            nestUeberEiImage.color = new Color(nestUeberEiImage.color.r, nestUeberEiImage.color.g, nestUeberEiImage.color.b, percentage);

            yield return null;
        }

        // Ensure the final state is fully opaque
        crackingEggImage.color = new Color(crackingEggImage.color.r, crackingEggImage.color.g, crackingEggImage.color.b, 1f);
        eggBaseImage.color = new Color(eggBaseImage.color.r, eggBaseImage.color.g, eggBaseImage.color.b, 1f);
        nestUeberEiImage.color = new Color(nestUeberEiImage.color.r, nestUeberEiImage.color.g, nestUeberEiImage.color.b, 1f);
    }

    IEnumerator FadeOutEgg()
    {
        isBreeding = false;

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            float percentage = i / fadeDuration;

            crackingEggImage.color = new Color(crackingEggImage.color.r, crackingEggImage.color.g, crackingEggImage.color.b, 1 - percentage);
            eggBaseImage.color = new Color(eggBaseImage.color.r, eggBaseImage.color.g, eggBaseImage.color.b, 1 - percentage);
            nestUeberEiImage.color = new Color(nestUeberEiImage.color.r, nestUeberEiImage.color.g, nestUeberEiImage.color.b, 1 - percentage);

            yield return null;
        }

        // Ensure the final state is fully transparent
        crackingEggImage.color = new Color(crackingEggImage.color.r, crackingEggImage.color.g, crackingEggImage.color.b, 0f);
        eggBaseImage.color = new Color(eggBaseImage.color.r, eggBaseImage.color.g, eggBaseImage.color.b, 0f);
        nestUeberEiImage.color = new Color(nestUeberEiImage.color.r, nestUeberEiImage.color.g, nestUeberEiImage.color.b, 0f);

        eggUI.SetActive(false);
    }
}
