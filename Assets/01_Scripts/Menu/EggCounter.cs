using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EggCounter : MonoBehaviour
{
    [SerializeField] TMP_Text eggCountText;

    List<Collider> alreadyCountedEggs = new List<Collider>();

    int eggsCollected = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyCountedEggs.Contains(other))
        {
            return;
        }

        alreadyCountedEggs.Add(other);

        eggsCollected++;
        eggCountText.text = eggsCollected.ToString();
    }
}