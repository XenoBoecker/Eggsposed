using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) GameManager.Instance.HackSpawnNextChicken();

        if (Input.GetKeyDown(KeyCode.L)) GameManager.Instance.GameOver();
    }
}
