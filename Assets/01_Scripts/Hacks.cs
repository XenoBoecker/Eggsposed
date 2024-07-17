using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{

    [SerializeField] GameObject chicken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) GameManager.Instance.HackSpawnNextChicken();

        if (Input.GetKeyDown(KeyCode.L)) GameManager.Instance.GameOver();

        if (Input.GetKeyDown(KeyCode.M)) Instantiate(chicken);
    }
}