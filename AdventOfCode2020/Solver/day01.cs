using AdventOfCode2020.Tools;

namespace AdventOfCode2020.Solver;

internal partial class Day01 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Report Repair";

    public override string GetSolution1(bool isChallenge)
    {
        List<int> numbers = QuickList.ListOfInt(_puzzleInput);
        List<List<int>> results = SmallTools.GenerateCombinations(numbers.Count, 2).FindAll(n => numbers[n[0]] + numbers[n[1]] == 2020);
        if (results.Count == 1)
        {
            return (numbers[results[0][0]] * numbers[results[0][1]]).ToString();
        }
        throw new InvalidDataException("No solution found !");
    }

    public override string GetSolution2(bool isChallenge)
    {
        List<int> numbers = QuickList.ListOfInt(_puzzleInput);
        List<List<int>> results = SmallTools.GenerateCombinations(numbers.Count, 3).FindAll(n => numbers[n[0]] + numbers[n[1]] + numbers[n[2]] == 2020);
        if (results.Count == 1)
        {
            return (numbers[results[0][0]] * numbers[results[0][1]] * numbers[results[0][2]]).ToString();
        }
        throw new InvalidDataException("No solution found !");
    }
}