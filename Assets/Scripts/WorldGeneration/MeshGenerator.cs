using System;
using System.Collections.Generic;
using Chunks;
using Unity.Mathematics;
using UnityEngine;

namespace WorldGeneration
{
    public class MeshGenerator
    {
        private NoiseStack _noiseStack = new();
        private MarchingCubes _marchingCubes = new ();
        private readonly ChunkGrid _chunkGrid;

        public MeshGenerator (ChunkGrid chunkGrid) {
            this._chunkGrid = chunkGrid;
        }

        public float DensityAt(Vector3 coords)
        {
            return this._noiseStack.SampleAt(coords);
        }

        public ChunkMeshData GenerateMeshData(int3 coords)
        {
            float[] densityField = new float[this._chunkGrid.PointsPerChunk];
            int i = 0;
            foreach (var point in this._chunkGrid.Points()) {
                densityField[i] = this.DensityAt(this._chunkGrid.ToWorld(coords, point));
                i++;
            }
            
            Debug.Log($"{this._chunkGrid.PointsPerChunk} points per chunk");

            List<Triangle> triangles = this._marchingCubes.GetTriangles(densityField, this._chunkGrid, .3f);
            
            Debug.Log($"{triangles.Count} triangles");
            
            Vector3[] vertices = new Vector3[triangles.Count * 3];
            int[] triangleIndices = new int[triangles.Count * 3];
            Vector3[] normals = new Vector3[triangles.Count * 3];
            
            for (i = 0; i < triangles.Count; i++)
            {
                Triangle triangle = triangles[i];
                vertices[i * 3] = triangle.VertexA;
                vertices[i * 3 + 1] = triangle.VertexB;
                vertices[i * 3 + 2] = triangle.VertexC;
                
                triangleIndices[i * 3] = i * 3;
                triangleIndices[i * 3 + 1] = i * 3 + 1;
                triangleIndices[i * 3 + 2] = i * 3 + 2;
                
                normals[i * 3] = Vector3.Cross(vertices[i * 3 + 1] - vertices[i * 3], vertices[i * 3 + 2] - vertices[i * 3]);
                normals[i * 3 + 1] = normals[i * 3];
                normals[i * 3 + 2] = normals[i * 3];
            }
            
            var meshData = new ChunkMeshData(vertices, triangleIndices, normals);

            return meshData;
        }
    }
}
