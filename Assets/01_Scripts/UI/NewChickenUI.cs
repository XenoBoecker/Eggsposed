using TMPro;
using UnityEngine;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText;

    private void Start()
    {
        showChickenPanel.SetActive(false);

        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;
    }

    private void OnSpawnChicken()
    {
        GameManager.Instance.PauseGame();
        showChickenPanel.SetActive(true);

        newChickenNameText.text = GameManager.Instance.CurrentChickenData.name;
    }

    public void Continue()
    {
        showChickenPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}