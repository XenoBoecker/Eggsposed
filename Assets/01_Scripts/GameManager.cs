using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CameraController playerCam;

    [SerializeField] ChickenData _baseChickenData;
    [SerializeField] List<ChickenData> _allChicken;

    [SerializeField] GameObject eggPrefab;

    [SerializeField] int inheritanceCount = 2;

    List<ChickenData> previousChickenDatas = new List<ChickenData>();
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


        SpawnChicken(_baseChickenData, Vector3.zero);
    }

    private void SpawnChicken(ChickenData chickenData, Vector3 spawnPos)
    {
        previousChickenDatas.Add(chickenData);
        
        _player = Instantiate(chickenData.prefab, spawnPos, Quaternion.identity).GetComponent<Chicken>();
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
            previousChickenDatas[previousChickenDatas.Count - 1 - i].prefab.GetComponent<ChickenAbilitySetup>().Setup(_player);
        }
        if(previousChickenDatas.Count > 1) _player.SetChickenVisuals(chickenData, previousChickenDatas[previousChickenDatas.Count - 2], previousChickenDatas.Count);

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
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    internal void HackSpawnNextChicken()
    {
        SpawnNextChicken();
    }
}
