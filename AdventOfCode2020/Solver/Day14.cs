using System.Text.RegularExpressions;

namespace AdventOfCode2020.Solver;

internal partial class Day14 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Docking Data";

    private enum Operation
    {
        Mask,
        Memory
    }

    private readonly List<(Operation operation, long memoryPosition, long value, string newMask)> _program = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();

        // Run the code
        long orMask = 0; // Use to force at 1
        long andMask = 0; // Use to force at 0
        Dictionary<long, long> memory = [];
        foreach ((Operation operation, long memoryPosition, long value, string newMask) in _program)
        {
            switch (operation)
            {
                case Operation.Mask:
                    (orMask, andMask) = ParseMask(newMask);
                    break;

                case Operation.Memory:
                    memory[memoryPosition] = (value | orMask) & andMask;
                    break;

                default:
                    break;
            }
        }

        // Return result
        return memory.Values.Sum().ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();

        // Run the code
        string mask = "";
        Dictionary<long, long> memory = [];
        foreach ((Operation operation, long memoryPosition, long value, string newMask) in _program)
        {
            switch (operation)
            {
                case Operation.Mask:
                    mask = newMask;
                    break;

                case Operation.Memory:
                    GetAllAddress(memoryPosition, mask).ForEach(p => memory[p] = value);
                    break;

                default:
                    break;
            }
        }

        // Return result
        return memory.Values.Sum().ToString();
    }

    private static List<long> GetAllAddress(long memoryPosition, string mask)
    {
        long baseNumber = memoryPosition;
        List<int> floatingBits = [];
        for (int i = 0; i < mask.Length; i++)
        {
            int bitPosition = mask.Length - i - 1; // String read from left to right but bit from right to left...
            switch (mask[i])
            {
                case '1':
                    baseNumber = SetBit(baseNumber, bitPosition, true);
                    break;

                case 'X':
                    baseNumber = SetBit(baseNumber, bitPosition, false);
                    floatingBits.Add(bitPosition);
                    break;

                default:
                    break;
            }
        }

        // Generate all numbers
        List<long> addresses = [];
        int totalCombinations = (int)Math.Pow(2, floatingBits.Count);
        foreach (long value in Enumerable.Range(0, totalCombinations))
        {
            long result = baseNumber;
            for (int i = 0; i < floatingBits.Count; i++)
            {
                result = SetBit(result, floatingBits[i], (value & (1 << i)) != 0);
            }
            addresses.Add(result);
        }

        // Done
        return addresses;
    }

    private static long SetBit(long number, int bitPosition, bool value)
    {
        return value ? (number | (1L << bitPosition)) : (number & ~(1L << bitPosition)); // Set at 1 if true, 0 if false
    }

    private static (long orMask, long andMask) ParseMask(string line)
    {
        long orMask = 0;
        long andMask = 0;
        foreach (char c in line)
        {
            orMask <<= 1;
            andMask <<= 1;
            switch (c)
            {
                case '1':
                    orMask |= 1;
                    andMask |= 1;
                    break;

                case 'X':
                    andMask |= 1;
                    break;

                default:
                    break;
            }
        }
        return (orMask, andMask);
    }

    private void ExtractData()
    {
        _program.Clear();
        foreach (string line in _puzzleInput)
        {
            if (line.StartsWith("mask"))
            {
                // Update masks
                _program.Add((Operation.Mask, -1, -1, line.Split(" = ")[1]));
            }
            else
            {
                // Update memory
                Match match = DataExtractionRegex().Match(line);
                if (!match.Success)
                {
                    throw new InvalidDataException("Invalid data format.");
                }

                // Update memory
                long memoryPosition = long.Parse(match.Groups["memoryPos"].Value);
                long value = long.Parse(match.Groups["value"].Value);
                _program.Add((Operation.Memory, memoryPosition, value, ""));
            }
        }
    }

    [GeneratedRegex(@"^mem\[(?<memoryPos>\d+)\] = (?<value>\d+)$")]
    private static partial Regex DataExtractionRegex();
}