using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{

    [SerializeField] Transform target;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.Rotate(0, 180, 0);
    }
}
