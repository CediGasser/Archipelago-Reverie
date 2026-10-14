using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration
{
    public class MeshGenerator
    {
        private NoiseStack _noiseStack = new();
        private readonly int _quadsPerSide = 10;

        public List<Vector3> GetSampleCoordinates(Vector2 coords)
        {
            List<Vector3> sampleCoordinates = new();
            
            for (int x = 0; x < this._quadsPerSide + 1; x++)
            {
                for (int z = 0; z < this._quadsPerSide + 1; z++)
                {
                    for (int y = 0; y < this._quadsPerSide + 1; y++)
                    {
                        sampleCoordinates.Add(new Vector3(coords.x + x, y, coords.y + z));
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
            int verticesPerSide = this._quadsPerSide + 1;
            
            Vector3[] vertices = new Vector3[verticesPerSide * verticesPerSide];
            for (int x = 0; x < verticesPerSide; x++)
            {
                for (int z = 0; z < verticesPerSide; z++)
                {
                    vertices[x * verticesPerSide + z] = new Vector3(coords.x + x, 0, coords.y + z);
                }        
            }

            int[] triangles = new int[2*3*this._quadsPerSide *this._quadsPerSide ];

            for (int x = 0; x < this._quadsPerSide ; x++)
            {
                for (int z = 0; z < this._quadsPerSide ; z++)
                {
                    // First triangle of the quad
                    triangles[(x * this._quadsPerSide + z) * 6 + 0] = x * verticesPerSide + z;
                    triangles[(x * this._quadsPerSide + z) * 6 + 1] = (x * verticesPerSide + verticesPerSide) + z + 1;
                    triangles[(x * this._quadsPerSide + z) * 6 + 2] = (x * verticesPerSide + verticesPerSide) + z;
                
                    // Second triangle of the quad
                    triangles[(x * this._quadsPerSide + z) * 6 + 3] = x * verticesPerSide + z;
                    triangles[(x * this._quadsPerSide + z) * 6 + 4] = (x * verticesPerSide) + z + 1;
                    triangles[(x * this._quadsPerSide + z) * 6 + 5] = (x * verticesPerSide + verticesPerSide) + z + 1;
                }
            }

            Vector3[] normals = new Vector3[verticesPerSide * verticesPerSide];

            for (int i = 0; i < verticesPerSide * verticesPerSide; i++)
            {
                normals[i] = Vector3.up;
            }

            var meshData = new ChunkMeshData(vertices, triangles, normals);

            return meshData;
        }
    }
}
