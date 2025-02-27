using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReuseSystem.ObjectPooling
{
    [Serializable]
    struct GameObjectPool
    {
        public GameObject objectToSpawn;
        public int numberToSpawn;
    }

    public class VfxPool : Singleton<VfxPool>
    {
        [SerializeField] private List<GameObjectPool> listGameObjectToPreload;
        private Dictionary<GameObject, List<GameObject>> _poolObj = new Dictionary<GameObject, List<GameObject>>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            PreLoadObject();
        }

        private void PreLoadObject()
        {
            if (listGameObjectToPreload == null) return;
            foreach (var obj in listGameObjectToPreload)
            {
                if (_poolObj.ContainsKey(obj.objectToSpawn)) continue;
                _poolObj.Add(obj.objectToSpawn, new List<GameObject>());
                for (int i = 0; i < obj.numberToSpawn; i++)
                {
                    GameObject gameObjInstance = Instantiate(obj.objectToSpawn, transform);
                    _poolObj[obj.objectToSpawn].Add(gameObjInstance);
                    gameObjInstance.SetActive(false);
                }
            }
        }

        public GameObject GetObj(GameObject objKey)
        {
            if (!_poolObj.ContainsKey(objKey))
            {
                _poolObj.Add(objKey, new List<GameObject>());
            }

            foreach (GameObject g in _poolObj[objKey])
            {
                if (g.activeSelf || g.transform.parent != transform)
                    continue;
                g.SetActive(true);
                return g;
            }

            GameObject g2 = Instantiate(objKey, transform);
            _poolObj[objKey].Add(g2);
            return g2;

        }

        public T GetObj<T>(GameObject objKey) where T : Component
        {
            return this.GetObj(objKey).GetComponent<T>();
        }

        public void AddObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
        }

       
    }
  
}