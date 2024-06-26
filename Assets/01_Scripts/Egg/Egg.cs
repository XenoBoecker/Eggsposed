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
}
