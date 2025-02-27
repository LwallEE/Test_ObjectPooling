using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{

    private Transform tf;

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

    private EPoolType poolTypeKey;

    public EPoolType GetPoolTypeKey()
    {
        return poolTypeKey;
    }

    public void SetPoolTypeKey(EPoolType poolType)
    {
        poolTypeKey = poolType;
    }

    protected virtual void Despawn()
    {
        ObjectPooling.Instance.Despawn(this);
    }

    public virtual void Respawn()
    {
        
    }
}
