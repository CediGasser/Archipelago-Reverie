using UnityEngine;

namespace WorldGeneration
{
    public class ChunkMeshData
    {
        public Vector3[] Vertices { get; private set; }
        public int[] Triangles { get; private set; }
        public Vector3[] Normals { get; private set; }
    
        public ChunkMeshData(Vector3[] vertices, int[] triangles, Vector3[] normals) {
            this.Vertices = vertices;
            this.Triangles = triangles;
            this.Normals = normals;
        }
    }
}
