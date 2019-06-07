using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    Pool Pool
    {
        get;
        set;
    }

    void Deactivate();
}
