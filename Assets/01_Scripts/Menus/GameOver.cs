using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform camStartPos, camFinalPos;
    [SerializeField] float cameraTransitionTime = 2f;
    [SerializeField] AnimationCurve cameraTransitionCurve;

    [SerializeField] float delayBeforeEggsDrop = 1f;
    [SerializeField] float delayBeforeCamAnimation = 1f;

    [SerializeField] float eggDropRange = 0.6f;

    [SerializeField] int testEggCount = 10;
    [SerializeField] ChickenData testChicken;

    List<ChickenData> bredChicken = new List<ChickenData>();

    public event Action OnShowLeaderboard;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("OnlyShowLeaderboard") == 1)
        {
            Invoke("OnlyShowLeaderboard", 0.1f);

            return;
        }

        Time.timeScale = 1;

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
        yield return new WaitForSeconds(delayBeforeEggsDrop);
        foreach (ChickenData chicken in bredChicken)
        {
            SpawnChickenEgg(chicken.eggVisual);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 0.8f));
        }

        yield return new WaitForSeconds(delayBeforeCamAnimation);

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

        OnShowLeaderboard?.Invoke();
    }

    private void SpawnChickenEgg(GameObject eggVisual)
    {
        GameObject egg = Instantiate(eggVisual, new Vector3(UnityEngine.Random.Range(-eggDropRange, eggDropRange), 8, UnityEngine.Random.Range(-eggDropRange, eggDropRange)), Quaternion.identity);

        if (egg.GetComponent<Rigidbody>() == null)
        {
            egg.AddComponent<Rigidbody>();
        }
        if (egg.GetComponent<Collider>() == null) egg.AddComponent<SphereCollider>();
    }

    void OnlyShowLeaderboard()
    {
        cam.position = camFinalPos.position;
        cam.rotation = camFinalPos.rotation;

        OnShowLeaderboard?.Invoke();
    }
}
