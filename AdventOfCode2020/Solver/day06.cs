namespace AdventOfCode2020.Solver;

internal partial class Day06 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Custom Customs";

    private readonly List<List<string>> _groupAnswers = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        return _groupAnswers.Sum(g => string.Join("", g).Distinct().Count()).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        return _groupAnswers.Sum(g => CountLettersInAllGroup(g)).ToString();
    }

    private static int CountLettersInAllGroup(List<string> group)
    {
        return group[0].Count(c => group.All(g => g.Contains(c)));
    }

    private void ExtractData()
    {
        _groupAnswers.Clear();
        _groupAnswers.Add([]);
        foreach (string answer in _puzzleInput)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                _groupAnswers.Add([]);
            }
            else
            {
                _groupAnswers[^1].Add(answer);
            }
        }
    }
}