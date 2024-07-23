using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggRotatingPart : MonoBehaviour
{

    [SerializeField] Vector3 rotSpeed;

    [SerializeField] float rotAngleUntilPause = 90f;

    [SerializeField] float pauseDuration = 0.2f;

    float timer;
    bool rotating;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (rotating)
        {
            transform.Rotate(rotSpeed * Time.deltaTime);
            if (timer > rotAngleUntilPause / rotSpeed.magnitude)
            {
                rotating = false;
                timer = 0;
            }
        }
        else
        {
            if (timer > pauseDuration)
            {
                rotating = true;
                timer = 0;
            }
        }

    }
}
