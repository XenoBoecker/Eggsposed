using UnityEngine;

public class NumberManager : MonoBehaviour
{

    [SerializeField] Sprite[] numberSprites;

    public static NumberManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetNumberSprite(int number)
    {
        return numberSprites[number];
    }
}