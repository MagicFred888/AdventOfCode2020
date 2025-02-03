using AdventOfCode2020.Tools;

namespace AdventOfCode2020.Solver;

internal partial class Day09 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Encoding Error";

    public override string GetSolution1(bool isChallenge)
    {
        int preambleSize = isChallenge ? 25 : 5;
        List<long> numbers = QuickList.ListOfLong(_puzzleInput);
        return FindInvalidNumber(numbers, preambleSize).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        // Get Invalid number
        int preambleSize = isChallenge ? 25 : 5;
        List<long> numbers = QuickList.ListOfLong(_puzzleInput);
        long invalidNumber = FindInvalidNumber(numbers, preambleSize);

        // Scan from first until we found the answer
        for (int i = 0; i < numbers.Count; i++)
        {
            var range = numbers.Skip(i) // Get right start position
                               .Select((value, index) => new { value, index }) // Convert as (value, index)
                               .TakeWhile((value, index) => numbers.Skip(i).Take(index + 1).Sum() <= invalidNumber) // Sum as long as not overshooting invalidNumber value
                               .ToList();

            if (range.Sum(x => x.value) == invalidNumber)
            {
                return (range.Min(p => p.value) + range.Max(p => p.value)).ToString();
            }
        }
        throw new InvalidDataException("No solution found !");
    }

    private static long FindInvalidNumber(List<long> numbers, int preambleSize)
    {
        for (int i = 0; i < numbers.Count - preambleSize; i++)
        {
            List<long> preamble = numbers.GetRange(i, preambleSize);
            if (!SmallTools.GenerateCombinations(preambleSize, 2).Exists(n => preamble[n[0]] + preamble[n[1]] == numbers[i + preambleSize]))
            {
                return numbers[i + preambleSize];
            }
        }
        throw new InvalidDataException("No solution found !");
    }
}