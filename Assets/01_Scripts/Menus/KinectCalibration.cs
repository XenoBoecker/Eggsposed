using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CalibrationPhase
{
    ConnectWithKinect,
    Stand,
    HeadForward,
    RotLeft,
    RotRight,
    Jump,
    Squat,
    DropEgg,
    AmbientNoise,
    Call,
    SetInputThresholdValues
}
public class KinectCalibration : MonoBehaviour
{

    [System.Serializable]
    public class CalibrationTexts
    {
        public string[] confirmationLines;

        public string connectingHeading;
        public string connectingDescription;

        public string standHeading;
        public string standDescription;

        public string headForwardHeading;
        public string headForwardDescription;

        public string rotLeftHeading;
        public string rotLeftDescription;

        public string rotRightHeading;
        public string rotRightDescription;

        public string jumpHeading;
        public string jumpDescription;

        public string glideHeading;
        public string glideDescription;

        public string squatHeading;
        public string squatDescription;

        public string hatchHeading;
        public string hatchDescription;

        public string dropEggHeading;
        public string dropEggDescription;

        public string dropEggTutorialHeading;
        public string dropEggTutorialDescription;

        public string ambientNoiseHeading;
        public string ambientNoiseDescription;

        public string callHeading;
        public string callDescription;

        public string stopMovingText;
        public string connectingText;
        public string standText;
        public string leanFurtherText;
        public string raiseArmsMoreText;
        public string squatLowerText;
        public string stretchArmsMoreText;
        public string screamLouderText;
    }

    CalibrationTexts calibrationTexts;
    int randomConfirmationLineIndex;


    KinectBody kinectBody;
    KinectInputs inputs;

    CalibrationPhase currentCalibrationPhase;
    public CalibrationPhase CurrentCalibrationPhase => currentCalibrationPhase;
    [SerializeField] CalibrationValues calibrationValues;

    [SerializeField] TMP_Text phaseText;
    [SerializeField] TMP_Text problemText;
    [SerializeField] TMP_Text descriptionText;
    

    int currentPhaseStepIndex;
    float waitTimer;

    Queue<Vector3> headPositionQueue = new Queue<Vector3>();
    Queue<Vector3> pelvisPositionQueue = new Queue<Vector3>();
    Queue<Vector3> leftHandPositionQueue = new Queue<Vector3>();
    Queue<Vector3> rightHandPositionQueue = new Queue<Vector3>();

    Queue<float> loudnessQueue = new Queue<float>();

    [SerializeField] int calibrationQueueSize = 50;
    public int CalibrationQueueSize => calibrationQueueSize;

    int calibrationQueueCounter;
    public int CalibrationQueueCounter => calibrationQueueCounter;

    // Connect To Kinect

    Vector3 pelvisLastPosition = Vector3.zero;


    [SerializeField] float connectingWaitTime = 5f;

    // Stand

    [SerializeField] float notMovingThreshold = 0.15f;

    [SerializeField] float standThreshold = 0.2f;

    // Head Forward

    [SerializeField] float headForwardMinDistance = 0.2f;

    // Rotation

    [SerializeField] float headSidewaysMinDistance = 0.1f;

    // Jump

    [SerializeField] float minJumpArmsDistance = 0.3f;

    // Glide


    [SerializeField] float glideWaitTime = 5f;

    // Squat

    [SerializeField] float squatMinValue = 0.1f;

    // Hatch


    [SerializeField] float hatchWaitTime = 5f;

    // Drop Egg


    [SerializeField] float minDropEggArmsDistance = 0.3f;

    // Call


    [SerializeField] float minCallToAmbientNoiseDifference = 0.1f;

    public float currentLoudness;

    private void Awake()
    {
        kinectBody = FindObjectOfType<KinectBody>();
        inputs = FindObjectOfType<KinectInputs>();
    }

    // Start is called before the first frame update
    void Start()
    {

        LoadCalibrationTexts();

        ChangePhase(CalibrationPhase.ConnectWithKinect);
    }
    
    void LoadCalibrationTexts()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("calibrationTexts");
        if (jsonTextFile != null)
        {
            calibrationTexts = JsonUtility.FromJson<CalibrationTexts>(jsonTextFile.text);
        }
        else
        {
            Debug.LogError("Could not find tutorialTexts.json in Resources folder.");
        }
    }
    void SetText(TMP_Text textComponent, string text)
    {
        textComponent.text = text;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangePhase((CalibrationPhase)((int)currentCalibrationPhase + 1));
        }

        currentLoudness = AudioLoudnessDetection.GetLoudnessFromMicrophone();

        switch (currentCalibrationPhase)
        {
            case CalibrationPhase.ConnectWithKinect:
                ConnectWithKinect();
                break;

            case CalibrationPhase.Stand:
                StandCalibration();
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

            case CalibrationPhase.Squat:
                SquatCalibration();
                break;

            case CalibrationPhase.DropEgg:
                DropEggCalibration();
                break;

            case CalibrationPhase.AmbientNoise:
                AmbientNoiseCalibration();
                break;

            case CalibrationPhase.Call:
                CallCalibration();
                break;

            case CalibrationPhase.SetInputThresholdValues:
                SetInputThresholdValues();
                break;
        }

        pelvisLastPosition = kinectBody.pelvis.position;
    }

    private void SetInputThresholdValues()
    {
        inputs.SetCalibrationValues(calibrationValues);

        SceneManager.LoadScene("MainMenu");
    }

    void ChangePhase(CalibrationPhase newPhase)
    {
        if (newPhase != currentCalibrationPhase) randomConfirmationLineIndex = UnityEngine.Random.Range(0, calibrationTexts.confirmationLines.Length);

        currentCalibrationPhase = newPhase;
        currentPhaseStepIndex = 0;

        SetText(phaseText, GetPhaseText(newPhase));
        SetText(descriptionText, GetDescriptionText(newPhase));
        
    }

    private string GetPhaseText(CalibrationPhase newPhase)
    {
        switch (newPhase)
        {
            case CalibrationPhase.ConnectWithKinect:
                return calibrationTexts.connectingHeading;
            case CalibrationPhase.Stand:
                return calibrationTexts.standHeading;
            case CalibrationPhase.HeadForward:
                return calibrationTexts.headForwardHeading;
            case CalibrationPhase.RotLeft:
                return calibrationTexts.rotLeftHeading;
            case CalibrationPhase.RotRight:
                return calibrationTexts.rotRightHeading;
            case CalibrationPhase.Jump:
                return calibrationTexts.jumpHeading;
            case CalibrationPhase.Squat:
                return calibrationTexts.squatHeading;
            case CalibrationPhase.DropEgg:
                return calibrationTexts.dropEggHeading;
            case CalibrationPhase.AmbientNoise:
                return calibrationTexts.ambientNoiseHeading;
            case CalibrationPhase.Call:
                return calibrationTexts.callHeading;
            case CalibrationPhase.SetInputThresholdValues:
                return "Calibration Done!";
        }

        return "";
    }

    private string GetDescriptionText(CalibrationPhase newPhase)
    {
        switch (newPhase)
        {
            case CalibrationPhase.ConnectWithKinect:
                return calibrationTexts.connectingDescription;
            case CalibrationPhase.Stand:
                return calibrationTexts.standDescription;
            case CalibrationPhase.HeadForward:
                return calibrationTexts.headForwardDescription;
                case CalibrationPhase.RotLeft:
                return calibrationTexts.rotLeftDescription;
                case CalibrationPhase.RotRight:
                return calibrationTexts.rotRightDescription;
            case CalibrationPhase.Jump:
                return calibrationTexts.jumpDescription;
            case CalibrationPhase.Squat:
                return calibrationTexts.squatDescription;
            case CalibrationPhase.DropEgg:
                return calibrationTexts.dropEggDescription;
            case CalibrationPhase.AmbientNoise:
                return calibrationTexts.ambientNoiseDescription;
            case CalibrationPhase.SetInputThresholdValues:
                return calibrationTexts.callDescription;
            default:
                return "";
        }

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

    float MeanOfArray(float[] array)
    {
        float value = 0;

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
        return kinectBody.pelvis.position.y > calibrationValues.standPelvisMeanPosition.y - standThreshold;
    }

    private void ConnectWithKinect()
    {
        if (currentPhaseStepIndex == 0)
        {
            waitTimer = 0;
            currentPhaseStepIndex++;
        }
        waitTimer += Time.deltaTime;

        SetText(problemText, calibrationTexts.connectingText);

        if (pelvisLastPosition != Vector3.zero && pelvisLastPosition != kinectBody.pelvis.position && waitTimer >= connectingWaitTime)
        {
            ChangePhase(CalibrationPhase.Stand);
        }

    }

    private void StandCalibration()
    {        
        if (currentPhaseStepIndex == 0)
        {
            calibrationQueueCounter = 0;
            headPositionQueue.Clear();
            pelvisPositionQueue.Clear();
            leftHandPositionQueue.Clear();
            rightHandPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        problemText.text = "";

        if (PlayerIsMoving())
        {
            problemText.text = calibrationTexts.stopMovingText;
            ChangePhase(CalibrationPhase.Stand);
            return;
        }

        SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);

        headPositionQueue.Enqueue(kinectBody.head.position);
        pelvisPositionQueue.Enqueue(kinectBody.pelvis.position);
        leftHandPositionQueue.Enqueue(kinectBody.leftHand.position);
        rightHandPositionQueue.Enqueue(kinectBody.rightHand.position);

        calibrationQueueCounter = headPositionQueue.Count;

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

            ChangePhase(CalibrationPhase.HeadForward);
        }

    }

    private void HeadForwardCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            calibrationQueueCounter = 0;
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }
        
        SetText(problemText, "");        

        if (PlayerIsMoving())
        {
            SetText(problemText, calibrationTexts.stopMovingText);
            ChangePhase(CalibrationPhase.HeadForward);
            return;
        }

        if (!PlayerIsStanding())
        {
            SetText(problemText, calibrationTexts.standText);
            ChangePhase(CalibrationPhase.HeadForward);
            return;
        }

        if (kinectBody.head.position.z < calibrationValues.standHeadMeanPosition.z - headForwardMinDistance)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);
            headPositionQueue.Enqueue(kinectBody.head.position);
            calibrationQueueCounter = headPositionQueue.Count;
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
            calibrationQueueCounter = 0;
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        SetText(problemText, "");

        if (PlayerIsMoving())
        {
            SetText(problemText, calibrationTexts.stopMovingText);
            ChangePhase(CalibrationPhase.RotLeft);
            return;
        }
        else if (!PlayerIsStanding())
        {
            SetText(problemText, calibrationTexts.standText);
            ChangePhase(CalibrationPhase.RotLeft);
            return;
        }
        else
        {
            SetText(problemText, "");
        }

        if (kinectBody.head.position.x > calibrationValues.standHeadMeanPosition.x + headSidewaysMinDistance)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);
            headPositionQueue.Enqueue(kinectBody.head.position);
            calibrationQueueCounter = headPositionQueue.Count;
        }
        else
        {
            SetText(problemText, calibrationTexts.leanFurtherText);
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
            calibrationQueueCounter = 0;
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }

        SetText(problemText, "");        

        if (PlayerIsMoving())
        {
            SetText(problemText, calibrationTexts.stopMovingText);

            ChangePhase(CalibrationPhase.RotRight);
            return;
        }

        if (!PlayerIsStanding())
        {
            SetText(problemText, calibrationTexts.standText);
            ChangePhase(CalibrationPhase.RotRight);
            return;
        }

        if (kinectBody.head.position.x < calibrationValues.standHeadMeanPosition.x - headSidewaysMinDistance)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);
            headPositionQueue.Enqueue(kinectBody.head.position);
            calibrationQueueCounter = headPositionQueue.Count;
        }
        else
        {
            SetText(problemText, calibrationTexts.leanFurtherText);
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
            calibrationQueueCounter = 0;
            leftHandPositionQueue.Clear();
            rightHandPositionQueue.Clear();
            currentPhaseStepIndex++;
        }
        
        SetText(problemText, "");

        if (PlayerIsMoving())
        {
            SetText(problemText, calibrationTexts.stopMovingText);
            ChangePhase(CalibrationPhase.Jump);
            return;
        }

        if (!PlayerIsStanding())
        {
            SetText(problemText, calibrationTexts.standText);
            ChangePhase(CalibrationPhase.Jump);
            return;
        }

        if (    kinectBody.leftHand.position.y > calibrationValues.standLeftHandMeanPosition.y + minJumpArmsDistance
            &&  kinectBody.rightHand.position.y > calibrationValues.standRightHandMeanPosition.y + minJumpArmsDistance)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);
            leftHandPositionQueue.Enqueue(kinectBody.leftHand.position);
            rightHandPositionQueue.Enqueue(kinectBody.rightHand.position);
            calibrationQueueCounter = leftHandPositionQueue.Count;
        }

        if (leftHandPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.jumpLeftHandMeanPosition = MeanOfArray(leftHandPositionQueue.ToArray());
            calibrationValues.jumpRightHandMeanPosition = MeanOfArray(rightHandPositionQueue.ToArray());
            calibrationValues.jumpHandsMeanPosition = (calibrationValues.jumpLeftHandMeanPosition + calibrationValues.jumpRightHandMeanPosition) / 2;

            calibrationValues.jumpDistance = (calibrationValues.jumpLeftHandMeanPosition.y - calibrationValues.standLeftHandMeanPosition.y
                                + calibrationValues.jumpRightHandMeanPosition.y - calibrationValues.standRightHandMeanPosition.y) / 2;

            ChangePhase(CalibrationPhase.Squat);
        }
    }



    private void SquatCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            calibrationQueueCounter = 0;
            pelvisPositionQueue.Clear();
            headPositionQueue.Clear();
            currentPhaseStepIndex++;
        }
        
        SetText(problemText, "");

        if (PlayerIsMoving())
        {
            SetText(problemText, calibrationTexts.stopMovingText);
            ChangePhase(CalibrationPhase.Squat);
            return;
        }


        if (kinectBody.pelvis.position.y < calibrationValues.standPelvisMeanPosition.y - squatMinValue)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);

            pelvisPositionQueue.Enqueue(kinectBody.pelvis.position);
            headPositionQueue.Enqueue(kinectBody.head.position);
            calibrationQueueCounter = headPositionQueue.Count;
        }
        else
        {
            SetText(problemText, calibrationTexts.squatLowerText);
        }


        if (pelvisPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.squatPelvisMeanPosition = MeanOfArray(pelvisPositionQueue.ToArray());
            calibrationValues.squatHeadMeanPosition = MeanOfArray(headPositionQueue.ToArray());

            calibrationValues.squatDistance = calibrationValues.standPelvisMeanPosition.y - calibrationValues.squatPelvisMeanPosition.y;

            ChangePhase(CalibrationPhase.DropEgg);
        }


    }

    private void DropEggCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            calibrationQueueCounter = 0;
            leftHandPositionQueue.Clear();
            rightHandPositionQueue.Clear();
            currentPhaseStepIndex++;
        }
        
        SetText(problemText, "");

        if (PlayerIsMoving())
        {
            SetText(problemText, calibrationTexts.stopMovingText);
            ChangePhase(CalibrationPhase.DropEgg);
            return;
        }

        if (PlayerIsStanding())
        {
            SetText(problemText, calibrationTexts.squatLowerText);

            ChangePhase(CalibrationPhase.DropEgg);
            return;
        }

        if (Mathf.Abs(kinectBody.leftHand.position.x - kinectBody.rightHand.position.x) > minDropEggArmsDistance)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);
            leftHandPositionQueue.Enqueue(kinectBody.leftHand.position);
            rightHandPositionQueue.Enqueue(kinectBody.rightHand.position);
            calibrationQueueCounter = leftHandPositionQueue.Count;
        }
        else
        {
            SetText(problemText, calibrationTexts.stretchArmsMoreText);
        }

        if (leftHandPositionQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.leftHandStretchPosition = MeanOfArray(leftHandPositionQueue.ToArray());
            calibrationValues.rightHandStretchPosition = MeanOfArray(rightHandPositionQueue.ToArray());

            calibrationValues.handsStretchDistance = Mathf.Abs(calibrationValues.leftHandStretchPosition.x - calibrationValues.rightHandStretchPosition.x);

            ChangePhase(CalibrationPhase.AmbientNoise);
        }
    }

    private void AmbientNoiseCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            calibrationQueueCounter = 0;
            loudnessQueue.Clear();
            currentPhaseStepIndex++;
        }
        
        SetText(problemText, "");

        loudnessQueue.Enqueue(AudioLoudnessDetection.GetLoudnessFromMicrophone());
        calibrationQueueCounter = loudnessQueue.Count;


        if (loudnessQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.ambientNoiseMaxValue = MeanOfArray(loudnessQueue.ToArray());

            ChangePhase(CalibrationPhase.Call);
        }
    }

    private void CallCalibration()
    {
        if (currentPhaseStepIndex == 0)
        {
            calibrationQueueCounter = 0;
            loudnessQueue.Clear();
            currentPhaseStepIndex++;
        }
        
        SetText(problemText, "");

        float currentLoudness = AudioLoudnessDetection.GetLoudnessFromMicrophone();

        if (currentLoudness > calibrationValues.ambientNoiseMaxValue + minCallToAmbientNoiseDifference)
        {
            SetText(problemText, calibrationTexts.confirmationLines[randomConfirmationLineIndex]);
            loudnessQueue.Enqueue(currentLoudness);
            calibrationQueueCounter = loudnessQueue.Count;
        }
        else
        {
            SetText(problemText, calibrationTexts.screamLouderText);
        }


        if (loudnessQueue.Count >= calibrationQueueSize)
        {
            calibrationValues.callNoiseMeanValue = MaxOfArray(loudnessQueue.ToArray());

            calibrationValues.ambientToCallNoiseDifference = Mathf.Abs(calibrationValues.ambientNoiseMaxValue - calibrationValues.callNoiseMeanValue);

            ChangePhase(CalibrationPhase.SetInputThresholdValues);
        }
    }

    private float MaxOfArray(float[] floats)
    {
        float max = 0;

        foreach (float f in floats)
        {
            if (f > max) max = f;
        }

        return max;
    }
}
