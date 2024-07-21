using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 2.4f, -3.5f);


    [SerializeField] Transform flyThroughTargetParent;
    List<CameraFlyThroughTarget> flyThroughTargets = new List<CameraFlyThroughTarget>();
    [SerializeField] float flyThroughSpeed = 1.0f;
    [SerializeField] AnimationCurve flySpeedCurve;



    [SerializeField] GameObject skipTutorial;
    [SerializeField] CanvasGroup tutorialPanel;

    [SerializeField] GameObject bottomLeftUI, bottomRightUI;

    [SerializeField] TMP_Text tutorialText;

    [SerializeField] float tutorialFadeDuration = 0.2f;

    [SerializeField] NumberDisplay countdownDisplay;

    [SerializeField] float countdownDuration = 3.0f;


    [SerializeField] int countdownStartNumber = 3;



    [SerializeField] bool skipAnimation = false;

    KinectInputs kinectInputs;
    bool waitingForInput;
    PlayerControls controls;

    Transform target;
    bool flyThroughActive;

    private void Start()
    {
        GameManager.Instance.OnSpawnChicken += TargetCurrentPlayerChicken;

        if (GameManager.Instance.KinectInputs)
        {
            kinectInputs = FindObjectOfType<KinectInputs>();
            kinectInputs.OnSitDown += OnConfirm;
            kinectInputs.OnStandUp += SkipIntro;
        }

        controls = new PlayerControls();
        controls.Enable();

        Transform[] targets = flyThroughTargetParent.GetComponentsInChildren<Transform>();

        foreach (Transform target in targets)
        {
            if (target != flyThroughTargetParent)
            {
                flyThroughTargets.Add(target.GetComponent<CameraFlyThroughTarget>());
            }
        }

        CameraFlyThroughTarget playerTargetTransform = new GameObject().AddComponent< CameraFlyThroughTarget>();
        playerTargetTransform.transform.position = GameManager.Instance.Player.transform.position + offset;
        playerTargetTransform.transform.rotation = GameManager.Instance.Player.transform.rotation;
        playerTargetTransform.transform.Rotate(20, 0, 0);

        flyThroughTargets.Add(playerTargetTransform);

        if (skipAnimation)
        {
            SkipIntro();
        }
        else {
            StartCoroutine(StartGameFlyThrough());
        }
    }

    private void OnConfirm()
    {
        if (!waitingForInput) return;

        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.confirmSound);

        waitingForInput = false;
    }

    private void Update()
    {
        if (controls.Player.Jump.triggered)
        {
            SkipIntro();
        }
        if(controls.Player.Breed.triggered) OnConfirm();

        if (!flyThroughActive) AdjustPositionToNotClip();
    }

    private void AdjustPositionToNotClip()
    {
        // stay at the same height above the target, cast ray in that height backwards from the target, if hit anything move camera to hit point and a bit further

        Vector3 rayStartPosition = target.position + new Vector3(0, offset.y, 0);
        Vector3 direction = -target.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(rayStartPosition, direction, out hit, Mathf.Abs(offset.z)))
        {
            transform.position = hit.point - direction * 0.1f;
        }
        else
        {
            transform.position = rayStartPosition - direction * offset.z;
        }

        
    }

    private void SkipIntro()
    {
        print("Skip");
        StopAllCoroutines();

        skipTutorial.SetActive(false);

        tutorialPanel.gameObject.SetActive(false);

        bottomLeftUI.SetActive(true);
        bottomRightUI.SetActive(true);

        TargetCurrentPlayerChicken();

        TimeManager.Instance.SetTimeScale(1);
    }

    void TargetCurrentPlayerChicken()
    {
        SetTarget(GameManager.Instance.Player.transform);
    }

    public void SetTarget(Transform target)
    {
        transform.parent = target;

        transform.position = target.position + target.forward * offset.z + target.up * offset.y;

        transform.rotation = target.rotation;

        transform.Rotate(Vector3.right, 20);

        this.target = target;
    }

    public IEnumerator StartGameFlyThrough()
    {
        skipTutorial.SetActive(true);
        bottomLeftUI.SetActive(false);
        bottomRightUI.SetActive(false);

        flyThroughActive = true;
        print("StartGameFlyThrough");
        TimeManager.Instance.Pause();

        transform.position = flyThroughTargets[0].transform.position;
        transform.rotation = flyThroughTargets[0].transform.rotation;


        // first target stopping time
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + flyThroughTargets[0].stoppingTime)
        {
            yield return null;
        }

        for (int i = 0; i < flyThroughTargets.Count-1; i++)
        {
            waitingForInput = flyThroughTargets[i].waitForInput;
            if (flyThroughTargets[i].tutorialText != "")
            {
                tutorialText.text = flyThroughTargets[i].tutorialText;
                StartCoroutine(FadeIn(tutorialPanel));
            }

            print("wait for input ");
            // wait for input
            while (waitingForInput)
            {
                if (controls.Player.Breed.triggered) waitingForInput = false;

                yield return null;
            }

            if (flyThroughTargets[i].tutorialText != "")
            {
                StartCoroutine(FadeOut(tutorialPanel));
            }

            float t = 0;
            startTime = Time.realtimeSinceStartup;
            float distanceToNextTarget = Vector3.Distance(flyThroughTargets[i].transform.position, flyThroughTargets[i + 1].transform.position);
            

            print("start fly through");
            
            // fly from current target to next
            while (t < 1)
            {
                t = (Time.realtimeSinceStartup-startTime) * flyThroughSpeed / distanceToNextTarget;
                
                transform.position = Vector3.Lerp(flyThroughTargets[i].transform.position, flyThroughTargets[i+1].transform.position, flyThroughTargets[i].flyPositionCurve.Evaluate(t));
                transform.rotation = Quaternion.Lerp(flyThroughTargets[i].transform.rotation, flyThroughTargets[i+1].transform.rotation, flyThroughTargets[i].flyRotationCurve.Evaluate(t));

                yield return null;
            }

            // wait at next target for next target stopping time
            startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < startTime + flyThroughTargets[i+1].stoppingTime)
            {
                yield return null;
            }
        }

        SetTarget(GameManager.Instance.Player.transform);


        skipTutorial.SetActive(false);

        StartCoroutine(GameStartCountdown());

    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        for (float i = 0; i < tutorialFadeDuration; i += Time.unscaledDeltaTime)
        {
            float percentage = i / tutorialFadeDuration;

            canvasGroup.alpha = percentage;

            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        for (float i = 0; i < tutorialFadeDuration; i += Time.unscaledDeltaTime)
        {
            float percentage = i / tutorialFadeDuration;

            canvasGroup.alpha = 1 - percentage;

            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator GameStartCountdown()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.countdownSound);

        for (float i = 0; i < countdownDuration; i+= Time.unscaledDeltaTime)
        {
            int number = countdownStartNumber - (int)(i / countdownDuration * countdownStartNumber);

            countdownDisplay.SetNumber(number);

            yield return null;
        }

        countdownDisplay.gameObject.SetActive(false);

        TimeManager.Instance.SetTimeScale(1);

        flyThroughActive = false;
    }
}
