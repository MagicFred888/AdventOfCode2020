using AdventOfCode2020.Tools;
using System.Drawing;

namespace AdventOfCode2020.Solver;

internal partial class Day20 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Jurassic Jigsaw";

    private sealed class Tile
    {
        public int Id { get; init; }
        public readonly List<(int left, int top, int right, int bottom, QuickMatrix tile)> PossibleEdges = [];
        public static readonly Dictionary<int, Tile> AllTiles = [];

        public Tile(int id, QuickMatrix tile)
        {
            Id = id;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    PossibleEdges.Add(GetEdges(tile));
                    tile = tile.Clone();
                    tile.RotateClockwise();
                }
                if (i == 0) tile.FlipHorizontal();
            }
        }

        public List<(long tileId, int position)> FindFullSquarre(int squarreSize)
        {
            // Initialization
            Queue<List<Point>> toCheck = new();
            for (int possibleId = 0; possibleId < 8; possibleId++)
            {
                toCheck.Enqueue([new(Id, possibleId)]);
            }

            // Search answers using BFS
            while (toCheck.Count > 0)
            {
                List<Point> currentPath = toCheck.Dequeue();
                if (currentPath.Count == Tile.AllTiles.Count)
                {
                    // Found a full squarre
                    return currentPath.ConvertAll(p => ((long)p.X, p.Y));
                }
                else
                {
                    // pos for which we must find a tile
                    Point gridPos = new(currentPath.Count % squarreSize, currentPath.Count / squarreSize);

                    // Need have new piece left edge matching previous one?
                    bool mustCheckLeft = gridPos.X > 0;
                    int leftEdgeHash = mustCheckLeft ? AllTiles[currentPath[^1].X].PossibleEdges[currentPath[^1].Y].right : -1;

                    // Need have new piece top edge matching bottom of above one?
                    bool mustCheckTop = gridPos.Y > 0;
                    int topEdgeHash = mustCheckTop ? AllTiles[currentPath[^squarreSize].X].PossibleEdges[currentPath[^squarreSize].Y].bottom : -1;

                    // Scan each tile not yet used
                    foreach (Tile tile in AllTiles.Values.Where(tile => !currentPath.Any(p => p.X == tile.Id)))
                    {
                        // Check each of their possible edge
                        for (int i = 0; i < tile.PossibleEdges.Count; i++)
                        {
                            (int left, int top, _, _, _) = tile.PossibleEdges[i];
                            if (mustCheckLeft && leftEdgeHash != left || mustCheckTop && topEdgeHash != top)
                            {
                                continue;
                            }
                            List<Point> newPath = [.. currentPath.Append(new Point(tile.Id, i))];
                            toCheck.Enqueue(newPath);
                        }
                    }
                }
            }
            return [];
        }

        private static (int left, int top, int right, int bottom, QuickMatrix tile) GetEdges(QuickMatrix tile)
        {
            return (GetHashCode(tile.Cols[0]), GetHashCode(tile.Rows[0]), GetHashCode(tile.Cols[^1]), GetHashCode(tile.Rows[^1]), tile);
        }

        private static int GetHashCode(List<CellInfo> cellInfos)
        {
            int hash = 0;
            for (int i = 0; i < cellInfos.Count; i++)
            {
                hash <<= 1;
                hash |= cellInfos[i].StringVal == "#" ? 1 : 0;
            }
            return hash;
        }
    }

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();
        int squarreSize = (int)Math.Sqrt(Tile.AllTiles.Count);
        List<(long tileId, int position)> rightSequence = GetRightSequence(squarreSize);
        return (rightSequence[0].tileId
            * rightSequence[squarreSize - 1].tileId
            * rightSequence[^squarreSize].tileId
            * rightSequence[^1].tileId)
                .ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();
        int squarreSize = (int)Math.Sqrt(Tile.AllTiles.Count);
        List<(long tileId, int position)> rightSequence = GetRightSequence(squarreSize);
        QuickMatrix fullImage = GetFullMonsterImage(rightSequence);

        // Find the monster...
        List<string> monster = [
            "                  O ",
            "O    OO    OO    OOO",
            " O  O  O  O  O  O   "
        ];

        // flip and turn to test all solution
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int nbrOfMonster = FindAllMonster(fullImage, monster);
                if (nbrOfMonster > 0)
                {
                    return (fullImage.Cells.Count(c => c.StringVal == "#") - nbrOfMonster * monster.ConvertAll(s => s.Count(c => c == 'O')).Sum()).ToString();
                }
                fullImage.RotateClockwise();
            }
            if (i == 0) fullImage.FlipHorizontal();
        }
        throw new InvalidDataException();
    }

    private static int FindAllMonster(QuickMatrix fullImage, List<string> monster)
    {
        int nbrFound = 0;
        for (int x = 0; x < fullImage.ColCount - monster[0].Length; x++)
        {
            for (int y = 0; y < fullImage.RowCount - monster.Count; y++)
            {
                if (IsMonster(fullImage, monster, x, y))
                {
                    nbrFound++;
                }
            }
        }
        return nbrFound;
    }

    private static bool IsMonster(QuickMatrix fullImage, List<string> monster, int startX, int startY)
    {
        for (int y = 0; y < monster.Count; y++)
        {
            for (int x = 0; x < monster[y].Length; x++)
            {
                if (monster[y][x] == 'O' && fullImage.Cell(startX + x, startY + y).StringVal != "#")
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static QuickMatrix GetFullMonsterImage(List<(long tileId, int position)> rightSequence)
    {
        int truncatedSize = Tile.AllTiles.Values.First().PossibleEdges[0].tile.ColCount - 2;
        int finalSize = (int)Math.Sqrt(rightSequence.Count) * truncatedSize;
        QuickMatrix fullMonsterImage = new(finalSize, finalSize, ".");
        for (int i = 0; i < rightSequence.Count; i++)
        {
            Tile tile = Tile.AllTiles[(int)rightSequence[i].tileId];
            QuickMatrix tileImage = tile.PossibleEdges[rightSequence[i].position].tile;
            int row = i / (finalSize / truncatedSize) * truncatedSize;
            int col = i % (finalSize / truncatedSize) * truncatedSize;
            QuickMatrix subMatrix = tileImage.GetSubMatrix(new(1, 1), new(tileImage.ColCount - 2, tileImage.RowCount - 2));
            fullMonsterImage.SetSubMatrix(new(col, row), subMatrix);
        }
        return fullMonsterImage;
    }

    private static List<(long tileId, int position)> GetRightSequence(int squarreSize)
    {
        foreach (Tile tile in Tile.AllTiles.Values)
        {
            List<(long tileId, int position)> rightSequence = tile.FindFullSquarre(squarreSize);
            if (rightSequence.Count == Tile.AllTiles.Count)
            {
                return rightSequence;
            }
        }
        throw new InvalidDataException();
    }

    private void ExtractData()
    {
        int lineId = 0;
        Tile.AllTiles.Clear();
        while (lineId < _puzzleInput.Count)
        {
            int tileId = int.Parse(_puzzleInput[lineId].Trim(':').Split(' ')[1]);
            List<string> tileData = _puzzleInput.GetRange(lineId + 1, 10);
            QuickMatrix tile = new(tileData);

            // Add Tile object in list
            Tile.AllTiles.Add(tileId, new(tileId, tile));
            lineId += 12;
        }
    }
}