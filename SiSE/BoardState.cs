using System.Text;

namespace SiSE;

public struct BoardState 
{
    public int[,] Tiles { get; set;}
    public int Width { get;set; }
    public int Height { get; set;}
    public int? HashCode;
    public (int y, int x) EmptyTile { get; set; }
    public List<Direction> Moves { get; set; }

    // Constructor to create a new board state with given width and height
    public BoardState(int[,] inputTiles)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        EmptyTile = GetEmptyTile();
        Moves = new List<Direction>();
        HashCode = GetHashCode();
    }

    public BoardState(int[,] inputTiles, int y, int x, List<Direction> moves, int? hashCode)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        EmptyTile = (y, x);
        Moves = moves;
        HashCode = hashCode;
    }

    public List<BoardState> GetNeighbours(Direction[] directions)

    {
        var neighbours = new List<BoardState>();
        if (Moves.Count != 0)
        {
            Direction? skip = IPuzzleSolver.Reverse(Moves[Moves.Count - 1]);

            foreach (var direction in directions)
            {
                if (direction != skip)
                {
                    var state = Move(direction);
                    if (state != null) neighbours.Add((BoardState)state);
                }
            }
        }
        else
        {
            foreach (var direction in directions)
            {
                var state = Move(direction);
                if (state != null) neighbours.Add((BoardState)state);
            }
        }


        return neighbours;
    }

    public List<BoardState> GetNeighbours()

    {
        var neighbours = new List<BoardState>();
        foreach (var direction in Enum.GetValues<Direction>())
        {
            var state = Move(direction);
            if (state != null) neighbours.Add((BoardState)state);
        }
        return neighbours;
    }

    // Move a tile in the given direction (if possible), returning a new board state
    public BoardState? Move(Direction direction)
    {
        (var emptyX, var emptyY) = EmptyTile;

        // Check if the move is possible
        var moveX = emptyX;
        var moveY = emptyY;

        switch (direction)
        {
            case Direction.Up:
                if (emptyY > 0)
                    moveY--;
                else
                    return null;
                break;
            case Direction.Down:
                if (moveY < Height - 1)
                    moveY++;
                else
                    return null;
                break;
            case Direction.Left:
                if (emptyX > 0)
                    moveX--;
                else
                    return null;
                break;
            case Direction.Right:
                if (emptyX < Width - 1)
                    moveX++;
                else
                    return null;
                break;
        }

        // Create a new board state with the tile moved
        var newTiles = (int[,])Tiles.Clone();
        newTiles[emptyX, emptyY] = Tiles[moveX, moveY];
        newTiles[moveX, moveY] = 0;
        var newMoves = Moves.ToList();
        newMoves.Add(direction);
        int? newHashCode = (int?)(HashCode * 23 + direction);
        return new BoardState(newTiles, moveX, moveY, newMoves, newHashCode);
    }

    public bool IsGoal()
    {
        var value = 1;
        var max = Height * Width;
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            if (Tiles[x, y] != value) return false;
            value = (value + 1) % max;
        }

        return true;
    }

    private (int x, int y) GetEmptyTile()
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            if (Tiles[y, x] == 0)
                return (y, x);
        throw new InvalidOperationException("Board contains no empty space.");
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        BoardState other = (BoardState)obj;
        return GetHashCode() == other.GetHashCode();
    }

    public bool Equals(BoardState other)
    {
        return Moves.SequenceEqual(other.Moves);
    }

    public override int GetHashCode()
    {
        if (HashCode == null)
        {
            unchecked
            {
                int hash = 17;

                foreach (var m in Moves)
                {
                    hash = (int)(hash * 23 + m);
                }

                return hash;
            }
        }
        else
        {
            return (int)HashCode;
        }
    }

    public string GetPath()
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < Moves.Count; i++)
        {
            stringBuilder.Append(IPuzzleSolver.GetStringFromDirection(Moves[i])); //Cannot be null thought
        }

        return stringBuilder.ToString();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Width: {Width}, Height: {Height}");
        sb.AppendLine("Tiles:");
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++) sb.Append($"{Tiles[x, y],3} ");
            sb.AppendLine();
        }


        sb.AppendLine($"EmptyTile: ({EmptyTile.x}, {EmptyTile.y})");
        sb.AppendLine($"LastMove: {Moves}");

        return sb.ToString();
    }
    
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}