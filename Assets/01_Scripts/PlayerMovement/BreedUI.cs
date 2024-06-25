using System;
using UnityEngine;
using UnityEngine.UI;

public class BreedUI : MonoBehaviour
{
    BaseChickenController playerChickenController;

    [SerializeField] Image fillImage;

    private void Start()
    {
        playerChickenController = GameManager.Instance.Player.GetComponent<BaseChickenController>();
        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;
    }

    private void OnSpawnChicken(ChickenData data)
    {
        playerChickenController = GameManager.Instance.Player.GetComponent<BaseChickenController>();
    }

    void Update()
    {
        fillImage.rectTransform.localScale = new Vector3(playerChickenController.breedPercentage, 1, 1);
    }
}