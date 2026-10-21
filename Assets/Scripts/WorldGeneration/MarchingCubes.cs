using System.Collections.Generic;
using System.Data;
using Chunks;
using Unity.Mathematics;

namespace WorldGeneration
{
    public class MarchingCubes
    {
        public List<Triangle> GetTriangles(float[] densityField, ChunkGrid chunkGrid, float isoLevel)
        {
            List<Triangle> triangles = new List<Triangle>();

            foreach (var cell in chunkGrid.Cells())
            {
                // The 8 corners
                var x = cell.x;
                var y = cell.y;
                var z = cell.z;
                int3[] corners =
                {
                    new (x, y, z),
                    new (x, y, z + 1),
                    new (x + 1, y, z + 1),
                    new (x + 1, y, z),
                    new (x, y + 1, z),
                    new (x, y + 1, z + 1),
                    new (x + 1, y + 1, z + 1),
                    new (x + 1, y + 1, z)
                };
                        
                // Density values at all 8 corners of a cube
                float[] cornerValues =
                {
                    densityField[chunkGrid.Index(corners[0])],
                    densityField[chunkGrid.Index(corners[1])],
                    densityField[chunkGrid.Index(corners[2])],
                    densityField[chunkGrid.Index(corners[3])],
                    densityField[chunkGrid.Index(corners[4])],
                    densityField[chunkGrid.Index(corners[5])],
                    densityField[chunkGrid.Index(corners[6])],
                    densityField[chunkGrid.Index(corners[7])]
                };
                
                // Coordinates at all 8 corners
                float3[] cornerCoordinates =
                {
                    chunkGrid.ToLocal(corners[0]),
                    chunkGrid.ToLocal(corners[1]),
                    chunkGrid.ToLocal(corners[2]),
                    chunkGrid.ToLocal(corners[3]),
                    chunkGrid.ToLocal(corners[4]),
                    chunkGrid.ToLocal(corners[5]),
                    chunkGrid.ToLocal(corners[6]),
                    chunkGrid.ToLocal(corners[7]),
                };
                        
                this.Polygonise(cornerValues, cornerCoordinates, isoLevel, ref triangles);
            }
            return triangles;
        }

        private void Polygonise(float[] cornerValues, float3[] cornerCoordinates, float isoLevel, ref List<Triangle> triangles)
        {
            byte cubeIndex = 0;
            if (cornerValues[0] < isoLevel) cubeIndex |= 1;
            if (cornerValues[1] < isoLevel) cubeIndex |= 2;
            if (cornerValues[2] < isoLevel) cubeIndex |= 4;
            if (cornerValues[3] < isoLevel) cubeIndex |= 8;
            if (cornerValues[4] < isoLevel) cubeIndex |= 16;
            if (cornerValues[5] < isoLevel) cubeIndex |= 32;
            if (cornerValues[6] < isoLevel) cubeIndex |= 64;
            if (cornerValues[7] < isoLevel) cubeIndex |= 128;

            // Cube is completely inside or outside isosurface -> no triangles
            if (MarchingCubesTables.TriangleTable[cubeIndex, 0] == -1) return;
            
            for (var i = 0; MarchingCubesTables.TriangleTable[cubeIndex, i] != -1; i += 3)
            {
                var triangle = new Triangle()
                {
                    VertexA = this.VertexInterpolate(
                        isoLevel, 
                        cornerCoordinates[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 0]], 
                        cornerCoordinates[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 1]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 0]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 1]]),
                    VertexC = this.VertexInterpolate(
                        isoLevel, 
                        cornerCoordinates[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 0]], 
                        cornerCoordinates[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 1]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 0]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 1]]),
                    VertexB = this.VertexInterpolate(
                        isoLevel, 
                        cornerCoordinates[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 0]], 
                        cornerCoordinates[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 1]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 0]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 1]])
                };
                triangles.Add(triangle);
            }
        }

        private float3 VertexInterpolate(float isoLevel, float3 p1, float3 p2, float v1, float v2)
        {
            return math.lerp(p1, p2, (v1 - isoLevel) / (v1 - v2));
        }
    }
}
