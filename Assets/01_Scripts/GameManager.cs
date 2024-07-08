using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CameraController playerCam;

    [SerializeField] Transform playerStartPosition;

    [SerializeField] Chicken chickenPrefab;

    [SerializeField] ChickenData _baseChickenData;
    [SerializeField] List<ChickenData> _allChicken;

    [SerializeField] GameObject eggPrefab;

    [SerializeField] int inheritanceCount = 2;

    List<ChickenData> previousChickenDatas = new List<ChickenData>();
    public int BredEggCount => previousChickenDatas.Count -1;
    ChickenData _nextChickenData;
    public ChickenData CurrentChickenData => previousChickenDatas[previousChickenDatas.Count - 1];

    Chicken _player;
    public Chicken Player => _player;

    public event Action OnSpawnChicken;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        SpawnChicken(_baseChickenData, playerStartPosition.position);
        playerStartPosition.gameObject.SetActive(false);
    }

    private void SpawnChicken(ChickenData chickenData, Vector3 spawnPos)
    {
        previousChickenDatas.Add(chickenData);
        
        _player = Instantiate(chickenPrefab, spawnPos, Quaternion.identity);
        _player.SetControlledByPlayer(true);

        _player.SetEgg(Instantiate(eggPrefab, spawnPos - Vector3.forward*2 + Vector3.up, Quaternion.identity).GetComponent<Egg>());

        playerCam.SetTarget(_player.transform);

        do _nextChickenData = _allChicken[UnityEngine.Random.Range(0, _allChicken.Count)];
        while (_nextChickenData == chickenData && _allChicken.Count != 1);

        _player.OnFinishBreeding += SpawnNextChicken;

        for (int i = 0; i < inheritanceCount; i++)
        {
            if (previousChickenDatas.Count <= i)
            {
                return;
            }
            CopyDerivedComponent(previousChickenDatas[previousChickenDatas.Count - 1-i].prefab, _player.gameObject);
        }
        if (previousChickenDatas.Count == 1) _player.SetChickenVisuals(chickenData, chickenData, previousChickenDatas.Count);
        else if (previousChickenDatas.Count > 1) _player.SetChickenVisuals(chickenData, previousChickenDatas[previousChickenDatas.Count - 2], previousChickenDatas.Count);

        OnSpawnChicken?.Invoke();
    }
    
    private void SpawnNextChicken()
    {
        print("spawn next");

        _player.OnFinishBreeding -= SpawnNextChicken;

        _player.SetControlledByPlayer(false);

        SpawnChicken(_nextChickenData, _player.transform.position);
    }

    public void PauseGame()
    {
        TimeManager.Instance.Pause();
    }

    public void ResumeGame()
    {
        TimeManager.Instance.Unpause();
    }

    private void CopyDerivedComponent(GameObject source, GameObject destination)
    {
        // Find any component derived from ChickenAbilitySetup
        ChickenAbilitySetup sourceComponent = source.GetComponent<ChickenAbilitySetup>();
        if (sourceComponent != null)
        {
            // Get the actual type of the component (e.g., LightningMcChickSetup, RotatorChickenSetup)
            Type sourceType = sourceComponent.GetType();
            Component destinationComponent = destination.AddComponent(sourceType);
            CopyComponentValues(sourceComponent, destinationComponent);
        }
    }

    // Method to copy values from one component to another
    private void CopyComponentValues(Component source, Component destination)
    {
        // Get all fields from the source component
        FieldInfo[] fields = source.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            field.SetValue(destination, field.GetValue(source));
        }

        // Get all properties from the source component
        PropertyInfo[] properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite)
            {
                property.SetValue(destination, property.GetValue(source));
            }
        }
    }

    internal void HackSpawnNextChicken()
    {
        SpawnNextChicken();
    }
}