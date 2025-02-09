using AdventOfCode2020.Tools;
using System.Drawing;

namespace AdventOfCode2020.Solver;

internal partial class Day24 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Lobby Layout";

    private QuickHexGrid _hexGrid = new();

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        foreach (string path in _puzzleInput)
        {
            Point position = _hexGrid.GetTileCoordinate(new(), path);
            _hexGrid.HexTiles[position] = !_hexGrid.HexTiles[position];
        }
        return _hexGrid.HexTiles.Count(kvp => kvp.Value).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        // Needed to restart with end of part1 result...
        GetSolution1(isChallenge);

        // Perform the 100 steps
        for (int i = 0; i < 100; i++)
        {
            Dictionary<Point, bool> newState = [];
            foreach (Point position in _hexGrid.HexTiles.Keys)
            {
                List<bool> around = _hexGrid.GetNeighbours(position);
                newState.Add(position, _hexGrid.HexTiles[position] switch
                {
                    true => around.Any(b => b) && around.Count(b => b) <= 2,
                    false => around.Count(b => b) == 2
                });
            }
            _hexGrid.SetAllTiles(newState);
        }
        return _hexGrid.HexTiles.Count(kvp => kvp.Value).ToString(); //3818 too low
    }

    private void ExtractData()
    {
        _hexGrid = new(QuickHexGrid.HexGridType.DoubleWidth, 115); // 115 choose due to some start + 100 cycles... Adjust if needed
    }
}