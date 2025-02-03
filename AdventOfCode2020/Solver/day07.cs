namespace AdventOfCode2020.Solver;

internal partial class Day07 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Handy Haversacks";

    private sealed class Bag(string color)
    {
        public string Color => color;

        public Dictionary<Bag, int> MaxAllowed = [];

        public bool CanContain(string targetColor)
        {
            if (MaxAllowed.Keys.Any(b => b.Color == targetColor)) return true;
            return MaxAllowed.Keys.Any(b => b.CanContain(targetColor));
        }

        public long CountBagInside()
        {
            return MaxAllowed.Sum(kvp => kvp.Value * (1 + kvp.Key.CountBagInside()));
        }
    }

    private readonly Dictionary<string, Bag> _allBags = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        return _allBags.Values.Count(b => b.CanContain("shiny gold")).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        return _allBags["shiny gold"].CountBagInside().ToString();
    }

    private void ExtractData()
    {
        string mainSplitter = " bags contain ";

        // Create all bags
        _allBags.Clear();
        foreach (string line in _puzzleInput)
        {
            string[] parts = line.Split(mainSplitter);
            string color = parts[0];
            if (!_allBags.TryGetValue(color, out _))
            {
                Bag bag = new(color);
                _allBags[color] = bag;
            }
        }

        // Fill bags
        foreach (string line in _puzzleInput)
        {
            string[] parts = line.Split(mainSplitter);
            string color = parts[0];
            Bag bag = _allBags[color];
            foreach ((int subBagQuantity, string subBagColor) in parts[1]
                .Split(", ").ToList()
                .ConvertAll(s => s[..s.IndexOf("bag")].Trim())
                .ConvertAll(s => (int.TryParse(s[..s.IndexOf(' ')], out int qty) ? qty : 0, s[(s.IndexOf(' ') + 1)..]))
                .Where(b => b.Item1 > 0))
            {
                if (!_allBags.TryGetValue(subBagColor, out Bag? subBag))
                {
                    subBag = new(subBagColor);
                    _allBags[subBagColor] = subBag;
                }
                bag.MaxAllowed[subBag] = subBagQuantity;
            }
        }
    }
}