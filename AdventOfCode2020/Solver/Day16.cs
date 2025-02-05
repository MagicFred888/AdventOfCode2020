namespace AdventOfCode2020.Solver;

internal partial class Day16 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Ticket Translation";

    private enum ScanMode
    {
        Category,
        MyTicket,
        NearbyTickets
    }

    private readonly Dictionary<string, List<(int minValue, int maxValue)>> _categoryRangeDic = [];
    private readonly List<int> _myTicket = [];
    private readonly List<List<int>> _nearbyTickets = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();

        // Check valid tickets
        int nbrOfValidTickets = 0;
        foreach (List<int> ticket in _nearbyTickets)
        {
            nbrOfValidTickets += ticket.Sum(categoryValue => IsValid(categoryValue) ? 0 : categoryValue);
        }
        return nbrOfValidTickets.ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();

        // Get valid tickets
        List<List<int>> validNearbyTickets = _nearbyTickets
            .Where(ticket => ticket.All(categoryValue => IsValid(categoryValue)))
            .ToList();

        // Make list of list containing all possible category
        List<List<string>> possibleCategory = [];
        _myTicket.ForEach(_ => possibleCategory.Add(new(_categoryRangeDic.Keys)));

        // Remove all invalid choice based on validNearbyTickets
        validNearbyTickets.ForEach(ticket =>
        {
            _categoryRangeDic.Keys.ToList().ForEach(category =>
            {
                ticket.Select((value, catId) => new { value, catId })
                      .Where(x => !IsValid(x.value, category))
                      .ToList()
                      .ForEach(x => possibleCategory[x.catId].Remove(category));
            });
        });

        // Remove all choice who are already used
        while (possibleCategory.Any(l => l.Count > 1))
        {
            var singleCategories = possibleCategory
                .Where(l => l.Count == 1)
                .Select(l => l[0])
                .ToList();

            possibleCategory = possibleCategory
                .Select(l => l.Count > 1 ? l.Except(singleCategories).ToList() : l)
                .ToList();
        }
        List<string> categories = possibleCategory.Select(l => l[0]).ToList();

        // Multiply items who are starting with "departure"
        return _myTicket.Select((catValue, catId) => new { catId, catValue })
            .Where(x => categories[x.catId].StartsWith("departure"))
            .Aggregate(1L, (a, b) => a * b.catValue)
            .ToString();
    }

    private bool IsValid(int value, string category)
    {
        return _categoryRangeDic[category].Any(range => value >= range.minValue && value <= range.maxValue);
    }

    private bool IsValid(int categoryValue)
    {
        return _categoryRangeDic.Values
            .SelectMany(ranges => ranges)
            .Any(range => categoryValue >= range.minValue && categoryValue <= range.maxValue);
    }

    private void ExtractData()
    {
        _categoryRangeDic.Clear();
        _myTicket.Clear();
        _nearbyTickets.Clear();

        ScanMode scanMode = ScanMode.Category;
        foreach (string line in _puzzleInput)
        {
            // Empty line
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            // Change scan mode
            ScanMode newScanMode = line switch
            {
                "your ticket:" => ScanMode.MyTicket,
                "nearby tickets:" => ScanMode.NearbyTickets,
                _ => scanMode,
            };
            if (newScanMode != scanMode)
            {
                scanMode = newScanMode;
                continue;
            }

            // Extract data
            switch (scanMode)
            {
                case ScanMode.Category:
                    string[] parts = line.Split(": ");
                    string category = parts[0];
                    List<(int, int)> ranges = parts[1].Split(" or ").Select(range =>
                    {
                        string[] rangeParts = range.Split('-');
                        return (int.Parse(rangeParts[0]), int.Parse(rangeParts[1]));
                    }).ToList();
                    _categoryRangeDic.Add(category, ranges);
                    break;

                case ScanMode.MyTicket:
                    _myTicket.AddRange(line.Split(',').Select(int.Parse));
                    break;

                case ScanMode.NearbyTickets:
                    _nearbyTickets.Add(line.Split(',').Select(int.Parse).ToList());
                    break;
            }
        }
    }
}