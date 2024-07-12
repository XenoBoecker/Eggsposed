using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] NumberDisplay eggCounter;

    void Start()
    {
        GameManager.Instance.OnSpawnChicken += OnSpawnChicken;

        eggCounter.SetNumber(0);
    }

    private void OnSpawnChicken()
    {
        eggCounter.SetNumber(GameManager.Instance.BredEggCount);
    }
}