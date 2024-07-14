using UnityEngine;

[CreateAssetMenu(fileName = "CalibrationValues", menuName = "CalibrationValues")]
public class CalibrationValues : ScriptableObject
{
    public Vector3 standHeadMeanPosition;
    public Vector3 standPelvisMeanPosition;
    public Vector3 standLeftHandMeanPosition;
    public Vector3 standRightHandMeanPosition;
    public Vector3 standHandsMeanPosition;

    public float squatDistancePercentageToTriggerInput;
    public Vector3 squatHeadMeanPosition;
    public Vector3 squatPelvisMeanPosition;

    public float moveDistancePercentageToTriggerInput;
    public Vector3 headForwardMeanPosition;

    public float rotateDistancePercentageToTriggerInput;
    public Vector3 headLeftMeanPosition;
    public Vector3 headRightMeanPosition;

    public float jumpDistancePercentageToTriggerInput;
    public Vector3 jumpLeftHandMeanPosition;
    public Vector3 jumpRightHandMeanPosition;
    public Vector3 jumpHandsMeanPosition;

    public float stretchDistancePercentageToTriggerInput;
    public Vector3 leftHandStretchPosition;
    public Vector3 rightHandStretchPosition;

    public float loundessPercentageToTriggerInput;
    public float ambientNoiseMeanValue;
    public float callNoiseMeanValue;


    public float squatDistance;
    public float headForwardDistance;
    public float headSidewaysDistance;
    public float jumpDistance;
    public float handsStretchDistance;
    public float ambientToCallNoiseDifference;
}
