using Chunks;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NoiseSettings", menuName = "Scriptable Objects/NoiseSettings")]
    public class NoiseSettings : ScriptableObject
    {
        [SerializeField] int cellsPerAxis = 32;
        [SerializeField] float voxelSize = 1f;
        public ChunkGrid Grid => new(this.cellsPerAxis, this.voxelSize);
    }
}
