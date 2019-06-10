using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpSpeed = 3f;
    public float jumpHeight = 1.75f;
    public float crashHeight = 0.45f;

    [Header("Positioning")]
    public Transform rightSide;
    public Transform leftSide;

    [Header("Timing")]
    public float crashTime = 0.5f;
    public float maxDizzyTime = 3f;
    public float minDizzyTime = 1f;

    [Header("Collisions")]
    public LayerMask holeLayer;
    public LayerMask hazardLayer;
    public float skinHeight = 0.2f;

    private StageManager _stage;
    public StageManager Stage
    {
        get { return _stage; }
        set { _stage = value; }
    }

    private float _dizzyTimer;
    public float DizzyTimer
    {
        get { return _dizzyTimer; }
        set { _dizzyTimer = value; }
    }

    private float _dizzyTime;
    public float DizzyTime
    {
        get { return _dizzyTime; }
        set { _dizzyTime = value; }
    }

    private int _lane;
    public int Lane
    {
        get { return _lane; }
        set { _lane = value; }
    }

    private StateMachine playerStateMachine;
    private float floorDistance = 0.01f;

    // Start is called before the first frame update
    void Awake()
    {
        playerStateMachine = new StateMachine();
        InitializeStates();
        playerStateMachine.Start("Idle");
        Lane = -1;
    }

    void Update()
    {
        playerStateMachine.UpdateMachine();
    }

    public void RefreshDizzyTimer()
    {
        DizzyTimer = 0f;
        DizzyTime = Random.Range(minDizzyTime, maxDizzyTime);
    }

    public bool CheckFloorHole()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, floorDistance, holeLayer);
        return hit;
    }

    public bool CheckHazardCollision()
    {
        float right = rightSide.position.x;
        float left = leftSide.position.x;
        float center = transform.position.x;
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + Vector3.up * skinHeight, Vector2.right, right - center, hazardLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + Vector3.up * skinHeight, Vector2.left, center - left, hazardLayer);

        return rightHit || leftHit;
    }

    private void InitializeStates()
    {
        playerStateMachine.AddState("Idle", new PlayerStateIdle(this));
        playerStateMachine.AddState("WrapAround", new PlayerStateWrapAround(this));
        playerStateMachine.AddState("Crashing", new PlayerStateCrashing(this));
        playerStateMachine.AddState("FallingCrash", new PlayerStateFallingCrash(this));
        playerStateMachine.AddState("Dizzy", new PlayerStateDizzy(this));
        playerStateMachine.AddState("Jumping", new PlayerStateJumping(this));
        playerStateMachine.AddState("Falling", new PlayerStateFalling(this));
        playerStateMachine.AddState("Hazard", new PlayerStateHazard(this));
        playerStateMachine.AddState("GameOver", new PlayerStateGameOver(this));
    }
}
