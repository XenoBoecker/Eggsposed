using TMPro;
using UnityEngine;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText;

    PlayerControls controls;

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

        controls = new PlayerControls();
        controls.Enable();
    }

    private void Update()
    {
        if (controls.Player.Breed.triggered) Continue();
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