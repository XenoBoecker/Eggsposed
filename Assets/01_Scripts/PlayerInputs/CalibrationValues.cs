using UnityEngine;

[CreateAssetMenu(fileName = "CalibrationValues", menuName = "CalibrationValues")]
public class CalibrationValues : ScriptableObject
{
    [Header("TriggerInputPercentages")]
    public float squatDistancePercentageToTriggerInput;
    public float moveDistancePercentageToTriggerInput;
    public float rotateDistancePercentageToTriggerInput;
    public float jumpDistancePercentageToTriggerInput;
    public float stretchDistancePercentageToTriggerInput;
    public float loundessPercentageToTriggerInput;

    [Header("Calculated Distances")]
    public float squatDistance;
    public float headForwardDistance;
    public float headSidewaysDistance;
    public float jumpDistance;
    public float handsStretchDistance;
    public float ambientToCallNoiseDifference;

    [Header("Stand Calibration")]
    public Vector3 standHeadMeanPosition;
    public Vector3 standPelvisMeanPosition;
    public Vector3 standLeftHandMeanPosition;
    public Vector3 standRightHandMeanPosition;
    public Vector3 standHandsMeanPosition;

    [Header("Squat Calibration")]
    public Vector3 squatHeadMeanPosition;
    public Vector3 squatPelvisMeanPosition;

    [Header("Move Calibration")]
    public Vector3 headForwardMeanPosition;

    [Header("Rotate Calibration")]
    public Vector3 headLeftMeanPosition;
    public Vector3 headRightMeanPosition;

    [Header("Jump Calibration")]
    public Vector3 jumpLeftHandMeanPosition;
    public Vector3 jumpRightHandMeanPosition;
    public Vector3 jumpHandsMeanPosition;
    
    [Header("Breed Calibration")]
    public Vector3 leftHandStretchPosition;
    public Vector3 rightHandStretchPosition;

    [Header("Audio Calibration")]
    public float ambientNoiseMaxValue;
    public float callNoiseMeanValue;
}
