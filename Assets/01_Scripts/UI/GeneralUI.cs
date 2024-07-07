using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] Text eggCountText;

    void Start()
    {
        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;
    }

    private void OnSpawnChicken()
    {
        eggCountText.text = GameManager.Instance.BredEggCount.ToString();
    }
}