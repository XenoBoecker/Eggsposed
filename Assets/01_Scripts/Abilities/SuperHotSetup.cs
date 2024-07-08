using UnityEngine;

public class SuperHotSetup : ChickenAbilitySetup
{
    [SerializeField] float timeSlowFactor = 0.1f;

    TimeManager timeManager;

    Chicken chicken;

    float baseAngularSpeed;

    bool passiveTimeslowDeactivated;

    public override void Setup(Chicken chicken)
    {
        base.Setup(chicken);

        this.chicken = chicken;

        baseAngularSpeed = bcc.angularSpeed;
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

        bcc.angularSpeed = baseAngularSpeed / timeSlowFactor;
    }

    void SetTimeNormal()
    {
        if (Time.timeScale == 0) return;

        TimeManager.Instance.SetTimeScale(1);

        bcc.angularSpeed = baseAngularSpeed;
    }
}