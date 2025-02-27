using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
struct GameObjectPool
{
    public GameObject objectToSpawn;
    public int numberToSpawn;
}

public class
    VfxPool : Singleton<VfxPool> //Pooling for vfx, no need for retrieve specific class, so use GameObject for less setup requirement
{
    [SerializeField] private List<GameObjectPool> listGameObjectToPreload;
    
    //save Pool by Prefab of GameObject as key and value is list of inactive and active GameObject
    private Dictionary<GameObject, List<GameObject>> _poolObj = new Dictionary<GameObject, List<GameObject>>(); 

    protected override void Awake()
    {
        base.Awake();
        PreLoadObject();
    }

    private void PreLoadObject() 
    {
        if (listGameObjectToPreload == null) return;
        foreach (var obj in listGameObjectToPreload)
        {
            if (_poolObj.ContainsKey(obj.objectToSpawn)) continue;
            List<GameObject> preloadList = new List<GameObject>();
            for (int i = 0; i < obj.numberToSpawn; i++) //initialize preload list
            {
                GameObject gameObjInstance = Instantiate(obj.objectToSpawn, transform);
                preloadList.Add(gameObjInstance);
                gameObjInstance.SetActive(false);
            }
            _poolObj.Add(obj.objectToSpawn,preloadList);
        }
    }

    public GameObject GetObj(GameObject objKey) //method for retrieve object in pool, key is prefab gameobject
    {
        if (!_poolObj.ContainsKey(objKey)) //if prefab isn't in Pool, create new one
        {
            _poolObj.Add(objKey, new List<GameObject>());
        }

        foreach (GameObject g in _poolObj[objKey]) //retrieve the first inactive object in list
        {
            if (g.activeSelf || g.transform.parent != transform)
                continue;
            g.SetActive(true);
            return g;
        }

        GameObject g2 = Instantiate(objKey, transform); //if all objects is active, create new one
        _poolObj[objKey].Add(g2);
        return g2;
    }

    public T GetObj<T>(GameObject objKey) where T : Component //for wanting retrieve the component in object
    {                                                         //not recommend for use because of GetComponent, using ObjectPooling instead
        return this.GetObj(objKey).GetComponent<T>();
    }

    public void AddObjectToPool(GameObject obj) //Simply DeActive the gameobject if want for push object back to pool
    {                                           //effective for particle, by set stop action to disable
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }

    public void ReleaseAll()
    {
        foreach (var pool in _poolObj.Values)
        {
            foreach (var obj in pool)
            {
                Destroy(obj);
            }
        }
    }
}