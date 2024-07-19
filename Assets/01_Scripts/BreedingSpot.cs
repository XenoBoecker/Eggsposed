using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingSpot : MonoBehaviour
{
    [SerializeField] float radius = 3f;


    [SerializeField] GameObject blockSpotVisuals;


    [SerializeField] GameObject tellPlayerToSquat;

    Chicken player;

    bool _isBlocked;

    float _timeBred;

    bool _breeding;

    float blockedTime;

    bool playerHasBredOutAnEgg;

    private void Start()
    {
        blockSpotVisuals.SetActive(false);

        tellPlayerToSquat.SetActive(false);

        GameManager.Instance.OnSpawnChicken += () => playerHasBredOutAnEgg = true;

        player = GameManager.Instance.Player;
    }

    void Update()
    {
        if (_breeding)
        {
            _timeBred += Time.deltaTime;
        }

        if (_isBlocked)
        {
            blockedTime -= Time.deltaTime;
            if (blockedTime <= 0) UnblockSpot();
        }

        if (!playerHasBredOutAnEgg)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < radius)
            {
                tellPlayerToSquat.SetActive(true);
            }
            else
            {
                tellPlayerToSquat.SetActive(false);
            }
        }
        else
        {
            tellPlayerToSquat.SetActive(false);
        }
    }

    private void UnblockSpot()
    {
        _isBlocked = false;

        blockSpotVisuals.SetActive(false);
    }

    internal void BlockSpot(float blockTime)
    {
        _isBlocked = true;
        blockedTime = blockTime;

        blockSpotVisuals.SetActive(true);
    }

    internal float GetTimeBred()
    {
        return _timeBred;
    }

    internal bool IsBlocked()
    {
        return _isBlocked;
    }

    internal bool IsCloseEnough(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < radius;
    }

    internal bool CanBreed(Vector3 position)
    {
        if (!IsCloseEnough(position)) return false;
        if (_isBlocked) return false;

        return true;
    }

    internal void StartBreeding(BaseChickenController bcc)
    {
        _breeding = true;

        bcc.OnStandUp += StopBreeding;
    }

    private void StopBreeding()
    {
        _breeding = false;
    }
}
