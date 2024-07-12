using System.Collections;
using UnityEngine;

[CreateAssetMenu()]
public class ChickenData : ScriptableObject
{
    public GameObject prefab;

    public GameObject eggPrefab;
    public GameObject chickenVisualHead, chickenVisualBody, chickenVisualTail;

    public Sprite abilityIcon;
}
