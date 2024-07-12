using System;
using UnityEngine;
using UnityEngine.UI;

public class BreedUI : MonoBehaviour
{
    BaseChickenController playerChickenController;

    [SerializeField] Image fillImage;


    [SerializeField] GameObject eggUI;

    [SerializeField] Image crackingEggImage;

    [SerializeField] Sprite[] crackingEggSprites;

    private void Start()
    {
        playerChickenController = GameManager.Instance.Player.GetComponent<BaseChickenController>();
        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;
    }

    private void OnSpawnChicken()
    {
        playerChickenController = GameManager.Instance.Player.GetComponent<BaseChickenController>();
    }

    void Update()
    {
        fillImage.rectTransform.localScale = new Vector3(playerChickenController.breedPercentage, 1, 1);

        if (playerChickenController.breeding)
        {
            eggUI.SetActive(true);

            UpdateEggSprite();
        }
        else
        {
            eggUI.SetActive(false);
        }
    }

    private void UpdateEggSprite()
    {
        int index = Mathf.FloorToInt(playerChickenController.breedPercentage * (crackingEggSprites.Length - 1));
        crackingEggImage.sprite = crackingEggSprites[index];
    }
}
