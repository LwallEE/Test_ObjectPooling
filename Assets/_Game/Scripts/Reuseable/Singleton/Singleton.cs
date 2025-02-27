using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Generic Singleton for reuse
public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    public static T Instance {get; private set;}

    protected virtual void Awake()
    {
        if (Instance == null) //if current Instance is not initialize
        {
            Instance = this as T; //Set Instance
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
