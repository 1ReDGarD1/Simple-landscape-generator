using UnityEngine;

namespace MyLandscapeGenerator.Scripts.View
{
    public sealed class MeshData
    {
        public Vector3[] Vertices { get; }
        public Vector2[] Uv { get; }

        private readonly int[] _triangles;
        private int _triangleIndex;

        public MeshData(int meshWidth, int meshHeight)
        {
            var size = meshWidth * meshHeight;

            Vertices = new Vector3[size];
            Uv = new Vector2[size];

            _triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c)
        {
            _triangles[_triangleIndex] = a;
            _triangles[_triangleIndex + 1] = b;
            _triangles[_triangleIndex + 2] = c;
            _triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            var mesh = new Mesh {vertices = Vertices, triangles = _triangles, uv = Uv};
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}