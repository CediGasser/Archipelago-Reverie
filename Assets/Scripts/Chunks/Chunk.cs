using Unity.Mathematics;
using UnityEngine;
using WorldGeneration;

namespace Chunks
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        private ChunkGrid _chunkGrid;
        private MeshGenerator _meshGenerator;
        private int3 _position;
        
        public void Initialize(int3 chunkCoords, MeshGenerator generator, ChunkGrid chunkGrid, Material material)
        {
            this._meshGenerator = generator;
            this._chunkGrid = chunkGrid;
            this._position = chunkCoords;
            this.transform.position = this._chunkGrid.ToWorld(chunkCoords, new int3(0, 0, 0));

            Mesh mesh = this.CreateMesh(chunkCoords, generator);
            this.GetComponent<MeshFilter>().mesh = mesh;
            
            this.GetComponent<MeshRenderer>().material = material;
        }

        private Mesh CreateMesh(int3 chunkCoords, MeshGenerator generator)
        {
            ChunkMeshData meshData = generator.GenerateMeshData(chunkCoords);

            Mesh newMesh = new Mesh();
            newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            newMesh.SetVertices(meshData.Vertices);
            newMesh.SetTriangles(meshData.Triangles, 0);
            newMesh.SetNormals(meshData.Normals);
            
            return newMesh;
        }

        private void OnDrawGizmos()
        {
            foreach (var point in this._chunkGrid.Points())
            {   
                float density = this._meshGenerator.DensityAt(this._chunkGrid.ToWorld(this._position, point)); 
                
                Gizmos.color = new Color(density, density, density, 1.0f);
                Gizmos.DrawSphere(this._chunkGrid.ToWorld(this._position, point), 0.1f);
            }
        }
    }
}
