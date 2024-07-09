using System.Collections.Generic;
using UnityEngine;

public class KinectInputs : ChickenInputManager
{
    KinectBody kinectBody;

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

    bool breeding;
    bool moving;
    bool flying;

    // Start is called before the first frame update
    void Awake()
    {
        kinectBody = FindObjectOfType<KinectBody>();

        head = kinectBody.head;
        pelvis = kinectBody.pelvis;
        leftHand = kinectBody.leftHand;
        rightHand = kinectBody.rightHand;
    }

    void Update()
    {
        Vector2 moveInput = Vector2.zero;

        moveInput += CheckHeadForwardMovement();

        moveInput += CheckHeadSidewaysPosition();

        CheckPelvisHeightChange();

        CheckHandsHeightChange();

        if (breeding) print("breeding");
        if (moving) print("moving");
        if (flying) print("flying");

        print("Input: " + moveInput);
        Move(moveInput);
    }

    private void CheckHandsHeightChange()
    {
        float currentHandHeight = leftHand.transform.position.y + rightHand.transform.position.y / 2;
        float heightChange = currentHandHeight - handsLastHeights.Peek();

        if (heightChange > jumpHeightChangeThreshold) Jump();
        else if (heightChange < -jumpHeightChangeThreshold) StopJump();
    }

    private void CheckPelvisHeightChange()
    {
        if (pelvisLastHeights.Count < 1) return;
        
        float heightChange = pelvis.position.y - pelvisLastHeights.Peek();

        if (heightChange > breedHeightChangeThreshold) StandUp();
        else if (heightChange < -breedHeightChangeThreshold) SitDown();
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
    }
}
