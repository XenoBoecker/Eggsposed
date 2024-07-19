using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 2.4f, -3.5f);


    [SerializeField] Transform flyThroughTargetParent;
    List<Transform> flyThroughTargets = new List<Transform>();
    [SerializeField] float flyThroughSpeed = 1.0f;
    [SerializeField] float stopAtTargetTime = 0.2f;
    [SerializeField] AnimationCurve flySpeedCurve;


    [SerializeField] NumberDisplay countdownDisplay;

    [SerializeField] float countdownDuration = 3.0f;


    [SerializeField] int countdownStartNumber = 3;



    [SerializeField] bool skipAnimation = false;

    private void Start()
    {
        GameManager.Instance.OnSpawnChicken += TargetNewChicken;

        Transform[] targets = flyThroughTargetParent.GetComponentsInChildren<Transform>();

        foreach (Transform target in targets)
        {
            if (target != flyThroughTargetParent)
            {
                flyThroughTargets.Add(target);
            }
        }

        Transform playerTargetTransform = new GameObject().transform;
        playerTargetTransform.position = GameManager.Instance.Player.transform.position + offset;
        playerTargetTransform.rotation = GameManager.Instance.Player.transform.rotation;
        playerTargetTransform.Rotate(20, 0, 0);

        flyThroughTargets.Add(playerTargetTransform);

        if(!skipAnimation) StartCoroutine(StartGameFlyThrough());
    }

    void TargetNewChicken()
    {
        SetTarget(GameManager.Instance.Player.transform);
    }

    public void SetTarget(Transform target)
    {
        transform.parent = target;

        transform.position = target.position + target.forward * offset.z + target.up * offset.y;

        transform.rotation = target.rotation;

        transform.Rotate(Vector3.right, 20);
    }

    public IEnumerator StartGameFlyThrough()
    {
        TimeManager.Instance.Pause();

        transform.position = flyThroughTargets[0].position;
        transform.rotation = flyThroughTargets[0].rotation;

        float startTime = Time.realtimeSinceStartup;
        startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + stopAtTargetTime)
        {
            yield return null;
        }


        for (int i = 0; i < flyThroughTargets.Count-1; i++)
        {
            float t = 0;
            startTime = Time.realtimeSinceStartup;
            float distanceToNextTarget = Vector3.Distance(flyThroughTargets[i].position, flyThroughTargets[i + 1].position);

            while (t < 1)
            {
                t = (Time.realtimeSinceStartup-startTime) * flyThroughSpeed / distanceToNextTarget;
                
                transform.position = Vector3.Lerp(flyThroughTargets[i].position, flyThroughTargets[i+1].position, flySpeedCurve.Evaluate(t));
                transform.rotation = Quaternion.Lerp(flyThroughTargets[i].rotation, flyThroughTargets[i+1].rotation, flySpeedCurve.Evaluate(t));

                yield return null;
            }

            startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + stopAtTargetTime)
            {
                yield return null;
            }
        }

        SetTarget(GameManager.Instance.Player.transform);


        StartCoroutine(GameStartCountdown());

    }

    private IEnumerator GameStartCountdown()
    {
        for (float i = 0; i < countdownDuration; i+= Time.unscaledDeltaTime)
        {
            int number = countdownStartNumber - (int)(i / countdownDuration * countdownStartNumber);

            countdownDisplay.SetNumber(number);

            yield return null;
        }

        countdownDisplay.gameObject.SetActive(false);

        TimeManager.Instance.SetTimeScale(1);
    }
}
