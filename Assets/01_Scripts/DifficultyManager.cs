using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    FarmerStats farmerBaseStats;
    FarmerStateMachine farmer;

    // public List<LevelDifficultyChanges> levelDifficultyChanges;

    public List<FarmerStats> levelChangeStats;

    int currentLevel = 0;

    private void Start()
    {
        GameManager.Instance.OnSpawnChicken += SetStatsToNextLevel;
        farmer = FindObjectOfType<FarmerStateMachine>();

        farmerBaseStats = farmer.FarmerStats;
    }

    private void SetStatsToNextLevel()
    {
        currentLevel++;
        
        print("NewStats");
        farmer.SetFarmerStats(CalculateNewStats());
        farmer.ResetToStartPositionAndState();
    }

    FarmerStats CalculateNewStats()
    {
        FarmerStats newStats = new FarmerStats();
        newStats.baseMovementSpeed = farmerBaseStats.baseMovementSpeed;
        float baseMovementSpeedMultiplier = 1;
        newStats.maxViewAngle = farmerBaseStats.maxViewAngle;
        newStats.hearingDistance = farmerBaseStats.hearingDistance;
        newStats.detectionRange = farmerBaseStats.detectionRange;
        newStats.xRayTrackingTime = farmerBaseStats.xRayTrackingTime;
        newStats.biasDistance = farmerBaseStats.biasDistance;
        newStats.scanTurnSpeed = farmerBaseStats.scanTurnSpeed;
        float scanTurningSpeedMultiplier = 1;
        newStats.scanAngle = farmerBaseStats.scanAngle;
        newStats.breedingSpotBlockDuration = farmerBaseStats.breedingSpotBlockDuration;
        newStats.blockCooldownTime = farmerBaseStats.blockCooldownTime;
        newStats.minimumUnblockedSpots = farmerBaseStats.minimumUnblockedSpots;
        newStats.collectionProgressTime = farmerBaseStats.collectionProgressTime;
        newStats.collectionRange = farmerBaseStats.collectionRange;
        newStats.timeoutRange = farmerBaseStats.timeoutRange;
        newStats.catchupMinimumDistance = farmerBaseStats.catchupMinimumDistance;
        newStats.catchupMinimumDistance = farmerBaseStats.catchupMinimumDistance;
        newStats.catchupSpeedMultiplier = farmerBaseStats.catchupSpeedMultiplier;
        float catchupSpeedMultiplier = 1;
        newStats.catchupCooldown = farmerBaseStats.catchupCooldown;
        newStats.catchupMaxDuration = farmerBaseStats.catchupMaxDuration;

        int addIndex;
        
        for (int i = 0; i < currentLevel; i++)
        {
            if(i < levelChangeStats.Count) addIndex = i;
            else addIndex = levelChangeStats.Count - 1;

            baseMovementSpeedMultiplier += levelChangeStats[addIndex].baseMovementSpeed;

            newStats.maxViewAngle += levelChangeStats[addIndex].maxViewAngle;

            newStats.hearingDistance += levelChangeStats[addIndex].hearingDistance;

            newStats.detectionRange += levelChangeStats[addIndex].detectionRange;

            newStats.xRayTrackingTime += levelChangeStats[addIndex].xRayTrackingTime;

            newStats.biasDistance += levelChangeStats[addIndex].biasDistance;

            scanTurningSpeedMultiplier += levelChangeStats[addIndex].scanTurnSpeed;

            newStats.scanAngle += levelChangeStats[addIndex].scanAngle;

            newStats.breedingSpotBlockDuration += levelChangeStats[addIndex].breedingSpotBlockDuration;

            newStats.blockCooldownTime += levelChangeStats[addIndex].blockCooldownTime;

            newStats.minimumUnblockedSpots += levelChangeStats[addIndex].minimumUnblockedSpots;

            newStats.collectionProgressTime += levelChangeStats[addIndex].collectionProgressTime;

            newStats.collectionRange += levelChangeStats[addIndex].collectionRange;

            newStats.timeoutRange += levelChangeStats[addIndex].timeoutRange;

            newStats.catchupMinimumDistance += levelChangeStats[addIndex].catchupMinimumDistance;

            newStats.catchupMinimumDistance += levelChangeStats[addIndex].catchupMinimumDistance;

            catchupSpeedMultiplier += levelChangeStats[addIndex].catchupSpeedMultiplier;

            newStats.catchupCooldown += levelChangeStats[addIndex].catchupCooldown;

            newStats.catchupMaxDuration += levelChangeStats[addIndex].catchupMaxDuration;
        }

        newStats.baseMovementSpeed *= baseMovementSpeedMultiplier;

        newStats.scanTurnSpeed *= scanTurningSpeedMultiplier;

        newStats.catchupSpeedMultiplier *= catchupSpeedMultiplier;

        return newStats;
    }
    
    /*
    private FarmerStats CalculateNewStats()
    {
        FarmerStats newStats = new FarmerStats();
        newStats.baseMovementSpeed = farmerBaseStats.baseMovementSpeed;
        float baseMovementSpeedMultiplier = 1;
        newStats.maxViewAngle = farmerBaseStats.maxViewAngle;
        newStats.hearingDistance = farmerBaseStats.hearingDistance;
        newStats.detectionRange = farmerBaseStats.detectionRange;
        newStats.xRayTrackingTime = farmerBaseStats.xRayTrackingTime;
        newStats.biasDistance = farmerBaseStats.biasDistance;
        newStats.scanTurnSpeed = farmerBaseStats.scanTurnSpeed;
        float scanTurningSpeedMultiplier = 1;
        newStats.scanAngle = farmerBaseStats.scanAngle;
        newStats.breedingSpotBlockDuration = farmerBaseStats.breedingSpotBlockDuration;
        newStats.blockCooldownTime = farmerBaseStats.blockCooldownTime;
        newStats.minimumUnblockedSpots = farmerBaseStats.minimumUnblockedSpots;
        newStats.collectionProgressTime = farmerBaseStats.collectionProgressTime;
        newStats.collectionRange = farmerBaseStats.collectionRange;
        newStats.timeoutRange = farmerBaseStats.timeoutRange;
        newStats.catchupDistance = farmerBaseStats.catchupDistance;
        newStats.catchupMinimumDistance = farmerBaseStats.catchupMinimumDistance;
        newStats.catchupSpeedMultiplier = farmerBaseStats.catchupSpeedMultiplier;
        float catchupSpeedMultiplier = 1;
        newStats.catchupCooldown = farmerBaseStats.catchupCooldown;
        newStats.catchupMaxDuration = farmerBaseStats.catchupMaxDuration;

        for (int i = 0; i < currentLevel; i++)
        {
            for (int j = 0; j < levelDifficultyChanges[i].statIncreases.Count; j++)
            {
                baseMovementSpeedMultiplier += levelDifficultyChanges[i].statIncreases[j].baseMovementSpeedIncrease;
                newStats.maxViewAngle += levelDifficultyChanges[i].statIncreases[j].maxViewAngleIncrease;
                newStats.hearingDistance += levelDifficultyChanges[i].statIncreases[j].hearingDistanceIncrease;
                newStats.detectionRange += levelDifficultyChanges[i].statIncreases[j].detectionRangeIncrease;
                newStats.xRayTrackingTime += levelDifficultyChanges[i].statIncreases[j].xRayTrackingimeIncrease;
                newStats.biasDistance += levelDifficultyChanges[i].statIncreases[j].biasDistanceIncrease;
                scanTurningSpeedMultiplier += levelDifficultyChanges[i].statIncreases[j].scanTurnSpeedIncrease;
                newStats.scanAngle += levelDifficultyChanges[i].statIncreases[j].scanAngleIncrease;
                newStats.breedingSpotBlockDuration += levelDifficultyChanges[i].statIncreases[j].breedingSpotBlockDurationIncrease;
                newStats.blockCooldownTime += levelDifficultyChanges[i].statIncreases[j].blockCooldownTimeIncrease;
                newStats.minimumUnblockedSpots += levelDifficultyChanges[i].statIncreases[j].minimumUnblockedSpotsIncrease;
                newStats.collectionProgressTime += levelDifficultyChanges[i].statIncreases[j].collectionProgressTimeIncrease;
                newStats.collectionRange += levelDifficultyChanges[i].statIncreases[j].collectionRangeIncrease;
                newStats.timeoutRange += levelDifficultyChanges[i].statIncreases[j].timeoutRangeIncrease;
                newStats.catchupDistance += levelDifficultyChanges[i].statIncreases[j].catchupDistanceIncrease;
                newStats.catchupMinimumDistance += levelDifficultyChanges[i].statIncreases[j].catchupMinimumDistanceIncrease;
                catchupSpeedMultiplier += levelDifficultyChanges[i].statIncreases[j].catchupSpeedMultiplierIncrease;
                newStats.catchupCooldown += levelDifficultyChanges[i].statIncreases[j].catchupCooldownIncrease;
                newStats.catchupMaxDuration += levelDifficultyChanges[i].statIncreases[j].catchupMaxDurationIncrease;
                
            }
        }

        newStats.scanTurnSpeed *= scanTurningSpeedMultiplier;
        newStats.baseMovementSpeed *= baseMovementSpeedMultiplier;
        newStats.catchupSpeedMultiplier *= catchupSpeedMultiplier;

        print("Speed:" + newStats.baseMovementSpeed);

        return newStats;
    }
    */
}

[System.Serializable]
public class LevelDifficultyChanges
{
    public List<StatIncrease> statIncreases;
}

[System.Serializable]
public class StatIncrease
{
    public StatType statType;

    [ConditionalHide("statType", (int)StatType.MaxViewAngle)]
    public float maxViewAngleIncrease;

    [ConditionalHide("statType", (int)StatType.HearingDistance)]
    public float hearingDistanceIncrease;

    [ConditionalHide("statType", (int)StatType.DetectionRange)]
    public float detectionRangeIncrease;

    [ConditionalHide("statType", (int)StatType.XRayTrackingTime)]
    public float xRayTrackingimeIncrease;

    [ConditionalHide("statType", (int)StatType.ScanTurnSpeed)]
    public float scanTurnSpeedIncrease;

    [ConditionalHide("statType", (int)StatType.ScanAngle)]
    public float scanAngleIncrease;

    [ConditionalHide("statType", (int)StatType.BreedingSpotBlockDuration)]
    public float breedingSpotBlockDurationIncrease;

    [ConditionalHide("statType", (int)StatType.BlockCooldownTime)]
    public float blockCooldownTimeIncrease;

    [ConditionalHide("statType", (int)StatType.MinimumUnblockedSpots)]
    public float minimumUnblockedSpotsIncrease;

    [ConditionalHide("statType", (int)StatType.CollectionProgressTime)]
    public float collectionProgressTimeIncrease;

    [ConditionalHide("statType", (int)StatType.CollectionRange)]
    public float collectionRangeIncrease;

    [ConditionalHide("statType", (int)StatType.TimeoutRange)]
    public float timeoutRangeIncrease;

    [ConditionalHide("statType", (int)StatType.BiasDistance)]
    public float biasDistanceIncrease;

    [ConditionalHide("statType", (int)StatType.BaseMovementSpeed)]
    public float baseMovementSpeedIncrease;

    [ConditionalHide("statType", (int)StatType.CatchupDistance)]
    public float catchupDistanceIncrease;

    [ConditionalHide("statType", (int)StatType.CatchupMinimumDistance)]

    public float catchupMinimumDistanceIncrease;

    [ConditionalHide("statType", (int)StatType.CatchupSpeedMultiplier)]
    public float catchupSpeedMultiplierIncrease;

    [ConditionalHide("statType", (int)StatType.CatchupCooldown)]
    public float catchupCooldownIncrease;

    [ConditionalHide("statType", (int)StatType.CatchupMaxDuration)]
    public float catchupMaxDurationIncrease;
}

public enum StatType
{
    MaxViewAngle,
    HearingDistance,
    DetectionRange,
    XRayTrackingTime,
    ScanTurnSpeed,
    ScanAngle,
    BreedingSpotBlockDuration,
    BlockCooldownTime,
    MinimumUnblockedSpots,
    CollectionProgressTime,
    CollectionRange,
    TimeoutRange,
    BiasDistance,
    BaseMovementSpeed,
    CatchupDistance,
    CatchupMinimumDistance,
    CatchupSpeedMultiplier,
    CatchupCooldown,
    CatchupMaxDuration
}

[System.Serializable]
public class FarmerStats
{

    public float baseMovementSpeed;
    
    [Header("Vision")]
    public float maxViewAngle;

    public float hearingDistance;

    public float detectionRange;

    public float xRayTrackingTime;

    public float biasDistance;

    [Header("Scan State")]
    public float scanTurnSpeed;

    public float scanAngle;

    [Header("Block Breeding Spots State")]
    public float breedingSpotBlockDuration;

    public float blockCooldownTime;

    public float minimumUnblockedSpots;

    [Header("Collect Egg State")]
    public float collectionProgressTime;

    public float collectionRange;

    public float timeoutRange;

    [Header("Chase State")]
    public float catchupMinimumDistance;

    public float catchupSpeedMultiplier;

    public float catchupCooldown;

    public float catchupMaxDuration;
}