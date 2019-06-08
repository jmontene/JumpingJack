using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Basic Configuration")]

    public GameObject laneParent;
    public GameObject hazardParent;
    public Transform rightSide;
    public Transform leftSide;
    public Player player;

    [Header("Holes")]

    public Pool holePool;
    public int maxHoles = 8;
    public float holeSpeed = 2f;
    public float successfulJumpMultiplier = 0.5f;

    [Header("Hazards")]
    public float hazardAnimSpeed = 0.1f;
    public float hazardHitTime = 0.3f;

    [Header("Background")]
    public SpriteRenderer backgroundRenderer;
    public Color idleColor;
    public Color hurtColor;
    public Color hazardColor;

    //Properties
    private int _initialHoles = 2;
    public int InitialHoles
    {
        get { return _initialHoles; }
        set { _initialHoles = value; }
    }
    private bool _updateActive = true;
    public bool UpdateActive
    {
        get { return _updateActive; }
        set { _updateActive = value; }
    }

    //Private Members

    private Transform[] lanes;
    private List<StageObject> holes;
    private List<StageObject> holesToAdd;
    private List<Hazard> hazards;
    private float speedMultiplier = 1f;
    private int curHoleNumber = 0;
    private float hazardTimer = 0f;

    void Awake()
    {
        holes = new List<StageObject>();
        holesToAdd = new List<StageObject>();
        hazards = new List<Hazard>();
        BuildLaneArray();

        for(int i=0;i<InitialHoles;++i) SpawnHoleAtRandom();
        player.Stage = this;
    }

    private void Start()
    {
        foreach(Hazard hazard in GameManager.Instance.CurrentHazards)
        {
            SpawnHazard(hazard);
        }
    }

    void Update()
    {
        if (!UpdateActive) return;

        foreach(StageObject hole in holes)
        {
            UpdateHole(hole);
        }

        hazardTimer += Time.deltaTime;
        bool switchAnim = false;
        if(hazardTimer >= hazardAnimSpeed)
        {
            hazardTimer = 0f;
            switchAnim = true;
        }

        foreach(Hazard h in hazards)
        {
            UpdateHazard(h, switchAnim);
        }

        holes.RemoveAll(h => h.MarkedForRemoval);
        holes.AddRange(holesToAdd);
        holesToAdd.Clear();
    }

    public void IdleColor()
    {
        ChangeBackgroundColor(idleColor);
    }

    public void HurtColor()
    {
        ChangeBackgroundColor(hurtColor);
    }

    public void HazardColor()
    {
        ChangeBackgroundColor(hazardColor);
    }

    public void SlowHoles()
    {
        speedMultiplier = successfulJumpMultiplier;
    }

    public void NormalSpeedHoles()
    {
        speedMultiplier = 1f;
    }

    public void OnSuccessfulJump()
    {
        SpawnHoleAtRandom();
    }

    private void ChangeBackgroundColor(Color c)
    {
        backgroundRenderer.color = c;
    }

    private void UpdateStageObject(StageObject obj)
    {
        obj.transform.Translate(Vector2.right * obj.Direction * holeSpeed * speedMultiplier * Time.deltaTime);
    }

    private void UpdateHazard(Hazard obj, bool switchSprite)
    {
        UpdateStageObject(obj);
        if (switchSprite)
        {
            obj.SwapSprite();
        }

        bool outFrame = obj.rightEdge <= leftSide.position.x;
        if (outFrame)
        {
            obj.LaneIndex = (obj.LaneIndex + 1) % (lanes.Length-1);
            obj.transform.position = new Vector2(rightSide.position.x, lanes[obj.LaneIndex].position.y);
        }
    }

    private void UpdateHole(StageObject obj)
    {
        UpdateStageObject(obj);
        bool touch = obj.Direction == 1 ? obj.rightEdge >= rightSide.position.x : obj.leftEdge <= leftSide.position.x;
        bool outFrame = obj.Direction == 1 ? obj.leftEdge >= rightSide.position.x : obj.rightEdge <= leftSide.position.x;

        if (!obj.PartiallyOut && touch)
        {
            obj.PartiallyOut = true;
            int nextLane = obj.LaneIndex + (obj.Direction == 1 ? -1 : 1);
            nextLane = nextLane >= 0 ? (nextLane % lanes.Length) : (lanes.Length - 1);
            float offset = (obj.Direction == 1 ? obj.leftEdge - obj.centerX : obj.rightEdge - obj.centerX);
            Transform side = (obj.Direction == 1 ? leftSide : rightSide);
            SpawnMirrorHole(nextLane, offset, obj.Direction, side);
        }
        else if (obj.PartiallyOut && outFrame)
        {
            obj.MarkedForRemoval = true;
            curHoleNumber--;
            obj.GetComponent<PoolObject>().Deactivate();
        }
    }

    private void BuildLaneArray()
    {
        lanes = new Transform[laneParent.transform.childCount];
        for(int i = 0; i < lanes.Length; ++i)
        {
            lanes[i] = laneParent.transform.GetChild(i);
        }
    }

    private void SpawnHoleAtRandom()
    {
        SpawnHole(Random.Range(0, lanes.Length), Random.Range(leftSide.position.x, rightSide.position.x));
    }

    private void SpawnMirrorHole(int laneIndex, float offset, int dir, Transform side)
    {
        SpawnHole(laneIndex, side.position.x+offset, dir, false, false);
    }

    private void SpawnHole(int laneIndex, float xPos, int dir = 0, bool clampPos = true, bool addToList = true)
    {
        if (curHoleNumber == maxHoles) return;
        curHoleNumber++;

        StageObject obj = SpawnStageObject(holePool, laneIndex, xPos, dir, clampPos);

        if (addToList) holes.Add(obj);
        else holesToAdd.Add(obj);
    }

    private void SpawnHazard(Hazard h)
    {
        int lane = Random.Range(0, lanes.Length-1);
        float xPos = Random.Range(leftSide.position.x, rightSide.position.x);
        Hazard obj = Instantiate<Hazard>(h, hazardParent.transform);
        SpawnStageObject(obj, lane, xPos, -1);
        hazards.Add(obj);
    }

    private StageObject SpawnStageObject(Pool pool, int laneIndex, float xPos, int dir = 0, bool clampPos = true)
    {
        StageObject obj = pool.Get().GetComponent<StageObject>();
        return SpawnStageObject(obj, laneIndex, xPos, dir, clampPos);
    }

    private StageObject SpawnStageObject(StageObject obj, int laneIndex, float xPos, int dir = 0, bool clampPos = true)
    {
        obj.Init(dir, laneIndex);

        float left = leftSide.position.x;
        float right = rightSide.position.x;

        Transform lane = lanes[laneIndex];
        obj.transform.position = new Vector2(xPos, lane.position.y);

        if (clampPos)
        {
            if (obj.leftEdge < left) obj.transform.position = new Vector2(left + (obj.centerX - obj.leftEdge), obj.centerY);
            else if (obj.rightEdge > right) obj.transform.position = new Vector2(right - (obj.rightEdge - obj.centerX), obj.centerY);
        }

        return obj;
    }
}
