using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KinectCalibration : MonoBehaviour
{
    enum CalibrationPhase
    {
        ConnectWithKinect,
        Stand,
        Squat,
        HeadForward,
        RotLeft,
        RotRight,
        Jump,
        DropEgg,
        SetInputThresholdValues
    }


    KinectBody kinectBody;
    KinectInputs inputs;

    CalibrationPhase currentCalibrationPhase;
    [SerializeField] CalibrationValues calibrationValues;

    [SerializeField] TMP_Text calibrationText;
    [SerializeField] TMP_Text problemText;

    int currentPhaseStepIndex;

    Queue<Vector3> headPositionQueue = new Queue<Vector3>();
    Queue<Vector3> pelvisPositionQueue = new Queue<Vector3>();
    Queue<Vector3> leftHandPositionQueue = new Queue<Vector3>();
    Queue<Vector3> rightHandPositionQueue = new Queue<Vector3>();

    [SerializeField] int calibrationQueueSize = 50;

    [SerializeField] float distancePercentageToTriggerInput = 0.7f;
    [SerializeField] float moveDistancePercentageToTriggerInput = 0.7f;
    [SerializeField] float rotateDistancePercentageToTriggerInput = 0.7f;
    [SerializeField] float jumpDistancePercentageToTriggerInput = 0.7f;

    // Connect To Kinect

    Vector3 pelvisLastPosition = Vector3.zero;

    // Stand

    [SerializeField] float notMovingThreshold = 0.15f;

    // Squat

    [SerializeField] float squatMinValue = 0.1f;

    // Head Forward

    [SerializeField] float headForwardMinDistance = 0.2f;

    // Rotation

    [SerializeField] float headSidewaysMinDistance = 0.1f;

    // Jump

    [SerializeField] float minJumpArmsDistance = 0.3f;

    // Drop Egg


    [SerializeField] float minDropEggArmsDistance = 0.3f;


    private void Awake()
    {
        kinectBody = FindObjectOfType<KinectBody>();
        inputs = FindObjectOfType<KinectInputs>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCalibrationPhase = CalibrationPhase.ConnectWithKinect;

        calibrationValues.squatDistancePercentageToTriggerInput = distancePercentageToTriggerInput;
        calibrationValues.moveDistancePercentageToTriggerInput = moveDistancePercentageToTriggerInput;
        calibrationValues.rotateDistancePercentageToTriggerInput = rotateDistancePercentageToTriggerInput;
        calibrationValues.jumpDistancePercentageToTriggerInput = jumpDistancePercentageToTriggerInput;
    }


    // Update is called once per frame
    void Update()
    {
        switch (currentCalibrationPhase)
        {
            case CalibrationPhase.ConnectWithKinect:
                ConnectWithKinect();
                break;

            case CalibrationPhase.Stand:
                StandCalibration();
                break;

            case CalibrationPhase.Squat:
                SquatCalibration();
                break;

            case CalibrationPhase.HeadForward:
                HeadForwardCalibration();
                break;

            case CalibrationPhase.RotLeft:
                RotLeftCalibration();
                break;

            case CalibrationPhase.RotRight:
                RotRightCalibration();
                break;

            case CalibrationPhase.Jump:
                JumpCalibration();
                break;
            case CalibrationPhase.DropEgg:
                DropEggCalibration();
                break;
            case CalibrationPhase.SetInputThresholdValues:
                SetInputThresholdValues();
                break;
        }

        pelvisLastPosition = kinectBody.pelvis.position;
    }

    private void SetInputThresholdValues()
    {
        print("Calibration Done!");
        inputs.SetCalibrationValues(calibrationValues);

        SceneManager.LoadScene("MainMenu");
    }

    void ChangePhase(CalibrationPhase newPhase)
    {
        currentCalibrationPhase = newPhase;
        currentPhaseStepIndex = 0;

        calibrationText.text = newPhase.ToString();

        print("Change Phase to: " + newPhase);
    }

    private Vector3 MeanOfArray(Vector3[] array)
    {
        Vector3 value = Vector3.zero;

        for (int i = 0; i < array.Length; i++)
        {
            value += array[i];
        }
        value /= array.Length;

        return value;
    }

    private bool PlayerIsMoving()
    {
        return Vector3.Distance(pelvisLastPosition, kinectBody.pelvis.position) > notMovingThreshold;
    }

    private bool PlayerIsStanding()
    {
        return Vector3.Distance(calibrationValues.standPelvisMeanPosition, kinectBody.pelvis.position) < notMovingThreshold;
    }

    private void ConnectWithKinect()
    {
        print("Pelvis: " + kinectBody.pelvis.position);

        if (pelvisLastPosition != Vector3.zero && pelvisLastPosition != kinectBody.pelvis.position)
        {
            ChangePhase(CalibrationPhase.Stand);
        }

    }

    private void StandCalibration()
    {
        // Panel: Stand straight in front of the camera in a comfortable position

        headPositionQueue.Enqueue(kinectBody.head.position);
        pelvisPositionQueue.Enqueue(kinectBody.pelvis.position);
        leftHandPositionQueue.Enqueue(kinectBody.leftHand.position);
        rightHandPositionQueue.Enqueue(kinectBody.rightHand.position);

        if (headPositionQueue.Count > calibrationQueueSize)
        {
            headPositionQueue.Dequeue();
            pelvisPositionQueue.Dequeue();
            leftHandPositionQueue.Dequeue();
            rightHandPositionQueue.Dequeue();
        }

        if (headPositionQueue.Count == calibrationQueueSize &&
            Vector3.Distance(pelvisPositionQueue.Peek(), kinectBody.pelvis.position) < notMovingThreshold &&
            Vector3.Distance(leftHandPositionQueue.Peek(), kinectBody.leftHand.position) < notMovingThreshold &&
            Vector3.Distance(rightHandPositionQueue.Peek(), kinectBody.rightHand.position) < notMovingThreshold)
        {
            calibrationValues.standHeadMeanPosition = MeanOfArray(headPositionQueue.ToArray());
            calibrationValues.standPelvisMeanPosition = MeanOfArray(pelvisPositionQueue.ToArray());
            calibrationValues.standLeftHandMeanPosition = MeanOfArray(leftHandPositionQueue.ToArray());
            calibrationValues.standRightHandMeanPosition = MeanOfArray(rightHandPositionQueue.ToArray());

            calibrationValues.standHandsMeanPosition = (calibrationValues.standLeftHandMeanPosition + calibrationValues.standRightHandMeanPosition) / 2;

            print("HeadMean:" + calibrationValues.standHeadMeanPosition);
            print("Pelvis: " + calibrationValues.standPelvisMeanPosition);
            print("Left Hand: " + calibrationValues.standLeftHandMeanPosition);
            print("Right Hand: " + calibrationValues.standRightHandMeanPosition);

            ChangePhase(CalibrationPhase.Squat);


        }

    }



    private void SquatCalibration()
    {
        // squat down and hold position

        if (currentPhaseStepIndex == 0)
        {
            pelvisPositionQueue.Clear();
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        if (PlayerIsMoving())
        {
            problemText.text = "Stay still! Movement Distance: " + Vector3.Distance(pelvisLastPosition, kinectBody.pelvis.position).ToString();
            ChangePhase(CalibrationPhase.Squat);
            return;
        }
        else problemText.text = "";


        if (kinectBody.pelvis.position.y < calibrationValues.standPelvisMeanPosition.y - squatMinValue)
        {
            pelvisPositionQueue.Enqueue(kinectBody.pelvis.position);
            headPositionQueue.Enqueue(kinectBody.head.position);
        }
        else
        {
            print("not low enough: " + (calibrationValues.standPelvisMeanPosition.y - kinectBody.pelvis.position.y).ToString());
        }


        if (pelvisPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.squatPelvisMeanPosition = MeanOfArray(pelvisPositionQueue.ToArray());
            calibrationValues.squatHeadMeanPosition = MeanOfArray(headPositionQueue.ToArray());

            calibrationValues.squatDistance = calibrationValues.standPelvisMeanPosition.y - calibrationValues.squatPelvisMeanPosition.y;

            ChangePhase(CalibrationPhase.HeadForward);
        }


    }

    private void HeadForwardCalibration()
    {
        // stand up again

        if (currentPhaseStepIndex == 0)
        {
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        if (PlayerIsMoving() || !PlayerIsStanding())
        {
            ChangePhase(CalibrationPhase.HeadForward);
            return;
        }

        if (kinectBody.head.position.z < calibrationValues.standHeadMeanPosition.z - headForwardMinDistance)
        {
            headPositionQueue.Enqueue(kinectBody.head.position);
        }

        if (headPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.headForwardMeanPosition = MeanOfArray(headPositionQueue.ToArray());

            calibrationValues.headForwardDistance = calibrationValues.standHeadMeanPosition.z - calibrationValues.headForwardMeanPosition.z;

            ChangePhase(CalibrationPhase.RotLeft);
        }


    }

    private void RotLeftCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        if (PlayerIsMoving())
        {
            problemText.text = "Stop Moving!";
            ChangePhase(CalibrationPhase.RotLeft);
            return;
        }
        else if (!PlayerIsStanding())
        {
            problemText.text = "Stand Up!";
            ChangePhase(CalibrationPhase.RotLeft);
            return;
        }
        else problemText.text = "";

        if (kinectBody.head.position.x > calibrationValues.standHeadMeanPosition.x + headSidewaysMinDistance)
        {
            headPositionQueue.Enqueue(kinectBody.head.position);
        }
        else
        {
            problemText.text = "Lean further: " + (calibrationValues.standHeadMeanPosition.x - kinectBody.head.position.x).ToString();
        }

        if (headPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.headLeftMeanPosition = MeanOfArray(headPositionQueue.ToArray());

            ChangePhase(CalibrationPhase.RotRight);
        }
    }

    private void RotRightCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        if (PlayerIsMoving() || !PlayerIsStanding())
        {
            ChangePhase(CalibrationPhase.RotRight);
            return;
        }

        if (kinectBody.head.position.x < calibrationValues.standHeadMeanPosition.x - headSidewaysMinDistance)
        {
            headPositionQueue.Enqueue(kinectBody.head.position);
        }
        else
        {
            problemText.text = "Lean further: " + (calibrationValues.standHeadMeanPosition.x - kinectBody.head.position.x).ToString();
        }

        if (headPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.headRightMeanPosition = MeanOfArray(headPositionQueue.ToArray());

            calibrationValues.headSidewaysDistance = (calibrationValues.standHeadMeanPosition.x - calibrationValues.headRightMeanPosition.x
                                + calibrationValues.headLeftMeanPosition.x - calibrationValues.standHeadMeanPosition.x) / 2;

            ChangePhase(CalibrationPhase.Jump);
        }
    }

    private void JumpCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            leftHandPositionQueue.Clear();
            rightHandPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        if (PlayerIsMoving() || !PlayerIsStanding())
        {
            ChangePhase(CalibrationPhase.Jump);
            return;
        }

        if (    kinectBody.leftHand.position.y > calibrationValues.standLeftHandMeanPosition.y + minJumpArmsDistance
            &&  kinectBody.rightHand.position.y > calibrationValues.standRightHandMeanPosition.y + minJumpArmsDistance)
        {
            leftHandPositionQueue.Enqueue(kinectBody.leftHand.position);
            rightHandPositionQueue.Enqueue(kinectBody.rightHand.position);
        }

        if (leftHandPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.jumpLeftHandMeanPosition = MeanOfArray(leftHandPositionQueue.ToArray());
            calibrationValues.jumpRightHandMeanPosition = MeanOfArray(rightHandPositionQueue.ToArray());
            calibrationValues.jumpHandsMeanPosition = (calibrationValues.jumpLeftHandMeanPosition + calibrationValues.jumpRightHandMeanPosition) / 2;

            calibrationValues.jumpDistance = (calibrationValues.jumpLeftHandMeanPosition.y - calibrationValues.standLeftHandMeanPosition.y
                                + calibrationValues.jumpRightHandMeanPosition.y - calibrationValues.standRightHandMeanPosition.y) / 2;

            ChangePhase(CalibrationPhase.DropEgg);
        }
    }

    private void DropEggCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            leftHandPositionQueue.Clear();
            rightHandPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        if (PlayerIsMoving() || PlayerIsStanding())
        {
            ChangePhase(CalibrationPhase.DropEgg);
            return;
        }

        if (kinectBody.leftHand.position.x < calibrationValues.standLeftHandMeanPosition.x + minDropEggArmsDistance
            && kinectBody.rightHand.position.x > calibrationValues.standRightHandMeanPosition.x + minDropEggArmsDistance)
        {
            leftHandPositionQueue.Enqueue(kinectBody.leftHand.position);
            rightHandPositionQueue.Enqueue(kinectBody.rightHand.position);
        }

        if (leftHandPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.leftHandStretchPosition = MeanOfArray(leftHandPositionQueue.ToArray());
            calibrationValues.rightHandStretchPosition = MeanOfArray(rightHandPositionQueue.ToArray());

            calibrationValues.handsStretchDistance = Mathf.Abs(calibrationValues.leftHandStretchPosition.x - calibrationValues.rightHandStretchPosition.x);

            ChangePhase(CalibrationPhase.SetInputThresholdValues);
        }
    }
}
