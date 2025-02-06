namespace AdventOfCode2020.Solver;

internal partial class Day19 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Monster Messages";

    private sealed class Rule(string rawData)
    {
        public static readonly Dictionary<string, Rule> AllRules = [];

        private readonly List<List<string>> _allowedCombination = rawData
                .Split('|')
                .ToList()
                .ConvertAll(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());

        public bool IsValid(string targetMessage)
        {
            // Enqueue initial conditions
            Queue<(string message, List<string> combination)> toCheck = new();
            foreach (List<string> combination in _allowedCombination)
            {
                toCheck.Enqueue((targetMessage, combination));
            }

            // BFS search
            while (toCheck.Count > 0)
            {
                (string subMessage, List<string> combination) = toCheck.Dequeue();
                if (combination.Count > 0)
                {
                    string rule = combination[0];
                    if (rule.StartsWith('\"'))
                    {
                        // Check if letter matching what we search
                        if (subMessage.StartsWith(rule[1..^1]))
                        {
                            toCheck.Enqueue((subMessage[1..], combination[1..]));
                        }
                    }
                    else
                    {
                        // Insert set of rule to be test in front of remaining one
                        Rule subRule = AllRules[rule];
                        foreach (List<string> subCombination in subRule._allowedCombination)
                        {
                            List<string> newCombination = [.. subCombination[..].Concat(combination[1..])];
                            toCheck.Enqueue((subMessage, newCombination));
                        }
                    }
                }
                else if (subMessage.Length == 0)
                {
                    // We found a match, search can be stopped
                    return true;
                }
            }

            // No match found
            return false;
        }
    }

    private readonly List<string> _messages = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData(false);
        return _messages.Count(m => Rule.AllRules["0"].IsValid(m)).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData(true);
        return _messages.Count(m => Rule.AllRules["0"].IsValid(m)).ToString();
    }

    private void ExtractData(bool isPart2)
    {
        _messages.Clear();
        Rule.AllRules.Clear();
        foreach (string line in _puzzleInput)
        {
            if (line.Contains(':'))
            {
                // We have a rule
                string[] parts = line.Split(':');
                Rule.AllRules.Add(parts[0], new(parts[1]));
            }
            else if (!string.IsNullOrEmpty(line))
            {
                // We have a message
                _messages.Add(line);
            }
        }

        // Change rules ?
        if (isPart2)
        {
            Rule.AllRules["8"] = new("42 | 42 8");
            Rule.AllRules["11"] = new("42 31 | 42 11 31");
        }
    }
}