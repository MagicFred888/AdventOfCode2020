using AdventOfCode2020.Tools;

namespace AdventOfCode2020.Solver;

internal partial class Day10 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Adapter Array";

    private List<int> _adapters = [];
    private readonly Dictionary<string, long> _hashInfo = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();

        // Create new list with difference and count content
        int nbrDiff1 = _adapters.Skip(1).Zip(_adapters, (a, b) => a - b).Count(diff => diff == 1);
        int nbrDiff3 = _adapters.Skip(1).Zip(_adapters, (a, b) => a - b).Count(diff => diff == 3);
        return (nbrDiff1 * nbrDiff3).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        return CountPossibleArrangement(new(_adapters)).ToString();
    }

    private long CountPossibleArrangement(List<int> adapters)
    {
        // Check if we have the sequence in hash dictionary
        string hash = string.Join(",", adapters);
        if (_hashInfo.TryGetValue(hash, out long value))
        {
            return value;
        }

        // Compute the number of arrangement
        long numberOfArrangement = 1;
        for (int i = 1; i < adapters.Count - 1; i++)
        {
            if (adapters[i + 1] - adapters[i - 1] <= 3)
            {
                List<int> newAdapters = adapters.GetRange(i - 1, adapters.Count - i + 1);
                newAdapters.Remove(adapters[i]);
                numberOfArrangement += CountPossibleArrangement(newAdapters);
            }
        }

        // Save in hash dic and return value
        _hashInfo[hash] = numberOfArrangement;
        return numberOfArrangement;
    }

    private void ExtractData()
    {
        _hashInfo.Clear();
        _adapters = QuickList.ListOfInt(_puzzleInput);
        _adapters.Sort();
        _adapters.Insert(0, 0);
        _adapters.Add(_adapters[^1] + 3);
    }
}