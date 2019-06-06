using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject
{
    private Pool _pool;
    public Pool Pool
    {
        get { return _pool; }
        set { _pool = value; }
    }

    public void Deactivate()
    {
        Pool.Deactivate(this);
        gameObject.SetActive(false);
    }
}
