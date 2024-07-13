using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewChickenUI : MonoBehaviour
{
    [SerializeField] GameObject showChickenPanel;

    [SerializeField] TMP_Text newChickenNameText, explanationText;

    [SerializeField] Image newChickenImage;

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

        ChickenData data = GameManager.Instance.CurrentChickenData;

        newChickenNameText.text = data.name;
        explanationText.text = data.chickenSpawnScreenExplanation;
        if(data.chickenSpawnScreenImage != null) newChickenImage.sprite = data.chickenSpawnScreenImage;
    }

    public void Continue()
    {
        showChickenPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}