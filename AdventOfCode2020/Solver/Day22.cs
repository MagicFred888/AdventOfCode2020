namespace AdventOfCode2020.Solver;

internal partial class Day22 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Crab Combat";

    private readonly List<Queue<int>> _allDecks = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        _ = PlayAndGetWinnerId(_allDecks[0], _allDecks[1], false);
        return CalculateBestScore().ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        _ = PlayAndGetWinnerId(_allDecks[0], _allDecks[1], true);
        return CalculateBestScore().ToString();
    }

    private static int PlayAndGetWinnerId(Queue<int> deck0, Queue<int> deck1, bool recursiveCombat)
    {
        HashSet<string> previousRounds = [];
        while (deck0.Count > 0 && deck1.Count > 0)
        {
            // Check for infinite game
            string roundHash = string.Join(",", deck0) + "|" + string.Join(",", deck1);
            if (!previousRounds.Add(roundHash))
            {
                return 0;
            }

            // Deal cards
            int p0card = deck0.Dequeue();
            int p1card = deck1.Dequeue();

            // Check winner and put cards back in the proper deck
            if (GetWinner(p0card, deck0, p1card, deck1, recursiveCombat) == 0)
            {
                deck0.Enqueue(p0card);
                deck0.Enqueue(p1card);
            }
            else
            {
                deck1.Enqueue(p1card);
                deck1.Enqueue(p0card);
            }
        }
        return deck0.Count > 0 ? 0 : 1;
    }

    private static int GetWinner(int p0card, Queue<int> deck0, int p1card, Queue<int> deck1, bool recursiveCombat)
    {
        // Simple check
        if (!recursiveCombat || deck0.Count < p0card || deck1.Count < p1card)
        {
            return p0card > p1card ? 0 : 1;
        }

        // Need to start recursive combat
        Queue<int> newDeck0 = new(deck0.Take(p0card));
        Queue<int> newDeck1 = new(deck1.Take(p1card));
        return PlayAndGetWinnerId(newDeck0, newDeck1, true);
    }

    private int CalculateBestScore()
    {
        int score = 0;
        Queue<int> winnerDeck = _allDecks.First(d => d.Count > 0);
        while (winnerDeck.Count > 0)
        {
            score += winnerDeck.Count * winnerDeck.Dequeue();
        }
        return score;
    }

    private void ExtractData()
    {
        _allDecks.Clear();
        Queue<int> currentDeck = new();
        foreach (string line in _puzzleInput)
        {
            if (line.StartsWith("Player"))
            {
                if (currentDeck.Count > 0)
                {
                    _allDecks.Add(currentDeck);
                    currentDeck = new();
                }
            }
            else if (!string.IsNullOrEmpty(line))
            {
                currentDeck.Enqueue(int.Parse(line));
            }
        }
        _allDecks.Add(currentDeck);
    }
}