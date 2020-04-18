using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MyLandscapeGenerator.Scripts.View
{
    public sealed class WorldView : MonoBehaviour
    {
        [Serializable]
        public struct RegionRecord
        {
            public float height;
            public Color colour;
        }

        [SerializeField]
        public RegionRecord[] _regions;

        [SerializeField]
        public float _meshHeightMultiplier;

        [SerializeField]
        public AnimationCurve _meshHeightCurve;

        [SerializeField]
        public MeshFilter _meshFilter;

        [SerializeField]
        public MeshRenderer _meshRenderer;

        private void Awake()
        {
            Assert.IsTrue(_regions.Length > 0);
            Assert.IsTrue(_meshHeightCurve != null);
            Assert.IsTrue(_meshFilter != null);
            Assert.IsTrue(_meshRenderer != null);
        }

        public void Display(float[,] map, ICollection<ObstacleData> obstacleDatas)
        {
            var terrainMesh = GenerateTerrainMesh(map);
            var colourMap = CreateColourMap(map);
            DrawWorldMesh(terrainMesh, colourMap);

            DisplayObstacles(map, obstacleDatas);
        }

        private void DisplayObstacles(float[,] map, ICollection<ObstacleData> obstacleDatas)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var topLeftX = (width - 1) / -2f;
            var topLeftZ = (height - 1) / 2f;

            foreach (var obstacleData in obstacleDatas)
            {
                var x = obstacleData.MapX;
                var y = obstacleData.MapY;

                var vertexX = topLeftX + x;
                var vertexY = _meshHeightCurve.Evaluate(map[x, y]) * _meshHeightMultiplier;
                var vertexZ = topLeftZ - y;
                
                var obstacle = obstacleData.PrefabPoolSetting.Get();
                var positionByVertex = new Vector3(vertexX, vertexY, vertexZ);

                obstacle.transform.position = positionByVertex;
            }
        }

        private void DrawWorldMesh(MeshData meshData, Texture2D texture)
        {
            _meshFilter.sharedMesh = meshData.CreateMesh();
            _meshRenderer.sharedMaterial.mainTexture = texture;
        }

        private Texture2D CreateColourMap(float[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var colourMap = new Color[width * height];
            for (var y = 0; y < width; y++)
            {
                for (var x = 0; x < height; x++)
                {
                    var currentHeight = map[x, y];
                    foreach (var region in _regions)
                    {
                        if (currentHeight <= region.height)
                        {
                            colourMap[y * width + x] = region.colour;
                            break;
                        }
                    }
                }
            }

            return TextureFromColourMap(colourMap, width, height);
        }

        private Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
        {
            var texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colourMap);
            texture.Apply();
            return texture;
        }

        private MeshData GenerateTerrainMesh(float[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var topLeftX = (width - 1) / -2f;
            var topLeftZ = (height - 1) / 2f;

            var meshData = new MeshData(width, width);
            var vertexIndex = 0;

            for (var y = 0; y < height; y += 1)
            {
                for (var x = 0; x < width; x += 1)
                {
                    var vertexX = topLeftX + x;
                    var vertexY = _meshHeightCurve.Evaluate(map[x, y]) * _meshHeightMultiplier;
                    var vertexZ = topLeftZ - y;
                    meshData.Vertices[vertexIndex] = new Vector3(vertexX, vertexY, vertexZ);

                    meshData.Uv[vertexIndex] = new Vector2(x / (float) width, y / (float) height);

                    if (x < width - 1 && y < height - 1)
                    {
                        var incrementVertex = vertexIndex + width + 1;
                        meshData.AddTriangle(vertexIndex, incrementVertex, vertexIndex + width);
                        meshData.AddTriangle(incrementVertex, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }

            return meshData;
        }
    }
}