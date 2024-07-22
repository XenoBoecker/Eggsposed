using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] bool isPlayersEgg;

    [SerializeField] Transform visualParent;

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
        print("Egg add collider");
        if (data.eggVisual == null) return;

        Instantiate(data.eggVisual, visualParent);

        foreach (MeshFilter child in GetComponentsInChildren<MeshFilter>())
        {
            MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();
            collider.convex = true;
        }
    }
}
