using System.Drawing;
using AdventOfCode2020.Extensions;
using AdventOfCode2020.Tools;

namespace AdventOfCode2020.Solver;

internal partial class Day03 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Toboggan Trajectory";

    private QuickMatrix _treeMap = new();

    public override string GetSolution1(bool isChallenge)
    {
        _treeMap = new(_puzzleInput);
        return CountHitTrees(new(3, 1)).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        _treeMap = new(_puzzleInput);
        return new List<Point>() { new(1, 1), new(3, 1), new(5, 1), new(7, 1), new(1, 2) }
            .ConvertAll(p => CountHitTrees(p))
            .Aggregate((a, b) => a * b)
            .ToString();
    }

    private long CountHitTrees(Point direction)
    {
        Point position = new();
        long numberOfTreesHit = 0;
        while (position.Y < _treeMap.RowCount)
        {
            numberOfTreesHit += _treeMap.Cell(position).StringVal == "#" ? 1 : 0;
            position = position.Add(direction).Modulo(new(_treeMap.ColCount, 0));
        }
        return numberOfTreesHit;
    }
}