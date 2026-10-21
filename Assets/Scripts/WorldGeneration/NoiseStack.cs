using Unity.Mathematics;

namespace WorldGeneration
{
    public class NoiseStack
    {
        private readonly float3 _offset;
        private readonly float _scale;

        public NoiseStack(float scale, int seed)
        { 
            this._scale = scale;
            // Kept moderate so snoise doesn't lose float precision far from the origin
            this._offset = Random.CreateFromIndex((uint)seed).NextFloat3(-1000f, 1000f);
        }

        public float SampleAt(float3 worldPos)
        {
            return (noise.snoise(worldPos / this._scale + this._offset) + 1) / 2;
        }
    }
}
