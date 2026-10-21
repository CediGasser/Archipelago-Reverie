using System.Collections.Generic;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using WorldGeneration;

namespace Chunks
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] NoiseSettings noiseSettings;
        [SerializeField] private Material material;

        private readonly HashSet<Chunk> _chunks = new ();
        private MeshGenerator _meshGenerator;
    
        void Awake()
        {
            this._meshGenerator = new MeshGenerator(this.noiseSettings.Grid);
        }

        void OnEnable()
        {
            this._chunks.Clear();
            this._chunks.Add(this.CreateChunk(new int3(0, 0, 0)));
            this._chunks.Add(this.CreateChunk(new int3(0, 1, 0)));
        }


        private Chunk CreateChunk(int3 position)
        {
            var go = new GameObject($"Chunk {position}");
            go.transform.parent = this.transform;

            var chunk = go.AddComponent<Chunk>();
            chunk.Initialize(position, this._meshGenerator, this.noiseSettings.Grid, this.material);
            
            return chunk;
        }
    }
}
