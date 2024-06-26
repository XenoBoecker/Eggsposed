using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingSpot : MonoBehaviour
{
    [SerializeField] float radius = 3f;
    internal bool IsCloseEnough(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < radius;
    }
}
