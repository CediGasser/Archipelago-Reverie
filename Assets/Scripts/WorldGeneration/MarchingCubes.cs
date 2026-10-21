using System.Collections.Generic;
using Chunks;
using Unity.Mathematics;

namespace WorldGeneration
{
    public class MarchingCubes
    {
        public List<Triangle> GetTriangles(float[] densityField, ChunkGrid chunkGrid, float isoLevel)
        {
            List<Triangle> triangles = new List<Triangle>();

            foreach (var cellCoord in chunkGrid.Cells())
            {
                // Grid coordinates of all 8 corners
                var x = cellCoord.x;
                var y = cellCoord.y;
                var z = cellCoord.z;
                int3[] cornerCoords =
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
                    densityField[chunkGrid.PointIndex(cornerCoords[0])],
                    densityField[chunkGrid.PointIndex(cornerCoords[1])],
                    densityField[chunkGrid.PointIndex(cornerCoords[2])],
                    densityField[chunkGrid.PointIndex(cornerCoords[3])],
                    densityField[chunkGrid.PointIndex(cornerCoords[4])],
                    densityField[chunkGrid.PointIndex(cornerCoords[5])],
                    densityField[chunkGrid.PointIndex(cornerCoords[6])],
                    densityField[chunkGrid.PointIndex(cornerCoords[7])]
                };
                
                // Local positions of all 8 corners
                float3[] cornerPositions =
                {
                    chunkGrid.PointToLocal(cornerCoords[0]),
                    chunkGrid.PointToLocal(cornerCoords[1]),
                    chunkGrid.PointToLocal(cornerCoords[2]),
                    chunkGrid.PointToLocal(cornerCoords[3]),
                    chunkGrid.PointToLocal(cornerCoords[4]),
                    chunkGrid.PointToLocal(cornerCoords[5]),
                    chunkGrid.PointToLocal(cornerCoords[6]),
                    chunkGrid.PointToLocal(cornerCoords[7]),
                };
                        
                this.Polygonise(cornerValues, cornerPositions, isoLevel, triangles);
            }
            return triangles;
        }

        private void Polygonise(float[] cornerValues, float3[] cornerPositions, float isoLevel, List<Triangle> triangles)
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
                // Edges i, i+1, i+2 are assigned to A, C, B on purpose: the table's triangles are wound
                // opposite to Unity's clockwise front faces, so swapping B and C flips them to face outward.
                var triangle = new Triangle()
                {
                    VertexA = this.VertexInterpolate(
                        isoLevel, 
                        cornerPositions[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 0]], 
                        cornerPositions[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 1]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 0]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i], 1]]),
                    VertexC = this.VertexInterpolate(
                        isoLevel, 
                        cornerPositions[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 0]], 
                        cornerPositions[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 1]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 0]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+1], 1]]),
                    VertexB = this.VertexInterpolate(
                        isoLevel, 
                        cornerPositions[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 0]], 
                        cornerPositions[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 1]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 0]], 
                        cornerValues[MarchingCubesTables.EdgeCornerTable[MarchingCubesTables.TriangleTable[cubeIndex, i+2], 1]])
                };
                triangles.Add(triangle);
            }
        }

        private float3 VertexInterpolate(float isoLevel, float3 pos1, float3 pos2, float v1, float v2)
        {
            return math.lerp(pos1, pos2, (v1 - isoLevel) / (v1 - v2));
        }
    }
}
