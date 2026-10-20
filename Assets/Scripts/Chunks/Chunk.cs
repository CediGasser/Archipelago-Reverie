using UnityEngine;
using WorldGeneration;

namespace Chunks
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        private ChunkGrid _chunkGrid;
        private MeshGenerator _meshGenerator;
        public void Initialize(Vector3 coords, MeshGenerator generator, Material material)
        {
            this._meshGenerator = generator;
            this.transform.position = coords;

            Mesh mesh = this.CreateMesh(coords, generator);
            this.GetComponent<MeshFilter>().mesh = mesh;
            
            this.GetComponent<MeshRenderer>().material = material;
        }

        private Mesh CreateMesh(Vector3 coords, MeshGenerator generator)
        {
            ChunkMeshData meshData = generator.GenerateMeshData(coords);

            Mesh newMesh = new Mesh();
            newMesh.SetVertices(meshData.Vertices);
            newMesh.SetTriangles(meshData.Triangles, 0);
            newMesh.SetNormals(meshData.Normals);
            
            return newMesh;
        }

        private void OnDrawGizmos()
        {
            var points = this._meshGenerator.GetSampleCoordinates(this.transform.position);

            foreach (var point in points)
            {   
                float densityNormalized = this._meshGenerator.DensityAt(point); 
                
                Gizmos.color = new Color(densityNormalized, densityNormalized, densityNormalized, 1.0f);
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
