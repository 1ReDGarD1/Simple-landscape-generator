using System.Collections.Generic;
using UnityEngine;

namespace MyLandscapeGenerator.Scripts.Generator
{
    [CreateAssetMenu(menuName = "MyLandscape/ObstacleGenerator", fileName = "ObstacleGenerator")]
    public sealed class ObstacleGenerator : PerlinNoiseGenerator
    {
        [Range(0, 1)]
        [SerializeField]
        private float _grouping = 0.9f;

        public IEnumerable<Vector2Int> GenerationObstacleMapPositions()
        {
            var map = GenerationMap();

            var width = map.GetLength(0);
            var height = map.GetLength(1);

            for (var y = 0; y < width; y++)
            {
                for (var x = 0; x < height; x++)
                {
                    var value = map[x, y];
                    if (value > _grouping)
                    {
                        yield return new Vector2Int(x, y);
                    }
                }
            }
        }
    }
}