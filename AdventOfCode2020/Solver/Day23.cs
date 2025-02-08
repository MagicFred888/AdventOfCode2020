using AdventOfCode2020.Extensions;
using AdventOfCode2020.Tools;
using System.Text;

namespace AdventOfCode2020.Solver;

internal partial class Day23 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Crab Cups";

    private readonly Dictionary<int, ChainedNode> _nodes = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData(true);

        // Perform moves
        int startId = _puzzleInput[0][0].ToString().ToInt();
        ChainedNode startNode = _nodes[startId];
        for (int i = 0; i < 100; i++)
        {
            PerformeMove(ref startNode);
        }

        // Get cup order as answer
        startNode = _nodes[1].Next;
        StringBuilder result = new();
        do
        {
            result.Append(startNode.Value);
            startNode = startNode.Next;
        } while (startNode.Value != 1);

        // Done
        return result.ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData(false);

        // Perform moves
        int startId = _puzzleInput[0][0].ToString().ToInt();
        ChainedNode startNode = _nodes[startId];
        for (int i = 0; i < 10000000; i++)
        {
            PerformeMove(ref startNode);
        }

        // Get cup order as answer
        return ((long)_nodes[1].Next.Value * (long)_nodes[1].Next.Next.Value).ToString();
    }

    private void PerformeMove(ref ChainedNode startNode)
    {
        // Remove the 3 nodes
        ChainedNode removed = startNode.RemoveNextNode(3);

        // Find insertion point
        int destination = startNode.Value;
        do
        {
            destination = ((destination - 2 + _nodes.Count) % _nodes.Count) + 1;
        } while (removed.ChainContain(destination));

        // Insert back
        _nodes[destination].InsertAfter(removed);

        // Move next
        startNode = startNode.Next;
    }

    private void ExtractData(bool partOne)
    {
        // Erase
        _nodes.Clear();

        // Base nodes from input
        string data = _puzzleInput[0];
        ChainedNode startNode = new(data[0].ToString().ToInt());
        _nodes[startNode.Value] = startNode;
        foreach (char c in data[1..])
        {
            ChainedNode newNode = new(c.ToString().ToInt());
            _nodes[newNode.Value] = newNode;
            startNode.InsertAfter(newNode);
            startNode = newNode;
        }

        // Add node up to 1000000
        if (!partOne)
        {
            for (int i = 10; i <= 1000000; i++)
            {
                ChainedNode newNode = new(i);
                _nodes[newNode.Value] = newNode;
                startNode.InsertAfter(newNode);
                startNode = newNode;
            }
        }
    }
}