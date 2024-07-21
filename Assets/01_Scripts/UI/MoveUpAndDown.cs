using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{

    [SerializeField] float moveSpeed = 3;

    [SerializeField] float moveDistance = 0.3f;

    Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // sin up and down movement

        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * moveSpeed) * moveDistance, 0);

    }
}
