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
    public float dizzyTime = 3f;

    [Header("Collisions")]
    public LayerMask holeLayer;

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

    private StateMachine playerStateMachine;

    // Start is called before the first frame update
    void Awake()
    {
        playerStateMachine = new StateMachine();
        InitializeStates();
        playerStateMachine.Start("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.UpdateMachine();
    }

    public void RefreshDizzyTimer()
    {
        DizzyTimer = 0f;
    }

    private void InitializeStates()
    {
        playerStateMachine.AddState("Idle", new PlayerStateIdle(this));
        playerStateMachine.AddState("WrapAround", new PlayerStateWrapAround(this));
        playerStateMachine.AddState("Crashing", new PlayerStateCrashing(this));
        playerStateMachine.AddState("FallingCrash", new PlayerStateFallingCrash(this));
        playerStateMachine.AddState("Dizzy", new PlayerStateDizzy(this));
        playerStateMachine.AddState("Jumping", new PlayerStateJumping(this));
    }
}
