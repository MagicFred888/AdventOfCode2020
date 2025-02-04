using AdventOfCode2020.Extensions;
using System.Drawing;

namespace AdventOfCode2020.Solver;

internal partial class Day12 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Rain Risk";

    private readonly List<(string moveType, int moveValue)> _allMove = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        (Point shipPosition, Point shipDirection) = (new(), new(1, 0));
        for (int i = 0; i < _allMove.Count; i++)
        {
            (shipPosition, shipDirection) = ExecuteShipMove(shipPosition, shipDirection, _allMove[i].moveType, _allMove[i].moveValue);
        }
        return shipPosition.ManhattanDistance().ToString();
    }

    private static (Point position, Point direction) ExecuteShipMove(Point position, Point direction, string moveType, int moveValue)
    {
        return moveType switch
        {
            "N" => (position.Add(new(0, moveValue)), direction),
            "S" => (position.Add(new(0, -moveValue)), direction),
            "E" => (position.Add(new(moveValue, 0)), direction),
            "W" => (position.Add(new(-moveValue, 0)), direction),
            "L" when moveValue == 90 => (position, direction.RotateCounterclockwise()),
            "L" when moveValue == 180 => (position, direction.Rotate180Degree()),
            "L" when moveValue == 270 => (position, direction.RotateClockwise()),
            "R" when moveValue == 90 => (position, direction.RotateClockwise()),
            "R" when moveValue == 180 => (position, direction.Rotate180Degree()),
            "R" when moveValue == 270 => (position, direction.RotateCounterclockwise()),
            "F" => (position.Add(direction.Multiply(moveValue)), direction),
            _ => throw new NotImplementedException(),
        };
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        (Point shipPosition, Point waypointPosition) = (new(), new(10, 1));
        for (int i = 0; i < _allMove.Count; i++)
        {
            (shipPosition, waypointPosition) = ExecuteWaypointMove(shipPosition, waypointPosition, _allMove[i].moveType, _allMove[i].moveValue);
        }
        return shipPosition.ManhattanDistance().ToString();
    }

    private static (Point shipPosition, Point waypointPosition) ExecuteWaypointMove(Point shipPosition, Point waypointPosition, string moveType, int moveValue)
    {
        return moveType switch
        {
            "N" => (shipPosition, waypointPosition.Add(new(0, moveValue))),
            "S" => (shipPosition, waypointPosition.Add(new(0, -moveValue))),
            "E" => (shipPosition, waypointPosition.Add(new(moveValue, 0))),
            "W" => (shipPosition, waypointPosition.Add(new(-moveValue, 0))),
            "L" when moveValue == 90 => (shipPosition, waypointPosition.RotateCounterclockwise(shipPosition)),
            "L" when moveValue == 180 => (shipPosition, waypointPosition.Rotate180Degree(shipPosition)),
            "L" when moveValue == 270 => (shipPosition, waypointPosition.RotateClockwise(shipPosition)),
            "R" when moveValue == 90 => (shipPosition, waypointPosition.RotateClockwise(shipPosition)),
            "R" when moveValue == 180 => (shipPosition, waypointPosition.Rotate180Degree(shipPosition)),
            "R" when moveValue == 270 => (shipPosition, waypointPosition.RotateCounterclockwise(shipPosition)),
            "F" => (shipPosition.Add(waypointPosition.Subtract(shipPosition).Multiply(moveValue)), waypointPosition.Add(waypointPosition.Subtract(shipPosition).Multiply(moveValue))),
            _ => throw new NotImplementedException(),
        };
    }

    private void ExtractData()
    {
        _allMove.Clear();
        foreach (string line in _puzzleInput)
        {
            _allMove.Add((line[..1].ToUpper(), int.Parse(line[1..])));
        }
    }
}