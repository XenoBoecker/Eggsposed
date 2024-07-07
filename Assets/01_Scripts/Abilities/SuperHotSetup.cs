using UnityEngine;

public class SuperHotSetup : ChickenAbilitySetup
{
    [SerializeField] float timeSlowFactor = 0.1f;

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

        Time.timeScale = timeSlowFactor;

        bcc.angularSpeed = baseAngularSpeed / timeSlowFactor;
        print("slow");
    }

    void SetTimeNormal()
    {
        if (Time.timeScale == 0) return;
        
        Time.timeScale = 1;

        bcc.angularSpeed = baseAngularSpeed;
        print("normal");
    }
}