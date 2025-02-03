namespace AdventOfCode2020.Solver;

internal partial class Day02 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Password Philosophy";

    public override string GetSolution1(bool isChallenge)
    {
        int nbrValid = 0;
        foreach (string line in _puzzleInput)
        {
            string[] parts = line.Split(['-', ' ', ':'], StringSplitOptions.RemoveEmptyEntries);
            int count = parts[3].ToCharArray().Count(c => c == parts[2][0]);
            nbrValid += count >= int.Parse(parts[0]) && count <= int.Parse(parts[1]) ? 1 : 0;
        }
        return nbrValid.ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        int nbrValid = 0;
        foreach (string line in _puzzleInput)
        {
            string[] parts = line.Split(['-', ' ', ':'], StringSplitOptions.RemoveEmptyEntries);
            nbrValid += parts[3][int.Parse(parts[0]) - 1] == parts[2][0] ^ parts[3][int.Parse(parts[1]) - 1] == parts[2][0] ? 1 : 0;
        }
        return nbrValid.ToString();
    }
}