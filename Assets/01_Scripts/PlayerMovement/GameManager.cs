using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CameraController playerCam;

    [SerializeField] ChickenData _baseChickenData;
    [SerializeField] List<ChickenData> _allChicken;

    [SerializeField] GameObject eggPrefab;

    ChickenData _nextChickenData;

    Chicken _player;
    public Chicken Player => _player;

    public event Action<ChickenData> OnSpawnChicken;

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
        _player = Instantiate(chickenData.prefab, spawnPos, Quaternion.identity).GetComponent<Chicken>();
        _player.SetControlledByPlayer(true);

        _player.SetEgg(Instantiate(eggPrefab, spawnPos - Vector3.forward*2 + Vector3.up, Quaternion.identity).GetComponent<Egg>());

        playerCam.SetTarget(_player.transform);

        _nextChickenData = _allChicken[UnityEngine.Random.Range(0, _allChicken.Count)];

        _player.OnFinishBreeding += SpawnNextChicken;

        OnSpawnChicken?.Invoke(chickenData);
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
}
