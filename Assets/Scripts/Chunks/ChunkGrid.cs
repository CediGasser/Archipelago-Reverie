using System.Collections.Generic;
using Unity.Mathematics;

namespace Chunks
{
    public readonly struct ChunkGrid {
        public readonly int CellsPerAxis;
        public readonly int ChunkSize;

        public int PointsPerAxis => this.CellsPerAxis + 1;
        public int PointsPerChunk => this.PointsPerAxis *  this.PointsPerAxis * this.PointsPerAxis;
        public float CellSize => (float)this.ChunkSize / this.CellsPerAxis;

        public ChunkGrid(int cellsPerAxis, int chunkSize)
        {
            this.CellsPerAxis = cellsPerAxis;
            this.ChunkSize = chunkSize;
        }

        public int PointIndex(int3 pointCoord) =>
            pointCoord.x + pointCoord.y * this.PointsPerAxis + pointCoord.z * this.PointsPerAxis * this.PointsPerAxis;

        public float3 ChunkOrigin(int3 chunkCoord) =>
            chunkCoord * this.ChunkSize;

        // Multiply before dividing so the last point lands exactly on ChunkSize and matches the next chunk's origin
        public float3 PointToLocal(int3 pointCoord) =>
            (float3)(pointCoord * this.ChunkSize) / this.CellsPerAxis;

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
