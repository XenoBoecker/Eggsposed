using System;
using TMPro;
using UnityEngine;

public class CalibrationChickenKinectInputManager : ChickenInputManager
{
    KinectCalibration calibration;

    int lastCalibrationQueueCount;

    [SerializeField] TMP_Text debugText;

    void SetDebugText(string text)
    {

    debugText.text = text;
    }

    private void Start()
    {
        calibration = FindObjectOfType<KinectCalibration>();
    }

    private void Update()
    {
        if (calibration.CurrentCalibrationPhase == CalibrationPhase.Squat)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("Sit down!");
                SitDown();
            }
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.Jump)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("Jump!");
                Jump();
            }
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.HeadForward)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("Move");
                Move(new UnityEngine.Vector2(0, 1));
            }
        }
        else if(calibration.CurrentCalibrationPhase == CalibrationPhase.RotLeft)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("RotLeft");
                Move(new UnityEngine.Vector2(-1, 0));
            }
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.RotRight)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("RotRight");
                Move(new UnityEngine.Vector2(1, 0));
            }
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.DropEgg)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("DropEgg!");
                PickupDropEgg();
            }
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.Call)
        {
            GetComponent<ChickenCallSoundManager>().enabled = true;

            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount)
            {
                SetDebugText("Call!");
                Call();
            }
        }

        lastCalibrationQueueCount = calibration.CalibrationQueueCounter;
    }
}