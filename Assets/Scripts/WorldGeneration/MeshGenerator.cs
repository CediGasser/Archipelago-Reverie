using UnityEngine;

namespace WorldGeneration
{
    public class MeshGenerator
    {
        public Mesh GenerateMesh(Vector2 coords)
        {
            int quadsPerSide = 10;
            int verticesPerSide = quadsPerSide + 1;
            
            Vector3[] vertices = new Vector3[verticesPerSide * verticesPerSide];
            for (int x = 0; x < verticesPerSide; x++)
            {
                for (int z = 0; z < verticesPerSide; z++)
                {
                    vertices[x * verticesPerSide + z] = new Vector3(coords.x + x, 0, coords.y + z);
                }        
            }

            int[] triangles = new int[2*3*quadsPerSide*quadsPerSide];

            for (int x = 0; x < quadsPerSide; x++)
            {
                for (int z = 0; z < quadsPerSide; z++)
                {
                    // First triangle of the quad
                    triangles[(x * quadsPerSide + z) * 6 + 0] = x * verticesPerSide + z;
                    triangles[(x * quadsPerSide + z) * 6 + 1] = (x * verticesPerSide + verticesPerSide) + z + 1;
                    triangles[(x * quadsPerSide + z) * 6 + 2] = (x * verticesPerSide + verticesPerSide) + z;
                
                    // Second triangle of the quad
                    triangles[(x * quadsPerSide + z) * 6 + 3] = x * verticesPerSide + z;
                    triangles[(x * quadsPerSide + z) * 6 + 4] = (x * verticesPerSide) + z + 1;
                    triangles[(x * quadsPerSide + z) * 6 + 5] = (x * verticesPerSide + verticesPerSide) + z + 1;
                }
            }

            Vector3[] normals = new Vector3[verticesPerSide * verticesPerSide];

            for (int i = 0; i < verticesPerSide * verticesPerSide; i++)
            {
                normals[i] = Vector3.up;
            }

            Mesh newMesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles,
                normals = normals
            };

            return newMesh;
        }
    }
}
