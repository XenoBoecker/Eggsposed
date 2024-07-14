using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EggCounter : MonoBehaviour
{
    [SerializeField] NumberDisplay eggCounter;

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
        eggCounter.SetNumber(eggsCollected);
    }
}