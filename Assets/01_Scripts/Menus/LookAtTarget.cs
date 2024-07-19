using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{

    [SerializeField] Transform target;

    private void Start()
    {
        if (target == null) target = Camera.main.transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.Rotate(0, 180, 0);
    }
}
