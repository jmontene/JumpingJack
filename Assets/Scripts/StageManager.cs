﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Basic Configuration")]

    public GameObject laneParent;
    public Transform rightSide;
    public Transform leftSide;
    public float holeSpeed = 2f;

    [Header("Pools")]

    public Pool holePool;

    //Properties
    private int _initialHoles = 2;
    public int InitialHoles
    {
        get { return _initialHoles; }
        set { _initialHoles = value; }
    }

    //Private Members

    private Transform[] lanes;
    private List<Hole> holes;
    private List<Hole> holesToAdd;

    void Awake()
    {
        holes = new List<Hole>();
        holesToAdd = new List<Hole>();
        BuildLaneArray();

        for(int i=0;i<InitialHoles;++i) SpawnHoleAtRandom();
    }

    void Update()
    {
        foreach(Hole hole in holes)
        {
            UpdateHole(hole);
        }
        holes.RemoveAll(h => h.MarkedForRemoval);
        holes.AddRange(holesToAdd);
        holesToAdd.Clear();
    }

    private void UpdateHole(Hole hole)
    {
        hole.transform.Translate(Vector2.right * hole.Direction * holeSpeed * Time.deltaTime);

        bool touch = hole.Direction == 1 ? hole.rightEdge >= rightSide.position.x : hole.leftEdge <= leftSide.position.x;
        bool outFrame = hole.Direction == 1 ? hole.leftEdge >= rightSide.position.x : hole.rightEdge <= leftSide.position.x;

        if(!hole.PartiallyOut && touch)
        {
            hole.PartiallyOut = true;
            int nextLane = hole.LaneIndex + (hole.Direction == 1 ? -1 : 1);
            nextLane = nextLane >= 0 ? (nextLane % lanes.Length) : (lanes.Length - 1);
            float offset = (hole.Direction == 1 ? hole.leftEdge - hole.centerX : hole.rightEdge - hole.centerX);
            Transform side = (hole.Direction == 1 ? leftSide : rightSide);
            SpawnMirrorHole(nextLane, offset, hole.Direction, side);
        }else if (hole.PartiallyOut && outFrame)
        {
            hole.MarkedForRemoval = true;
            hole.GetComponent<PoolObject>().Deactivate();
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
        Hole hole = holePool.Get().GetComponent<Hole>();
        hole.Init(dir, laneIndex);

        float left = leftSide.position.x;
        float right = rightSide.position.x;

        Transform lane = lanes[laneIndex];
        hole.transform.position = new Vector2(xPos, lane.position.y);

        if (clampPos)
        {
            if (hole.leftEdge < left) hole.transform.position = new Vector2(left + (hole.centerX - hole.leftEdge), hole.centerY);
            else if (hole.rightEdge > right) hole.transform.position = new Vector2(right - (hole.rightEdge - hole.centerX), hole.centerY);
        }

        if (addToList) holes.Add(hole);
        else holesToAdd.Add(hole);
    }
}
