using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform camStartPos, camFinalPos;
    [SerializeField] float cameraTransitionTime = 2f;
    [SerializeField] AnimationCurve cameraTransitionCurve;

    [SerializeField] int testEggCount = 10;
    [SerializeField] ChickenData testChicken;

    List<ChickenData> bredChicken = new List<ChickenData>();
    // Start is called before the first frame update
    void Start()
    {
        bredChicken = GameOverInfo.GetBredChickens();

        if (bredChicken.Count == 0)
        {
            for (int i = 0; i < testEggCount; i++)
            {

                bredChicken.Add(testChicken);
            }
        }

        foreach (ChickenData chicken in bredChicken)
        {
            Debug.Log(chicken.ToString());
        }

        StartCoroutine(DropEggs());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DropEggs()
    {
        yield return new WaitForSeconds(1f);
        foreach (ChickenData chicken in bredChicken)
        {
            SpawnChickenEgg(chicken.eggVisual);

            yield return new WaitForSeconds(Random.Range(0.5f, 0.8f));
        }

        StartCoroutine(CameraAnimation());
    }

    IEnumerator CameraAnimation()
    {
        for (float i = 0; i < cameraTransitionTime; i+= Time.deltaTime)
        {
            float percentage = cameraTransitionCurve.Evaluate(i / cameraTransitionTime);

            cam.position = Vector3.Lerp(camStartPos.position, camFinalPos.position, percentage);
            cam.rotation = Quaternion.Lerp(camStartPos.rotation, camFinalPos.rotation, percentage);
            yield return null;
        }
    }

    private void SpawnChickenEgg(GameObject eggVisual)
    {
        GameObject egg = Instantiate(eggVisual, new Vector3(Random.Range(-1f, 1f), 8, Random.Range(-1f, 1f)), Quaternion.identity);

        if(egg.GetComponent<Collider>() == null) egg.AddComponent<SphereCollider>();
    }
}
