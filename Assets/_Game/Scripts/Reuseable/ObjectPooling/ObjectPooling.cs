using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PoolAmount
{
    public EPoolType poolType;
    public GameUnit prefab;
    public int amount;
}
public class ObjectPooling : Singleton<ObjectPooling>
{
    private Dictionary<EPoolType, Pool> poolInstance = new Dictionary<EPoolType, Pool>();
    [SerializeField] private PoolAmount[] poolAmounts;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            Preload(poolAmounts[i].poolType, poolAmounts[i].prefab, poolAmounts[i].amount);
        }
    }

    public void Preload(EPoolType type, GameUnit prefab, int amount, Transform parent=null)
    {
        if (prefab == null)
        {
            Debug.LogError(" PREFAB IN POOL IS EMPTY");
            return;
        }
        
        if (!poolInstance.ContainsKey(type) || poolInstance[type] == null)
        {
            if (parent == null)
            {
                parent = new GameObject(prefab.gameObject.name).transform;
                parent.transform.position = Vector3.zero;
                parent.SetParent(transform);
            }
            Pool p = new Pool();
            p.PreLoad(type, prefab, amount, parent);
            poolInstance[type] = p;
        }
        
    }

    public T Spawn<T>(EPoolType typeKey, Vector3 pos = default, Quaternion rot = default) where T: GameUnit
    {
        if (!poolInstance.ContainsKey(typeKey))
        {
            Debug.LogError($" PREFAB WITH TYPE {typeKey} IS NOT INITIALIZE");
        }

        return poolInstance[typeKey].Spawn(pos, rot) as T;
    }

    //Take the object to pool
    public void Despawn(GameUnit unit)
    {
        var key = unit.GetPoolTypeKey();
        if (!poolInstance.ContainsKey(key))
        {
            Debug.LogError($"{key} IS NOT PRELOAD");
        }
        poolInstance[key].Despawn(unit);
    }

    //Disable all object of type GameUnit
    public void Collect(EPoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError($"{poolType} IS NOT PRELOAD");
            return;
        }
        poolInstance[poolType].Collect();
    }

    //Disable all pool
    public void CollectAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Collect();
        }
    }

    //Destroy all object of PoolType
    public void Release(EPoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError($"{poolType} IS NOT PRELOAD");
            return;
        }
        poolInstance[poolType].Release();
    }

    public void ReleaseAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Release();
        }
    }
    
}

public class Pool
{
    private Transform parent;
    private GameUnit prefab;

    private EPoolType poolTypeKey;
    //list contain unit is not using
    private Queue<GameUnit> inactives = new Queue<GameUnit>();
    //list contain unit is using
    private List<GameUnit> actives = new List<GameUnit>();

    //init pool
    public void PreLoad(EPoolType poolTypeKey, GameUnit prefab, int amount, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.poolTypeKey = poolTypeKey;
        //Debug.Log(amount);
        for (int i = 0; i < amount; i++)
        {
            Spawn(Vector3.zero, Quaternion.identity);
        }
        Collect();
    }

    //get element from pool
    public GameUnit Spawn(Vector3 pos, Quaternion rot )
    {
        GameUnit unit;
        if (inactives.Count <= 0)
        {
            unit = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }
        
        unit.Tf.SetLocalPositionAndRotation(pos, rot);
        unit.SetPoolTypeKey(poolTypeKey);
        unit.gameObject.SetActive(true);
        unit.Respawn();
        actives.Add(unit);
        return unit;
    }
    
    
    //return element to pool
    public void Despawn(GameUnit unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            actives.Remove(unit);
            inactives.Enqueue(unit);
            unit.gameObject.SetActive(false);
            unit.Tf.SetParent(parent);
        }
    }

    //return all used element to pool
    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    //destroy all element in pool
    public void Release()
    {
        Collect();
        while(inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }
}