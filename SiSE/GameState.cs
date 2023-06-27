using System.Text;

namespace SiSE;

public struct GameState : IEquatable<GameState>
{
    public BoardState BoardState;
    public int? HashCode;
    public (int y, int x) EmptyTile { get; set; }
    public List<Direction> Moves { get; set; }

    // Constructor to create a new board state with given width and height
    public GameState(int[,] inputTiles)
    {
        BoardState.Width = inputTiles.GetLength(0);
        BoardState.Height = inputTiles.GetLength(1);
        BoardState.Tiles = inputTiles;
        EmptyTile = GetEmptyTile();
        Moves = new List<Direction>();
        HashCode = GetHashCode();
    }

    public GameState(int[,] inputTiles, int y, int x, List<Direction> moves, int? hashCode)
    {
        BoardState.Width = inputTiles.GetLength(0);
        BoardState.Height = inputTiles.GetLength(1);
        BoardState.Tiles = inputTiles;
        EmptyTile = (y, x);
        Moves = moves;
        HashCode = hashCode;
    }

    public List<GameState> GetNeighbours(Direction[] directions)

    {
        var neighbours = new List<GameState>();
        if (Moves.Count != 0)
        {
            Direction? skip = IPuzzleSolver.Reverse(Moves[Moves.Count - 1]);

            foreach (var direction in directions)
            {
                if (direction != skip)
                {
                    var state = Move(direction);
                    if (state != null) neighbours.Add((GameState)state);
                }
            }
        }
        else
        {
            foreach (var direction in directions)
            {
                var state = Move(direction);
                if (state != null) neighbours.Add((GameState)state);
            }
        }


        return neighbours;
    }

    public List<GameState> GetNeighbours()

    {
        var neighbours = new List<GameState>();
        foreach (var direction in Enum.GetValues<Direction>())
        {
            var state = Move(direction);
            if (state != null) neighbours.Add((GameState)state);
        }
        return neighbours;
    }

    // Move a tile in the given direction (if possible), returning a new board state
    public GameState? Move(Direction direction)
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
                if (moveY < BoardState.Height - 1)
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
                if (emptyX < BoardState.Width - 1)
                    moveX++;
                else
                    return null;
                break;
        }

        // Create a new board state with the tile moved
        var newTiles = (int[,])BoardState.Tiles.Clone();
        newTiles[emptyX, emptyY] = BoardState.Tiles[moveX, moveY];
        newTiles[moveX, moveY] = 0;
        var newMoves = Moves.ToList();
        newMoves.Add(direction);
        int? newHashCode = (int?)(HashCode * 23 + direction);
        return new GameState(newTiles, moveX, moveY, newMoves, newHashCode);
    }

    public bool IsGoal()
    {
        var value = 1;
        var max = BoardState.Height * BoardState.Width;
        for (var y = 0; y < BoardState.Height; y++)
        for (var x = 0; x < BoardState.Width; x++)
        {
            if (BoardState.Tiles[x, y] != value) return false;
            value = (value + 1) % max;
        }

        return true;
    }

    private (int x, int y) GetEmptyTile()
    {
        for (var y = 0; y < BoardState.Height; y++)
        for (var x = 0; x < BoardState.Width; x++)
            if (BoardState.Tiles[y, x] == 0)
                return (y, x);
        throw new InvalidOperationException("Board contains no empty space.");
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        GameState other = (GameState)obj;
        return GetHashCode() == other.GetHashCode();
    }

    public bool Equals(GameState other)
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
        sb.AppendLine($"Width: {BoardState.Width}, Height: {BoardState.Height}");
        sb.AppendLine("Tiles:");
        for (var y = 0; y < BoardState.Height; y++)
        {
            for (var x = 0; x < BoardState.Width; x++) sb.Append($"{BoardState.Tiles[x, y],3} ");
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