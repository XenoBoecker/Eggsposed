using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool kinectInputs;
    public bool KinectInputs => kinectInputs;


    [SerializeField] string gameOverSceneName = "GameOverScreenScene";

    [SerializeField] CameraController playerCam;

    [SerializeField] Transform playerStartPosition;

    [SerializeField] Chicken chickenPrefab;

    [SerializeField] Egg eggPrefab;

    [SerializeField] ChickenData _baseChickenData;
    [SerializeField] List<ChickenData> _allChicken;

    [SerializeField] int inheritanceCount = 2;

    List<ChickenData> previousChickenDatas = new List<ChickenData>();
    public int BredEggCount => previousChickenDatas.Count - 1;
    ChickenData _nextChickenData;
    public ChickenData CurrentChickenData => PreviousChickenData(0);

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


    public ChickenData PreviousChickenData(int reverseIndex) {
        int index = previousChickenDatas.Count - 1 - reverseIndex;

        if (index < 0) return null;

        else return previousChickenDatas[index];
    }

    private void SpawnChicken(ChickenData chickenData, Vector3 spawnPos)
    {
        previousChickenDatas.Add(chickenData);
        
        if(_player == null) _player = Instantiate(chickenPrefab, spawnPos, Quaternion.identity);
        else _player = Instantiate(chickenPrefab, spawnPos, _player.transform.rotation);
        _player.SetControlledByPlayer(true);

        Egg newEgg = Instantiate(eggPrefab, spawnPos - Vector3.forward * 2 + Vector3.up, Quaternion.identity);
        newEgg.SetPlayersEgg(true);

        int iterationCount = 0;

        do
        {
            _nextChickenData = _allChicken[UnityEngine.Random.Range(0, _allChicken.Count)];
            iterationCount++;
        }
        while ((_nextChickenData == chickenData || _nextChickenData == PreviousChickenData(1)) && _allChicken.Count != 1 && iterationCount < 100);

        newEgg.SetEggVisual(_nextChickenData);

        _player.SetEgg(newEgg);

        playerCam.SetTarget(_player.transform);


        _player.OnFinishBreeding += SpawnNextChicken;

        for (int i = 0; i < inheritanceCount; i++)
        {
            if (previousChickenDatas.Count <= i)
            {
                continue;
            }
            CopyDerivedComponent(previousChickenDatas[previousChickenDatas.Count - 1-i].prefab, _player.gameObject);
        }

        if (previousChickenDatas.Count == 1) _player.SetChickenVisuals(chickenData, chickenData);
        else if (previousChickenDatas.Count > 1)
        {
            if(previousChickenDatas.Count % 2 == 0) _player.SetChickenVisuals(chickenData, previousChickenDatas[previousChickenDatas.Count - 2]);
            else _player.SetChickenVisuals(previousChickenDatas[previousChickenDatas.Count - 2], chickenData);
        }

        print("OnSpawnChicken");
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
        // Get the type of the source component
        Type type = source.GetType();
        // Get the immediate base type of the source component
        Type baseType = type.BaseType;

        // Copy all fields from the source type and its immediate base type
        CopyFieldsAndProperties(source, destination, type);
        if (baseType != null && baseType != typeof(MonoBehaviour))
        {
            CopyFieldsAndProperties(source, destination, baseType);
        }
    }

    private void CopyFieldsAndProperties(Component source, Component destination, Type type)
    {
        // Copy fields
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(source);
            if (value != null)
            {
                field.SetValue(destination, value);
            }
        }

        // Copy properties
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite && property.GetIndexParameters().Length == 0)
            {
                object value = property.GetValue(source);
                if (value != null)
                {
                    property.SetValue(destination, value);
                }
            }
        }
    }

    internal void HackSpawnNextChicken()
    {
        SpawnNextChicken();
    }

    internal void GameOver()
    {
        print("GAME OVER!");

        PlayerPrefs.SetInt("OnlyShowLeaderboard", 0);
        previousChickenDatas.RemoveAt(0);
        GameOverInfo.SetBredChickens(previousChickenDatas);

        SceneManager.LoadScene(gameOverSceneName);
    }

    internal void SpawnEgg(Vector3 position)
    {
        int rand = UnityEngine.Random.Range(0, _allChicken.Count);

        ChickenData chickenData = _allChicken[rand];

        Instantiate(chickenData.eggVisual, position, Quaternion.identity);
    }
}

public static class GameOverInfo
{
    static List<ChickenData> bredChickens = new List<ChickenData>();

    public static void SetBredChickens(List<ChickenData> chickens)
    {
        bredChickens.Clear();

        foreach (ChickenData chicken in chickens)
        {
            bredChickens.Add(chicken);
        }
    }

    public static List<ChickenData> GetBredChickens()
    {
        return bredChickens;
    }
}
