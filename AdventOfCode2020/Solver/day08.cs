namespace AdventOfCode2020.Solver;

internal partial class Day08 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Handheld Halting";

    private enum Operation
    {
        acc,
        jmp,
        nop
    }

    private readonly List<(Operation op, int value)> _program = [];
    private int _accumulator = 0;

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        RunUntilEnd();
        return _accumulator.ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        for (int i = 0; i < _program.Count; i++)
        {
            // No change on acc
            if (_program[i].op == Operation.acc)
            {
                continue;
            }

            // Save original operation
            (Operation op, int value) = _program[i];
            _program[i] = op == Operation.jmp ? (Operation.nop, value) : (Operation.jmp, value);

            // Run program
            if (RunUntilEnd())
            {
                // Done
                return _accumulator.ToString();
            }
            else
            {
                // Restore operation to check another change
                _program[i] = (op, value);
            }
        }
        throw new InvalidDataException("No solution found");
    }

    private bool RunUntilEnd()
    {
        int position = 0;
        _accumulator = 0;
        HashSet<int> visitedPosition = [];
        while (position >= 0 && position < _program.Count)
        {
            // Start infinite loop ?
            if (!visitedPosition.Add(position))
            {
                return false;
            }

            // Execute operation
            (Operation op, int value) = _program[position];
            switch (op)
            {
                case Operation.acc:
                    _accumulator += value;
                    position++;
                    break;

                case Operation.jmp:
                    position += value;
                    break;

                case Operation.nop:
                    position++;
                    break;

                default:
                    break;
            }
        }
        return position == _program.Count;
    }

    private void ExtractData()
    {
        _program.Clear();
        _puzzleInput.ConvertAll(s => s.Split(' ')).ForEach(p => _program.Add((Enum.Parse<Operation>(p[0]), int.Parse(p[1]))));
    }
}