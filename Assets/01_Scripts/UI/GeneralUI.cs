using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] NumberDisplay eggCounter;


    [SerializeField] Image canCallImage;

    [SerializeField] Image fillImage;

    [SerializeField] Sprite canCallSprite, cannotCallSprite;

    [SerializeField] Image[] abilityIcons;

    void Awake()
    {
        GameManager.Instance.OnSpawnChicken += UpdateUI;

        eggCounter.SetNumber(0);
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (GameManager.Instance.Player.CheckCanCall())
        {
            canCallImage.sprite = canCallSprite;
        }
        else
        {
            canCallImage.sprite = cannotCallSprite;

            fillImage.fillAmount = GameManager.Instance.Player.GetCallCDPercentage();

        }
    }

    private void UpdateUI()
    {
        eggCounter.SetNumber(GameManager.Instance.BredEggCount);

        if (GameManager.Instance.BredEggCount == 0)
        {
            abilityIcons[0].sprite = GameManager.Instance.PreviousChickenData(0).abilityIcon;
            abilityIcons[1].sprite = GameManager.Instance.PreviousChickenData(0).abilityIcon;
            return;
        }

        for (int i = 0; i < abilityIcons.Length; i++)
        {
            abilityIcons[i].sprite = GameManager.Instance.PreviousChickenData(i).abilityIcon;
        }
    }
}