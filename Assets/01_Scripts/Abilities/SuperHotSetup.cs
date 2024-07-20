using UnityEngine;

public class SuperHotSetup : ChickenAbilitySetup
{
    [SerializeField] float timeSlowFactor = 0.1f;

    float baseAngularSpeed;

    bool passiveTimeslowDeactivated;

    ChickenStateTracker cst;
    AudioSource audioSource;

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

        if (!chicken.IsControlledByPlayer)
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
    }

    void SetTimeSlow()
    {
        if (Time.timeScale == 0) return;

        TimeManager.Instance.SetTimeScale(timeSlowFactor);

        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.superHotSlowDown);

        bcc.angularSpeed = baseAngularSpeed / timeSlowFactor;
    }

    void SetTimeNormal()
    {
        if (Time.timeScale == 0) return;

        TimeManager.Instance.SetTimeScale(1);

        SoundManager.Instance.PlaySound(SoundManager.Instance.chickenSFX.superHotSpeedUp);

        bcc.angularSpeed = baseAngularSpeed;
    }
}