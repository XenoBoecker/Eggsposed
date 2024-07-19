using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] bool isPlayersEgg;
    
    public void Pickup()
    {
        if (isPlayersEgg)
        {
            GameManager.Instance.GameOver();

            Debug.Log("Game Over");
        }
        else
        {
            Debug.Log("Farmer picked up egg");
        }

        Destroy(gameObject);
    }

    public void SetPlayersEgg(bool isPlayersEgg)
    {
        this.isPlayersEgg = isPlayersEgg;
    }

    public void SetEggVisual(ChickenData data)
    {
        if (data.eggVisual == null) return;

        transform.GetChild(0).gameObject.SetActive(false);

        Instantiate(data.eggVisual, transform);
    }
}
