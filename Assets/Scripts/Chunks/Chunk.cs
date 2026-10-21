using Unity.Mathematics;
using UnityEngine;
using WorldGeneration;

namespace Chunks
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        private MeshGenerator _meshGenerator;
        private int3 _chunkCoord;
        private Mesh _mesh;

        public void Initialize(int3 chunkCoord, MeshGenerator generator, Material material)
        {
            this._meshGenerator = generator;
            this._chunkCoord = chunkCoord;
            this.transform.localPosition = generator.Grid.ChunkOrigin(chunkCoord);

            this._mesh = this.CreateMesh(chunkCoord, generator);
            this.GetComponent<MeshFilter>().sharedMesh = this._mesh;

            this.GetComponent<MeshRenderer>().material = material;
        }

        private Mesh CreateMesh(int3 chunkCoord, MeshGenerator generator)
        {
            ChunkMeshData meshData = generator.GenerateMeshData(chunkCoord);

            Mesh newMesh = new Mesh();
            newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            newMesh.SetVertices(meshData.Vertices);
            newMesh.SetTriangles(meshData.Triangles, 0);
            newMesh.SetNormals(meshData.Normals);

            return newMesh;
        }

        private void OnDestroy()
        {
            if (this._mesh != null) Destroy(this._mesh);
        }

        private void OnDrawGizmos()
        {
            ChunkGrid grid = this._meshGenerator.Grid;
            float3 origin = grid.ChunkOrigin(this._chunkCoord);
            Gizmos.matrix = this.transform.localToWorldMatrix;

            foreach (var pointCoord in grid.Points())
            {
                float3 localPos = grid.PointToLocal(pointCoord);
                float density = this._meshGenerator.DensityAt(origin + localPos);

                Gizmos.color = new Color(density, density, density, 1.0f);
                Gizmos.DrawSphere(localPos, 0.1f);
            }
        }
    }
}
