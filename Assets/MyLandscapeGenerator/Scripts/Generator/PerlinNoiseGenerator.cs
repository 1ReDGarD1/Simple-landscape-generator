using UnityEngine;
using Random = System.Random;

namespace MyLandscapeGenerator.Scripts.Generator
{
    [CreateAssetMenu(menuName = "MyLandscape/PerlinNoiseGenerator", fileName = "PerlinNoiseGenerator")]
    public class PerlinNoiseGenerator : BaseGenerator
    {
        [SerializeField]
        private int _mapWidth;

        [SerializeField]
        private int _mapHeight;

        [SerializeField]
        private float _noiseScale;

        [Range(0, 100)]
        [SerializeField]
        private int _octaves;

        [Range(0, 1)]
        [SerializeField]
        public float _persistance;

        [Range(0, 100)]
        [SerializeField]
        public float _lacunarity;

        private Vector2[] CreateOctaveOffsets()
        {
            var random = new Random();
            var octaveOffsets = new Vector2[_octaves];
            for (var i = 0; i < _octaves; i++)
            {
                var offsetX = RandomNext();
                var offsetY = RandomNext();
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            int RandomNext() => random.Next(-100000, 100000);

            return octaveOffsets;
        }

        public override float[,] GenerationMap()
        {
            if (_noiseScale <= 0)
            {
                _noiseScale = 0.0001f;
            }

            var maxNoiseHeight = float.MinValue;
            var minNoiseHeight = float.MaxValue;

            var halfWidth = _mapWidth / 2f;
            var halfHeight = _mapHeight / 2f;

            var noiseMap = new float[_mapWidth, _mapHeight];
            var octaveOffsets = CreateOctaveOffsets();

            for (var y = 0; y < _mapHeight; y++)
            {
                for (var x = 0; x < _mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (var i = 0; i < _octaves; i++)
                    {
                        var octaveOffset = octaveOffsets[i];
                        var sampleX = (x - halfWidth) / _noiseScale * frequency + octaveOffset.x;
                        var sampleY = (y - halfHeight) / _noiseScale * frequency + octaveOffset.y;

                        var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= _persistance;
                        frequency *= _lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (var y = 0; y < _mapHeight; y++)
            {
                for (var x = 0; x < _mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}