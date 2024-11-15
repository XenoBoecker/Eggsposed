using ECM.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnGrassEffect : MonoBehaviour
{

    [SerializeField] GameObject grassEffectPrefab;

    List<Transform> onGrassWalkers = new List<Transform>();
    List<float> timers = new List<float>();
    List<GameObject> effects = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseCharacterController>())
        {
            ChickenStepSounds chickenStepSounds = other.GetComponent<ChickenStepSounds>();
            if (chickenStepSounds != null) chickenStepSounds.SetWalkingOnGrass(true);



            onGrassWalkers.Add(other.transform);
            timers.Add(0);
            effects.Add(Instantiate(grassEffectPrefab, other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onGrassWalkers.Contains(other.transform))
        {
            ChickenStepSounds chickenStepSounds = other.GetComponent<ChickenStepSounds>();
            if (chickenStepSounds != null) chickenStepSounds.SetWalkingOnGrass(false);
            
            int index = onGrassWalkers.IndexOf(other.transform);
            Destroy(effects[index]);
            effects.RemoveAt(index);
            timers.RemoveAt(index);
            onGrassWalkers.Remove(other.transform);
        }
    }
}
