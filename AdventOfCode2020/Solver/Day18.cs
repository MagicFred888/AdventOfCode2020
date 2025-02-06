namespace AdventOfCode2020.Solver;

internal partial class Day18 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Operation Order";

    public override string GetSolution1(bool isChallenge)
    {
        return _puzzleInput.Sum(l => GetStringValue(l, false)).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        return _puzzleInput.Sum(l => GetStringValue(l, true)).ToString();
    }

    private static long GetStringValue(string data, bool addBeforeMultiply)
    {
        // Check if we still have (...)
        while (data.Contains('('))
        {
            int start = data.LastIndexOf('(');
            int end = data.IndexOf(')', start);
            data = data[..start] + GetStringValue(data[(start + 1)..end], addBeforeMultiply) + data[(end + 1)..];
        }

        // Is there some operation?
        if (data.All(char.IsDigit))
        {
            return long.Parse(data);
        }

        // Parse data until first non digit char
        if (addBeforeMultiply && data.Contains('*'))
        {
            // Parse data until first non digit char
            return data.Split(" * ", StringSplitOptions.RemoveEmptyEntries).ToList()
                .ConvertAll(s => GetStringValue(s, addBeforeMultiply))
                .Aggregate(1L, (acc, value) => acc * value);
        }
        else
        {
            string[] dataParts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            long result = long.Parse(dataParts[0]);
            for (int i = 1; i < dataParts.Length - 1; i += 2)
            {
                result = dataParts[i] == "+" ? result + long.Parse(dataParts[i + 1]) : result * long.Parse(dataParts[i + 1]);
            }
            return result;
        }
    }
}