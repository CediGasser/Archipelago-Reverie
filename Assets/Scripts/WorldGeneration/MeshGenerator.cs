using System.Collections.Generic;
using Chunks;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;

namespace WorldGeneration
{
    public class MeshGenerator
    {
        private readonly NoiseStack _noiseStack;
        private readonly MarchingCubes _marchingCubes = new ();
        private readonly float _isoLevel;

        public ChunkGrid Grid { get; }
        public int Seed { get; }

        public MeshGenerator(GenerationSettings settings) {
            this.Grid = settings.Grid;
            this.Seed = settings.Seed ?? UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            this._noiseStack = new NoiseStack(settings.NoiseScale, this.Seed);
            this._isoLevel = settings.IsoLevel;
        }

        public float DensityAt(float3 worldPos)
        {
            return this._noiseStack.SampleAt(worldPos);
        }

        public ChunkMeshData GenerateMeshData(int3 chunkCoord)
        {
            float3 origin = this.Grid.ChunkOrigin(chunkCoord);
            float[] densityField = new float[this.Grid.PointsPerChunk];
            foreach (var pointCoord in this.Grid.Points()) {
                densityField[this.Grid.PointIndex(pointCoord)] = this.DensityAt(origin + this.Grid.PointToLocal(pointCoord));
            }

            List<Triangle> triangles = this._marchingCubes.GetTriangles(densityField, this.Grid, this._isoLevel);

            Vector3[] vertices = new Vector3[triangles.Count * 3];
            int[] triangleIndices = new int[triangles.Count * 3];
            Vector3[] normals = new Vector3[triangles.Count * 3];

            for (int i = 0; i < triangles.Count; i++)
            {
                Triangle triangle = triangles[i];
                vertices[i * 3] = triangle.VertexA;
                vertices[i * 3 + 1] = triangle.VertexB;
                vertices[i * 3 + 2] = triangle.VertexC;

                triangleIndices[i * 3] = i * 3;
                triangleIndices[i * 3 + 1] = i * 3 + 1;
                triangleIndices[i * 3 + 2] = i * 3 + 2;

                float3 normal = math.cross(triangle.VertexB - triangle.VertexA, triangle.VertexC - triangle.VertexA);
                normals[i * 3] = normal;
                normals[i * 3 + 1] = normal;
                normals[i * 3 + 2] = normal;
            }

            var meshData = new ChunkMeshData(vertices, triangleIndices, normals);

            return meshData;
        }
    }
}
