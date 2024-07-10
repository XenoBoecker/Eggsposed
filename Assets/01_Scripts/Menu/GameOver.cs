using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    List<ChickenData> bredChicken = new List<ChickenData>();
    // Start is called before the first frame update
    void Start()
    {
        bredChicken = GameOverInfo.GetBredChickens();

        foreach (ChickenData chicken in bredChicken)
        {
            Debug.Log(chicken.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
