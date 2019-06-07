using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 1.75f;
    public Transform rightSide;
    public Transform leftSide;

    private StageManager _stage;
    public StageManager Stage
    {
        get { return _stage; }
        set { _stage = value; }
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

    private void InitializeStates()
    {
        playerStateMachine.AddState("Idle", new PlayerStateIdle(this));
        playerStateMachine.AddState("WrapAround", new PlayerStateWrapAround(this));
    }
}
