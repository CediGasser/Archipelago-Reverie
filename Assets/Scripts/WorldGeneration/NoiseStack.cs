using Unity.Mathematics;
using UnityEngine;

namespace WorldGeneration
{
    public class NoiseStack
    {
        private readonly float _seed = Time.time;
        private readonly float _scale = 10.0f;

        public float SampleAt(Vector3 position)
        {
            position /= this._scale;
            position += new Vector3(this._seed, this._seed, this._seed);
            
            float3 samplePosition = new float3(position.x, position.y, position.z);
            return noise.snoise(samplePosition);
        }    
    }
}
