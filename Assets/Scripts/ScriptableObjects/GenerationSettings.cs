using Chunks;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GenerationSettings", menuName = "Scriptable Objects/GenerationSettings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("Grid")]
        [SerializeField] private int cellsPerAxis = 32;
        [SerializeField] private int chunkSize = 10;

        [Header("Noise")]
        [SerializeField] private bool useRandomSeed = true;
        [SerializeField] private int seed;
        [SerializeField] private float noiseScale = 5f;
        [SerializeField] private float isoLevel = 0.3f;

        public ChunkGrid Grid => new(this.cellsPerAxis, this.chunkSize);
        // Unity can't serialize int?, so the toggle stands in for null
        public int? Seed => this.useRandomSeed ? null : this.seed;
        public float NoiseScale => this.noiseScale;
        public float IsoLevel => this.isoLevel;

        private void OnValidate()
        {
            this.cellsPerAxis = Mathf.Clamp(this.cellsPerAxis, 1, 64);
            this.chunkSize = Mathf.Max(1, this.chunkSize);
            this.noiseScale = Mathf.Max(0.01f, this.noiseScale);
            this.isoLevel = Mathf.Clamp01(this.isoLevel);
        }
    }
}
