using System.Collections.Generic;
using Unity.Mathematics;

namespace Chunks
{
    public readonly struct ChunkGrid {
        public readonly int CellsPerAxis;
        public readonly float ChunkWorldSize;
        
        public int PointsPerAxis => this.CellsPerAxis + 1;
        public int PointsPerChunk => this.PointsPerAxis *  this.PointsPerAxis * this.PointsPerAxis;
        public float CellSize => this.ChunkWorldSize / this.CellsPerAxis;

        public ChunkGrid(int cellsPerAxis, float chunkWorldSize)
        {
            this.CellsPerAxis = cellsPerAxis;
            this.ChunkWorldSize = chunkWorldSize;
        }

        public int Index(int x, int y, int z) =>
            x + y * this.PointsPerAxis + z * this.PointsPerAxis * this.PointsPerAxis;
        
        public int Index(int3 coords) =>
            coords.x + coords.y * this.PointsPerAxis + coords.z * this.PointsPerAxis * this.PointsPerAxis;

        public float3 ToWorld(int3 chunk, int3 local) =>
            (float3)chunk * this.ChunkWorldSize + (float3)local * this.CellSize;

        public float3 ToLocal(int3 xyz) =>
            (float3)xyz * this.CellSize;
        
        public IEnumerable<int3> Cells() {
            for (int z = 0; z < this.CellsPerAxis; z++)
            for (int y = 0; y < this.CellsPerAxis; y++)
            for (int x = 0; x < this.CellsPerAxis; x++)
                yield return new int3(x, y, z);
        }
        
        public IEnumerable<int3> Points() {
            for (int z = 0; z < this.PointsPerAxis; z++)
            for (int y = 0; y < this.PointsPerAxis; y++)
            for (int x = 0; x < this.PointsPerAxis; x++)
                yield return new int3(x, y, z);
        }
    }
}