using TMPro;
using UnityEngine;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText;

    KinectInputs inputs;

    private void Start()
    {
        showChickenPanel.SetActive(false);

        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;

        if (GameManager.Instance.KinectInputs)
        {
            inputs = FindObjectOfType<KinectInputs>();
            inputs.OnSitDown += Continue;
        }
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