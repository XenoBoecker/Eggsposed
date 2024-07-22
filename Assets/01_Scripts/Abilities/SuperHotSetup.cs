using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SuperHotSetup : ChickenAbilitySetup
{
    [SerializeField] float timeSlowFactor = 0.1f;


    [SerializeField] float fadeDuration = 0.2f;

    [SerializeField] GameObject timeSlowVolumePrefab;
    Volume timeSlowVolume;


    [SerializeField] GameObject slowDownVFX;

    float baseAngularSpeed;

    bool passiveTimeslowDeactivated;

    ChickenStateTracker cst;
    AudioSource audioSource;

    float lastFrameTimescale;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        this.chicken = chicken;

        baseAngularSpeed = bcc.angularSpeed;

        cst = GetComponent<ChickenStateTracker>();

        cst.OnSitDown += StartSittingSound;
        cst.OnStandUp += StopSittingSound;

        audioSource = new GameObject().AddComponent<AudioSource>();
        audioSource.transform.position = transform.position;
        audioSource.transform.SetParent(transform);

        timeSlowVolume = Instantiate(timeSlowVolumePrefab, transform).GetComponentInChildren<Volume>();
    }

    private void StartSittingSound()
    {
        if (!bcc.breeding) return;
        SoundManager.Instance.StartLoopingSound(SoundManager.Instance.chickenSFX.superHotBreedingSound, audioSource);
    }

    private void StopSittingSound()
    {
        SoundManager.Instance.EndLoopingSound(audioSource);
    }

    protected override void Update()
    {
        base.Update();

        if (!chicken.IsControlledByPlayer || chicken != GameManager.Instance.Player)
        {
            if (!passiveTimeslowDeactivated)
            {
                print("deactivateTimeSlow");
                passiveTimeslowDeactivated = true;
                SetTimeNormal();
            }
            return;
        }
        

        if (!bcc.sitting && bcc.moveDirection.z <= 0)
        {
            SetTimeSlow();
        }
        else
        {
            SetTimeNormal();
        }

        lastFrameTimescale = Time.timeScale;
    }

    void SetTimeSlow()
    {
        if (Time.timeScale == 0) return;

        TimeManager.Instance.SetTimeScale(timeSlowFactor);

        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.superHotSlowDown);

        bcc.angularSpeed = baseAngularSpeed / timeSlowFactor;

        if (lastFrameTimescale == 1)
        {
            Instantiate(slowDownVFX, transform);
            StartCoroutine(ChangeVolumeWeight(0, 1, fadeDuration));
        }
    }

    void SetTimeNormal()
    {
        if (Time.timeScale == 0) return;

        TimeManager.Instance.SetTimeScale(1);

        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.superHotSpeedUp);

        bcc.angularSpeed = baseAngularSpeed;

        if(lastFrameTimescale != 1) StartCoroutine(ChangeVolumeWeight(1, 0, fadeDuration));
    }

    private IEnumerator ChangeVolumeWeight(int v1, int v2, float v3)
    {
        float elapsedTime = 0;
        float t;

        while (elapsedTime < v3)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / v3;

            timeSlowVolume.weight = Mathf.Lerp(v1, v2, t);

            yield return null;
        }

    }
}