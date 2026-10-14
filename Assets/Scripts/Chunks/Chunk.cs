using UnityEngine;
using WorldGeneration;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    private MeshGenerator _meshGenerator;
    
    public void Initialize(Vector3 coords, MeshGenerator generator)
    {
        this._meshGenerator = generator;
        this.transform.position = coords;

        var mesh = this._meshGenerator.GenerateMesh(coords);
        
        this.GetComponent<MeshFilter>().mesh = mesh;
    }
}
