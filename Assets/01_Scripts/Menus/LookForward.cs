using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForward : MonoBehaviour
{
    Vector3 startLookDir;

    // Start is called before the first frame update
    void Start()
    {
        startLookDir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = startLookDir;
    }
}
