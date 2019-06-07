using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public PoolObject prefab;
    public int initialElements = 30;

    private Queue<PoolObject> idleElements;

    void Awake()
    {
        idleElements = new Queue<PoolObject>();
        for(int i = 0; i < initialElements; ++i)
        {
            Spawn();
        }
    }

    public GameObject Get()
    {
        if (idleElements.Count == 0) Spawn();
        GameObject obj = idleElements.Dequeue().gameObject;
        obj.SetActive(true);
        return obj;
    }

    public void Deactivate(PoolObject obj)
    {
        idleElements.Enqueue(obj);
    }

    private void Spawn()
    {
        PoolObject obj = Instantiate<PoolObject>(prefab, transform);
        obj.gameObject.SetActive(false);
        idleElements.Enqueue(obj);
        obj.Pool = this;
    }
}
