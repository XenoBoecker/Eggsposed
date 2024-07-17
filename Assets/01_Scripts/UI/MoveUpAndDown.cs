using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{

    [SerializeField] float moveSpeed = 3;

    [SerializeField] float moveDistance = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // sin up and down movement

        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance, transform.position.z);

    }
}
