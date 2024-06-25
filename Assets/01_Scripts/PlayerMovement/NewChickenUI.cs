using TMPro;
using UnityEngine;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText;

    GameManager gameManager;

    private void Start()
    {
        showChickenPanel.SetActive(false);

        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.OnSpawnChicken += OnSpawnChicken;
    }

    private void OnSpawnChicken(ChickenData chickenData)
    {
        gameManager.PauseGame();
        showChickenPanel.SetActive(true);

        newChickenNameText.text = chickenData.name;
    }

    public void Continue()
    {
        showChickenPanel.SetActive(false);
        gameManager.ResumeGame();
    }
}