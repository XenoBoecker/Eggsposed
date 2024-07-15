using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingSpot : MonoBehaviour
{
    [SerializeField] float radius = 3f;


    [SerializeField] GameObject blockSpotVisuals;

    bool _isBlocked;

    float _timeBred;

    bool _breeding;

    float blockedTime;

    private void Start()
    {
        blockSpotVisuals.SetActive(false);
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
