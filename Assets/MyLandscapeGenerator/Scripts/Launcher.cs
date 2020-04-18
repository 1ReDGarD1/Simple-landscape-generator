using System;
using System.Collections.Generic;
using MyLandscapeGenerator.Scripts.Generator;
using MyLandscapeGenerator.Scripts.Pool;
using MyLandscapeGenerator.Scripts.View;
using UnityEngine;

namespace MyLandscapeGenerator.Scripts
{
    public sealed class Launcher : MonoBehaviour
    {
        [Serializable]
        class ObstacleRecord
        {
            public PrefabPoolSetting _prefabPoolSetting;
            public ObstacleGenerator _generator;
        }

        [SerializeField]
        private WorldView _worldView;

        [SerializeField]
        private BaseGenerator _landscapeGenerator;

        [SerializeField]
        private ObstacleRecord[] _obstacleRecords;

        private void Awake()
        {
            foreach (var obstacleRecord in _obstacleRecords)
            {
                obstacleRecord._prefabPoolSetting.Init();
            }
        }

        private void Start()
        {
            Generation();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 100), "Generation"))
            {
                Clear();
                Generation();
            }
        }

        private void Generation()
        {
            var map = _landscapeGenerator.GenerationMap();
            var obstacleDatas = new List<ObstacleData>();

            foreach (var obstacleRecord in _obstacleRecords)
            {
                var mapPositions = obstacleRecord._generator.GenerationObstacleMapPositions();
                foreach (var mapPosition in mapPositions)
                {
                    var obstacleData = new ObstacleData(mapPosition.x, mapPosition.y, obstacleRecord._prefabPoolSetting);
                    obstacleDatas.Add(obstacleData);
                }
            }

            _worldView.Display(map, obstacleDatas);
        }

        private void Clear()
        {
            foreach (var obstacleRecord in _obstacleRecords)
            {
                obstacleRecord._prefabPoolSetting.Clear();
            }
        }
    }
}