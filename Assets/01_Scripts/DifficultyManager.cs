using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    List<LevelDifficultyChanges> levelDifficultyChanges;
}

[System.Serializable]
public class LevelDifficultyChanges
{
    List<StatIncrease> statIncreases;
}

[System.Serializable]
public class StatIncrease
{
    public StatType statType;

    

}

public enum StatType
{
    Speed,
    Vision,
    DetectionRange,
    MaxViewAngle,
    ScanAngle,
    ScanTurnSpeed,
    BlockCooldownTime,
    MinimumUnblockedSpots,
    BlockRange,
    BlockingTime,
    CollectionRange
}