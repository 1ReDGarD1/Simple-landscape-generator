using System.Collections.Generic;
using UnityEngine;

namespace MyLandscapeGenerator.Scripts.Pool
{
    [CreateAssetMenu(menuName = "MyLandscape/PrefabPoolSetting", fileName = "PrefabPoolSetting")]
    public class PrefabPoolSetting : ScriptableObject
    {
        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private int _allocateSize;

        private Stack<GameObject> _returnedObjects;
        private PrefabPool _prefabPool;

        public void Init()
        {
            _returnedObjects = new Stack<GameObject>();
            _prefabPool = new PrefabPool(_prefab, _prefab.name);
            _prefabPool.Allocate(_allocateSize);
        }

        public GameObject Get()
        {
            var gameObject = _prefabPool.Get();
            _returnedObjects.Push(gameObject);
            return gameObject;
        }

        public void Clear()
        {
            foreach (var returnedObject in _returnedObjects)
            {
                _prefabPool.Return(returnedObject);
            }

            _returnedObjects.Clear();
        }
    }
}