using System.Collections.Generic;
using System.Diagnostics;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using WorldGeneration;
using Debug = UnityEngine.Debug;

namespace Chunks
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] GenerationSettings generationSettings;
        [SerializeField] Material material;

        private readonly HashSet<Chunk> _chunks = new ();
        private MeshGenerator _meshGenerator;

        void Awake()
        {
            this._meshGenerator = new MeshGenerator(this.generationSettings);
        }

        void Start()
        {
            var stopwatch = Stopwatch.StartNew();

            this._chunks.Add(this.CreateChunk(new int3(0, 0, 0)));
            this._chunks.Add(this.CreateChunk(new int3(0, 1, 0)));

            Debug.Log($"Generated {this._chunks.Count} chunks " +
                      $"in {stopwatch.Elapsed.TotalMilliseconds:F1} ms, seed {this._meshGenerator.Seed}");
        }


        private Chunk CreateChunk(int3 chunkCoord)
        {
            var go = new GameObject($"Chunk {chunkCoord}");
            go.transform.SetParent(this.transform, false);

            var chunk = go.AddComponent<Chunk>();
            chunk.Initialize(chunkCoord, this._meshGenerator, this.material);

            return chunk;
        }
    }
}
