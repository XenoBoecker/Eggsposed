public class CalibrationChickenKinectInputManager : ChickenInputManager
{
    KinectCalibration calibration;

    int lastCalibrationQueueCount;
    
    
    private void Update()
    {
        if (calibration.CurrentCalibrationPhase == CalibrationPhase.Squat)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount) SitDown();
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.Jump)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount) Jump();
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.HeadForward)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount) Move(new UnityEngine.Vector2(0,1));
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.DropEgg)
        {
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount) PickupDropEgg();
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.Call)
        {
            GetComponent<ChickenCallSoundManager>().enabled = true;
            
            if (calibration.CalibrationQueueCounter > lastCalibrationQueueCount) Call();
        }

        lastCalibrationQueueCount = calibration.CalibrationQueueCounter;
    }
}