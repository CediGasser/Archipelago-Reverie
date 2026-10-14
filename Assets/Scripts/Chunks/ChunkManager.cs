using System.Collections.Generic;
using UnityEngine;
using WorldGeneration;

namespace Chunks
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private NoiseSettings noiseSettings;

        private readonly HashSet<Chunk> _chunks = new ();
        private MeshGenerator _meshGenerator;
    
        void Awake()
        {
            this._meshGenerator = new MeshGenerator();
        }

        void OnEnable()
        {
            this._chunks.Clear();
            this._chunks.Add(this.CreateChunk(new Vector3(0, 0, 0)));
        }


        private Chunk CreateChunk(Vector3 position)
        {
            var go = new GameObject($"Chunk {position}");
            go.transform.parent = transform;
            go.transform.localPosition = position;

            var chunk = go.AddComponent<Chunk>();
            chunk.Initialize(position, this._meshGenerator);
            
            return chunk;
        }
    }
}
