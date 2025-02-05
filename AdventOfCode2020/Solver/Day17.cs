using AdventOfCode2020.Tools;
using System.Numerics;

namespace AdventOfCode2020.Solver;

internal partial class Day17 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Conway Cubes";

    public override string GetSolution1(bool isChallenge)
    {
        // Get data
        List<Vector4> activePosition = new QuickMatrix(_puzzleInput).Cells
            .FindAll(c => c.StringVal == "#")
            .ConvertAll(c => new Vector4(c.Position.X, c.Position.Y, 0, 0));

        // Make 6 cycles and count
        Enumerable.Range(0, 6).ToList().ForEach(_ => activePosition = ExecuteCycle(activePosition, false));
        return activePosition.Count.ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        // Get data
        List<Vector4> activePosition = new QuickMatrix(_puzzleInput).Cells
            .FindAll(c => c.StringVal == "#")
            .ConvertAll(c => new Vector4(c.Position.X, c.Position.Y, 0, 0));

        // Make 6 cycles and count
        Enumerable.Range(0, 6).ToList().ForEach(_ => activePosition = ExecuteCycle(activePosition, true));
        return activePosition.Count.ToString();
    }

    private static List<Vector4> ExecuteCycle(List<Vector4> activePosition, bool isHypercube)
    {
        // Get bounding box to use
        (float minX, float minY, float minZ, float minW) = (
            activePosition.Min(c => c.X) - 1,
            activePosition.Min(c => c.Y) - 1,
            activePosition.Min(c => c.Z) - 1,
            isHypercube ? activePosition.Min(c => c.W) - 1 : 0
        );
        (float maxX, float maxY, float maxZ, float maxW) = (
            activePosition.Max(c => c.X) + 1,
            activePosition.Max(c => c.Y) + 1,
            activePosition.Max(c => c.Z) + 1,
            isHypercube ? activePosition.Max(c => c.W) + 1 : 0
        );

        // Scan the entire volume
        List<Vector4> newActivePosition = [];
        for (float x = minX; x <= maxX; x++)
        {
            for (float y = minY; y <= maxY; y++)
            {
                for (float z = minZ; z <= maxZ; z++)
                {
                    for (float w = minW; w <= maxW; w++)
                    {
                        Vector4 currentPosition = new(x, y, z, w);
                        bool isActive = activePosition.Contains(currentPosition);
                        int activeAround = activePosition.FindAll(c => Math.Abs(c.X - x) <= 1 && Math.Abs(c.Y - y) <= 1 && Math.Abs(c.Z - z) <= 1 && Math.Abs(c.W - w) <= 1 && c != currentPosition).Count;
                        if ((isActive && (activeAround == 2 || activeAround == 3)) || (!isActive && activeAround == 3))
                        {
                            newActivePosition.Add(new Vector4(x, y, z, w));
                        }
                    }
                }
            }
        }
        return newActivePosition;
    }
}