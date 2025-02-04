using AdventOfCode2020.Tools;

namespace AdventOfCode2020.Solver;

internal partial class Day13 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Shuttle Search";

    public override string GetSolution1(bool isChallenge)
    {
        // Get data
        long departure = long.Parse(_puzzleInput[0]);
        List<long> buses = _puzzleInput[1].Split(',').Where(x => x != "x").Select(long.Parse).ToList();

        // Local function to calculate wait time
        long CalculateWaitTime(long busId) => ((departure / busId) + 1) * busId - departure;

        // Make the calculation
        long minWait = buses.Min(b => CalculateWaitTime(b));
        long busId = buses.First(b => CalculateWaitTime(b) == minWait);
        return (busId * minWait).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        // Get data (second line only)
        List<long> buses = _puzzleInput[1].Split(',').ToList().ConvertAll(x => x == "x" ? "-1" : x).Select(long.Parse).ToList();

        // Combine them to be able to solve the system using CRT (Chinese Remainder Theorem), reason why we check if prime.
        List<long> numbers = [];
        List<long> remainders = [];
        for (int i = 0; i < buses.Count; i++)
        {
            if (buses[i] != -1)
            {
                if (!SmallTools.IsPrime(buses[i]))
                {
                    throw new InvalidDataException("This solution is only valid for prime numbers.");
                }
                numbers.Add((int)buses[i]);
                remainders.Add((int)(buses[i] - i));
            }
        }

        // Return result
        return ChineseRemainderTheorem.GetSmallestNumber([.. numbers], [.. remainders]).ToString();
    }
}