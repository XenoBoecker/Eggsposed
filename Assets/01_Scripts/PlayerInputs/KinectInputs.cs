using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class KinectInputs : MonoBehaviour
{
    static KinectInputs Instance;

    KinectBody kinectBody;
    [SerializeField] CalibrationValues calibrationValues;
    public CalibrationValues CalibrationValues => calibrationValues;


    [SerializeField] AnimationCurve rotationInputCurve;

    Transform head;

    Transform pelvis;

    Transform leftHand, rightHand;

    Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    public event Action OnJump;
    public event Action OnStopJump;
    public event Action OnSitDown;
    public event Action OnStandUp;
    public event Action OnDropEgg;

    //float printTimer = 0;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        kinectBody = FindObjectOfType<KinectBody>();

        head = kinectBody.head;
        pelvis = kinectBody.pelvis;
        leftHand = kinectBody.leftHand;
        rightHand = kinectBody.rightHand;

        //OnStandUp += () => print("Stand Up");
        //OnSitDown += () => print("Sit Down");
        //OnJump += () => print("Jump");
        //OnStopJump += () => print("Stop Jump");
    }

    void Update()
    {
        // printTimer += Time.deltaTime;

        moveInput = Vector2.zero;

        moveInput += CheckHeadForwardMovement();

        moveInput += CheckHeadSidewaysPosition();

        CheckPelvisHeightChange();

        CheckHandsHeightChange();

        //print("MoveInput:" + moveInput);
    }

    private Vector2 CheckHeadForwardMovement()
    {
        int moveInput = 0;

        float currentHeadZPosition = head.position.z;

       // if(printTimer > 1) print("Head Z Position: " + currentHeadZPosition + "; stopValue: " + (calibrationValues.headForwardMeanPosition.z + calibrationValues.headForwardDistance * calibrationValues.moveDistancePercentageToTriggerInput).ToString()
        //    + "; forwardValue:" + (calibrationValues.standHeadMeanPosition.z - calibrationValues.headForwardDistance * calibrationValues.moveDistancePercentageToTriggerInput).ToString());

        if (currentHeadZPosition > calibrationValues.headForwardMeanPosition.z + calibrationValues.headForwardDistance * calibrationValues.moveDistancePercentageToTriggerInput) moveInput = 0;
        else moveInput = 1;

        return new Vector2(0, moveInput);
    }

    private Vector2 CheckHeadSidewaysPosition()
    {
        float rotDir = 0;

        float currentHeadXPosition = head.position.x;

        float rotRange = calibrationValues.headSidewaysDistance * (1 - calibrationValues.rotateDistancePercentageToTriggerInput);

        //if (printTimer > 1)
        //{
        //    print("Head X Position: " + currentHeadXPosition + "; rotLeft: " + (calibrationValues.standHeadMeanPosition.x + calibrationValues.headSidewaysDistance * calibrationValues.rotateDistancePercentageToTriggerInput).ToString()+
        //    "; rotRight: " + (calibrationValues.standHeadMeanPosition.x - calibrationValues.headSidewaysDistance * calibrationValues.rotateDistancePercentageToTriggerInput).ToString());
        //    printTimer = 0;
        //}

        if (currentHeadXPosition > calibrationValues.standHeadMeanPosition.x + calibrationValues.headSidewaysDistance * calibrationValues.rotateDistancePercentageToTriggerInput)
        {
            float valueOverRange = currentHeadXPosition - (calibrationValues.standHeadMeanPosition.x + calibrationValues.headSidewaysDistance * calibrationValues.rotateDistancePercentageToTriggerInput);

            rotDir = -rotationInputCurve.Evaluate(Mathf.Min(valueOverRange / rotRange, 1));
        }
        else if (currentHeadXPosition < calibrationValues.standHeadMeanPosition.x - calibrationValues.headSidewaysDistance * calibrationValues.rotateDistancePercentageToTriggerInput)
        {
            float valueOverRange = (calibrationValues.standHeadMeanPosition.x + calibrationValues.headSidewaysDistance * calibrationValues.rotateDistancePercentageToTriggerInput) - currentHeadXPosition;

            rotDir = rotationInputCurve.Evaluate(Mathf.Min(valueOverRange / rotRange, 1));
        }
        //else print("Dont Rotate");

        return new Vector2(rotDir, 0);
    }

    private void CheckPelvisHeightChange()
    {
        float currentPelvisHeight = pelvis.position.y;

        if (currentPelvisHeight > calibrationValues.squatPelvisMeanPosition.y + calibrationValues.squatDistance * calibrationValues.squatDistancePercentageToTriggerInput) OnStandUp?.Invoke();
        else
        {
            OnSitDown?.Invoke();

            CheckDropEgg();
        }
    }
    
    private void CheckHandsHeightChange()
    {
        float currentHandHeight = leftHand.transform.position.y + rightHand.transform.position.y / 2;

        if (currentHandHeight > calibrationValues.standHandsMeanPosition.y + calibrationValues.jumpDistance * calibrationValues.jumpDistancePercentageToTriggerInput) OnJump?.Invoke();
        else OnStopJump?.Invoke();
    }

    private void CheckDropEgg()
    {
        float currentHandDistance = Mathf.Abs(leftHand.position.x - rightHand.position.x);
        
        if (currentHandDistance > calibrationValues.handsStretchDistance * calibrationValues.stretchDistancePercentageToTriggerInput) OnDropEgg?.Invoke();
    }



    internal void SetCalibrationValues(CalibrationValues calibrationValues)
    {
        this.calibrationValues = calibrationValues;
    }







    /*
     * 
     * KinectBody kinectBody;
    CalibrationValues calibrationValues;

    [SerializeField] int savePositionCount = 10;
    public int SavePositionCount => savePositionCount;
    [SerializeField] float headMoveThreshold = 0.2f;
    Transform head;
    Queue<Vector3> headLastPositions = new Queue<Vector3>();

    Transform pelvis;
    public Transform Pelvis => pelvis;
    [SerializeField] float rotateOffsetThreshold = 0.2f;

    Queue<float> pelvisLastHeights = new Queue<float>();
    [SerializeField] float breedHeightChangeThreshold = 0.2f;
    public float BreedHeightChangeThreshold => breedHeightChangeThreshold;

    Transform leftHand, rightHand;
    Queue<float> handsLastHeights = new Queue<float>();
    [SerializeField] float jumpHeightChangeThreshold = 0.4f;

    Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    bool breeding;
    bool moving;
    bool flying;

    public event Action OnJump;
    public event Action OnStopJump;
    public event Action OnSitDown;
    public event Action OnStandUp;

    private void CheckHandsHeightChange()
    {
        float currentHandHeight = leftHand.transform.position.y + rightHand.transform.position.y / 2;
        float heightChange = currentHandHeight - handsLastHeights.Peek();
        
        if (heightChange > jumpHeightChangeThreshold) OnJump?.Invoke(); 
        else if (heightChange < -jumpHeightChangeThreshold) OnStopJump?.Invoke();
    }

    private void CheckPelvisHeightChange()
    {
        if (pelvisLastHeights.Count < 1) return;
        
        float heightChange = pelvis.position.y - pelvisLastHeights.Peek();
       
        if (heightChange > breedHeightChangeThreshold) OnStandUp?.Invoke();
        else if (heightChange < -breedHeightChangeThreshold) OnSitDown?.Invoke();
    }

    private Vector2 CheckHeadSidewaysPosition()
    {
        float offsetX = head.position.x - pelvis.position.x;
        
        int rotDir = 0;
        
        if (offsetX > rotateOffsetThreshold) rotDir = -1;
        else if (offsetX < -rotateOffsetThreshold) rotDir = 1;
        
        return new Vector2(rotDir, 0);
    }

    private Vector2 CheckHeadForwardMovement()
    {

        if (headLastPositions.Count < 1) return Vector2.zero;
        Vector3 headMovement = head.position - headLastPositions.Peek();
        
        if (headMovement.z > headMoveThreshold) moving = false;
        else if (headMovement.z < -headMoveThreshold) moving = true;
        
        if (moving) return Vector2.up;
        else return Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (headLastPositions.Count > savePositionCount)
        {
            headLastPositions.Dequeue();
        }
        headLastPositions.Enqueue(head.position);
        
        if (pelvisLastHeights.Count > savePositionCount) pelvisLastHeights.Dequeue();
        pelvisLastHeights.Enqueue(pelvis.position.y);
        
        if (handsLastHeights.Count > savePositionCount) handsLastHeights.Dequeue();
        handsLastHeights.Enqueue((leftHand.position.y + rightHand.position.y / 2));
    }*/
}
