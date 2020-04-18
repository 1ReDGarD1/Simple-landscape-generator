using System.Collections.Generic;
using UnityEngine;

namespace MyLandscapeGenerator.Scripts.Pool
{
    public class PrefabPool
    {
        private static readonly GameObject RootGameObject = new GameObject("Pool");
        
        private readonly Stack<GameObject> Pool = new Stack<GameObject>();
        private readonly GameObject Prefab;
        private readonly Transform ContainerTransform;

        private int _totalObjects;

        public PrefabPool(GameObject prefab, string poolName)
        {
            Prefab = prefab;

            ContainerTransform = new GameObject(poolName).transform;
            ContainerTransform.parent = RootGameObject.transform;
        }

        private void ResetPosition(Transform transform)
        {
            transform.position = new Vector3(-99999f, -99999f);
        }

        private GameObject CreateObject()
        {
            var gameObject = Object.Instantiate(Prefab, Vector3.zero, Quaternion.identity, ContainerTransform);
            gameObject.name += "_" + _totalObjects++;
            gameObject.SetActive(false);
            ResetPosition(gameObject.transform);
            return gameObject;
        }

        public void Allocate(int size)
        {
            for (var i = 0; i < size; i++)
            {
                var gameObject = CreateObject();
                Pool.Push(gameObject);
            }
        }

        public GameObject Get()
        {
            GameObject gameObject;

            if (Pool.Count > 0)
            {
                gameObject = Pool.Pop();
            }
            else
            {
                gameObject = CreateObject();
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            return gameObject;
        }

        public void Return(GameObject returnObject)
        {
            var returnObjTransform = returnObject.transform;
            
            returnObjTransform.SetParent(ContainerTransform, false);
            ResetPosition(returnObjTransform);
            
            returnObject.SetActive(false);

            Pool.Push(returnObject);
        }     
    }
}