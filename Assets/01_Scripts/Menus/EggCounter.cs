using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EggCounter : MonoBehaviour
{
    [SerializeField] NumberDisplay eggCounter;

    List<Egg> alreadyCountedEggs = new List<Egg>();

    int eggsCollected = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyCountedEggs.Contains(other.transform.parent.GetComponentInParent<Egg>()))
        {
            return;
        }

        alreadyCountedEggs.Add(other.GetComponentInParent<Egg>());

        
            

        eggsCollected++;
        eggCounter.SetNumber(eggsCollected);
    }
}