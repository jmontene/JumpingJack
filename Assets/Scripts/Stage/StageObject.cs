using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public Transform rightSide;
    public Transform leftSide;

    public void Init(int dir, int lane)
    {
        Direction = (dir == 0 ? (Random.Range(0, 2) == 0 ? -1 : 1) : dir);
        PartiallyOut = false;
        LaneIndex = lane;
        MarkedForRemoval = false;
    }

    public float leftEdge
    {
        get { return leftSide.position.x; }
    }

    public float rightEdge
    {
        get { return rightSide.position.x; }
    }

    public float centerX
    {
        get { return transform.position.x; }
    }

    public float centerY
    {
        get { return transform.position.y; }
    }

    private int _direction = 1;
    public int Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    private bool _partiallyOut = false;
    public bool PartiallyOut
    {
        get { return _partiallyOut; }
        set { _partiallyOut = value; }
    }

    private int _laneIndex = 0;
    public int LaneIndex
    {
        get { return _laneIndex; }
        set { _laneIndex = value; }
    }

    private bool _markedForRemoval = false;
    public bool MarkedForRemoval
    {
        get { return _markedForRemoval; }
        set { _markedForRemoval = value; }
    }
}
