using AdventOfCode2020.Extensions;
using AdventOfCode2020.Tools;
using System.Drawing;

namespace AdventOfCode2020.Solver;

internal partial class Day11 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Seating System";

    public override string GetSolution1(bool isChallenge)
    {
        QuickMatrix seatingArea = SearchStableState(new(_puzzleInput), true);
        return seatingArea.Cells.Count(c => c.StringVal == "#").ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        QuickMatrix seatingArea = SearchStableState(new(_puzzleInput), false);
        return seatingArea.Cells.Count(c => c.StringVal == "#").ToString();
    }

    private static QuickMatrix SearchStableState(QuickMatrix seatingArea, bool adjacentOnly)
    {
        List<bool> previousRound;
        for (int i = 0; i < int.MaxValue; i++)
        {
            previousRound = seatingArea.Cells.ConvertAll(c => c.StringVal == "L");
            seatingArea = ComputeNext(seatingArea, adjacentOnly);
            if (seatingArea.Cells.ConvertAll(c => c.StringVal == "L").SequenceEqual(previousRound))
            {
                return seatingArea;
            }
        }
        throw new InvalidDataException("No solution found");
    }

    private static QuickMatrix ComputeNext(QuickMatrix seatingArea, bool adjacentOnly)
    {
        QuickMatrix newSeatingArea = seatingArea.Clone();
        foreach (CellInfo seat in newSeatingArea.Cells.Where(c => c.StringVal != "."))
        {
            List<string> seatsAround;
            if (adjacentOnly)
            {
                seatsAround = seatingArea.GetNeighbours(seat.Position).ConvertAll(c => c.StringVal);
            }
            else
            {
                seatsAround = [];
                foreach (Point direction in QuickMatrix.Directions[TouchingMode.All])
                {
                    seatsAround.Add(GetFirstSeat(ref seatingArea, seat.Position, direction));
                }
                seatsAround.RemoveAll(s => s == "");
            }
            int maxToStay = adjacentOnly ? 4 : 5;
            seat.StringVal = seat.StringVal switch
            {
                "L" => seatsAround.All(s => s != "#") ? "#" : "L",
                "#" => seatsAround.Count(s => s == "#") >= maxToStay ? "L" : "#",
                _ => "."
            };
        }
        return newSeatingArea;
    }

    private static string GetFirstSeat(ref QuickMatrix seatingArea, Point position, Point direction)
    {
        do
        {
            position = position.Add(direction);
        } while (seatingArea.Cell(position).IsValid && seatingArea.Cell(position).StringVal == ".");
        return seatingArea.Cell(position).IsValid ? seatingArea.Cell(position).StringVal : "";
    }
}