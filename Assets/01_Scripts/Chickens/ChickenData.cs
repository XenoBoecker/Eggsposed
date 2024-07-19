using System.Collections;
using UnityEngine;

[CreateAssetMenu()]
public class ChickenData : ScriptableObject
{
    public GameObject prefab;

    public GameObject eggVisual;
    public SkinnedMeshRenderer eyeL, eyeR, head, torso, wings, tail;

    public Sprite abilityIcon;
    public AudioClip callSound;

    public Sprite chickenHeadSprite, chickenBodySprite, chickenTailSprite;
    public string chickenSpawnScreenExplanation;
    public string combinedNameFirstHalf, combinedNameSecondHalf;
}
