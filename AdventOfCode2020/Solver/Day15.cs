namespace AdventOfCode2020.Solver;

internal partial class Day15 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Rambunctious Recitation";

    public override string GetSolution1(bool isChallenge)
    {
        return PlayGame2(2020).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        return PlayGame2(30000000).ToString();
    }

    private long PlayGame2(int nbrOfTurn)
    {
        // Initialization
        int[] game = new int[nbrOfTurn];
        List<int> input = _puzzleInput[0].Split(',').ToList().ConvertAll(int.Parse);

        // Record initial values
        for (int i = 0; i < input.Count; i++)
        {
            game[input[i]] = i + 1;
        }

        // Run the game for remaining turns
        int lastNumberSpoken = input[^1];
        for (int turnId = input.Count; turnId < nbrOfTurn; turnId++)
        {
            int numberSpoken = game[lastNumberSpoken] == 0 ? 0 : turnId - game[lastNumberSpoken];
            game[lastNumberSpoken] = turnId;
            lastNumberSpoken = numberSpoken;
        }

        // Return last number spoken
        return lastNumberSpoken;
    }
}