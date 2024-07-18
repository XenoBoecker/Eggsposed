public class CalibrationChickenKinectInputManager : ChickenInputManager
{
    KinectInputs inputs;

    KinectCalibration calibration;

    bool canMove;
    bool canJump;
    bool canSit;
    bool canDropEgg;
    bool canCall;
    
    
    private void Update()
    {
        if (calibration.CurrentCalibrationPhase == CalibrationPhase.Squat && !canSit)
        {
            canSit = true;
            inputs.OnSitDown += SitDown;
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.Jump && !canJump)
        {
            canJump = true;

            inputs.OnJump += Jump;
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.HeadForward && !canMove)
        {
            canMove = true;
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.DropEgg && !canDropEgg)
        {
            canDropEgg = true;
            inputs.OnDropEgg += PickupDropEgg;
        }
        else if (calibration.CurrentCalibrationPhase == CalibrationPhase.Call && !canCall)
        {
            canCall = true;

            GetComponent<ChickenCallSoundManager>().enabled = true;
        }

        if(canMove) Move(inputs.MoveInput);

    }
}