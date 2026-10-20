using System;
using System.Collections.Generic;
using Chunks;
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

        public Vector3[] GetSampleCoordinates(Vector2 coords)
        {
            Vector3[] sampleCoordinates = new Vector3[this._chunkGrid.PointsPerChunk];
            
            for (int z = 0; z < this._chunkGrid.PointsPerAxis; z++)
            {
                for (int y = 0; y < this._chunkGrid.PointsPerAxis; y++)
                {
                    for (int x = 0; x < this._chunkGrid.PointsPerAxis; x++)
                    {
                        sampleCoordinates[this._chunkGrid.Index(x, y, z)] = new Vector3(coords.x + x, y, coords.y + z);
                    }
                }        
            }
            
            return sampleCoordinates;
        }

        public float DensityAt(Vector3 coords)
        {
            return this._noiseStack.SampleAt(coords);
        }

        public ChunkMeshData GenerateMeshData(Vector2 coords)
        {
            Vector3[] sampleCoordinates = GetSampleCoordinates(coords);
            float[] densityField = new float[sampleCoordinates.Length];
            for (int i = 0; i < sampleCoordinates.Length; i++) {
                densityField[i] = DensityAt(sampleCoordinates[i]);
            }

            List<Triangle> triangles = this._marchingCubes.GetTriangles(densityField, this._chunkGrid, .3f);
            
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
                
                normals[i * 3] = Vector3.Cross(vertices[i * 3 + 1] - vertices[i * 3], vertices[i * 3 + 2] - vertices[i * 3]);
                normals[i * 3 + 1] = normals[i * 3];
                normals[i * 3 + 2] = normals[i * 3];
            }
            
            var meshData = new ChunkMeshData(vertices, triangleIndices, normals);
            
            for (int t = 0; t < 5; t++)
                Debug.Log($"{vertices[t*3]} {vertices[t*3+1]} {vertices[t*3+2]}");

            return meshData;
        }
    }
}
