namespace AdventOfCode2020.Solver;

internal partial class Day05 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Binary Boarding";

    public override string GetSolution1(bool isChallenge)
    {
        return _puzzleInput.Max(i => GetSeatInfo(i).seatId).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        // Fill a dictionary with all seats
        Dictionary<int, List<int>> allSeats = [];
        foreach (string seatCode in _puzzleInput)
        {
            (int row, int col, _) = GetSeatInfo(seatCode);
            if (!allSeats.TryGetValue(row, out List<int>? value))
            {
                value = [];
                allSeats[row] = value;
            }
            value.Add(col);
        }

        // Search ours
        int myRow = allSeats.First(kvp => kvp.Value.Count != 8).Key;
        int myCol = Enumerable.Range(0, 8).Except(allSeats[myRow]).ToList()[0];

        // Return our seat ID
        return (myRow * 8 + myCol).ToString();
    }

    private static (int row, int col, int seatId) GetSeatInfo(string seatCode)
    {
        // Get Row
        (int minRow, int maxRow) = (0, 127);
        int row = seatCode.Take(7).Aggregate((minRow, maxRow), (acc, c) => c switch
        {
            'F' => (acc.minRow, (acc.maxRow + acc.minRow) / 2),
            'B' => ((acc.maxRow + acc.minRow) / 2 + 1, acc.maxRow),
            _ => throw new InvalidDataException($"Invalid character: {c}")
        }).minRow;

        // Get Col
        (int minCol, int maxCol) = (0, 7);
        int col = seatCode.Skip(7).Take(3).Aggregate((minCol, maxCol), (acc, c) => c switch
        {
            'L' => (acc.minCol, (acc.maxCol + acc.minCol) / 2),
            'R' => ((acc.maxCol + acc.minCol) / 2 + 1, acc.maxCol),
            _ => throw new InvalidDataException($"Invalid character: {c}")
        }).minCol;

        // Get seat ID
        int seatId = row * 8 + col;

        // Check for debug
        if (seatCode.Contains(':'))
        {
            int check = int.Parse(seatCode[(seatCode.IndexOf(':') + 1)..]);
            if (check != seatId)
            {
                throw new InvalidDataException($"SeatCode: {seatCode} - Expected: {seatId} - Found: {check}");
            }
        }

        // Done
        return (row, col, seatId);
    }
}