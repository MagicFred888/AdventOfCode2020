namespace AdventOfCode2020.Solver;

internal partial class Day25 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Combo Breaker";

    private readonly List<long> _publicKeys = [];

    public override string GetSolution1(bool isChallenge)
    {
        // Get public keys
        _publicKeys.Clear();
        _publicKeys.AddRange(_puzzleInput.Select(long.Parse).ToList());

        // Get number of loops
        long nbrOfLoop1 = GetNumberOfLoop(_publicKeys[1]);
        return Transform(_publicKeys[0], nbrOfLoop1).ToString();
    }

    private static long GetNumberOfLoop(long expectedResult)
    {
        int result = 1;
        for (int i = 1; i < int.MaxValue; i++)
        {
            result = (result * 7) % 20201227;
            if (result == expectedResult)
            {
                return i;
            }
        }
        throw new InvalidDataException("No solution found!");
    }

    private static long Transform(long subjectNumber, long loopSize)
    {
        long result = 1;
        for (int i = 0; i < loopSize; i++)
        {
            result = (result * subjectNumber) % 20201227;
        }
        return result;
    }

    public override string GetSolution2(bool isChallenge)
    {
        return "Merry Christmas!";
    }
}