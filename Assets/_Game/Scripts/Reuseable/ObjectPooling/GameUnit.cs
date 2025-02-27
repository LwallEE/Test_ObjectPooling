using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour  //Base class for object in Pool, use for retrieve class by type casting instead of GetComponent
{

    private Transform tf; //cache transform

    public Transform Tf
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }

            return tf;
        }
    }

    private EPoolType poolTypeKey; //poolType using for Despawn method

    public EPoolType GetPoolTypeKey()
    {
        return poolTypeKey;
    }

    public void SetPoolTypeKey(EPoolType poolType)
    {
        poolTypeKey = poolType;
    }

    protected virtual void Despawn() //Method push object back to pool
    {
        ObjectPooling.Instance.Despawn(this);
    }

    public virtual void Respawn() //Method call when object is taking from pool
    {
        
    }
}
