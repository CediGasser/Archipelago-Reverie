using Unity.Mathematics;

namespace Chunks
{
    public readonly struct ChunkGrid {
        public readonly int CellsPerAxis;   // e.g. 32
        public readonly float CellSize;    // world units per cell
        
        public int PointsPerAxis => this.CellsPerAxis + 1;
        public int PointsPerChunk => this.PointsPerAxis *  this.PointsPerAxis * this.PointsPerAxis;
        public float ChunkWorldSize => this.CellsPerAxis * this.CellSize;

        public ChunkGrid(int cellsPerAxis, float voxelSize)
        {
            this.CellsPerAxis = cellsPerAxis;
            this.CellSize = voxelSize;
        }

        public int Index(int x, int y, int z) =>
            x + y * this.PointsPerAxis + z * this.PointsPerAxis * this.PointsPerAxis;
        
        public int Index(int3 coords) =>
            coords.x + coords.y * this.PointsPerAxis + coords.z * this.PointsPerAxis * this.PointsPerAxis;

        public float3 ToWorld(int3 chunk, int3 local) =>
            (float3)chunk * this.ChunkWorldSize + (float3)local * this.CellSize;
    }
}