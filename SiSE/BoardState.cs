using System.Text;

namespace SiSE;

public struct BoardState
{
    public int[,] Tiles { get; set; }
    public int Width { get; }
    public int Height { get; }
    public (int y, int x) EmptyTile { get; set; }
    public Direction? LastMove { get; set; }

    // Constructor to create a new board state with given width and height
    public BoardState(int[,] inputTiles)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        EmptyTile = GetEmptyTile();
    }

    public BoardState(int[,] inputTiles, int y, int x, Direction direction)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        EmptyTile = (y, x);
        LastMove = direction;
    }

    // Check if two board states are equal
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false; // objects are not of the same type

        var other = (BoardState)obj;

        // Here we can compare the properties of both objects using the 'other' instance
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            if (Tiles[y, x] != other.Tiles[y, x])
                return false;

        return true;
    }

    public List<BoardState> GetNeighbours(Direction[] directions)

    {
        var neighbours = new List<BoardState>();

        foreach (var direction in directions)
        {
            var state = Move(direction);
            if (state != null) neighbours.Add((BoardState)state);
        }

        return neighbours;
    }

    public List<BoardState> GetNeighbours()

    {
        Direction[] directions = { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
        var neighbours = new List<BoardState>();

        foreach (var direction in directions)
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

        return new BoardState(newTiles, moveX, moveY, direction);
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

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            var hash = 17;
            hash = hash * 23 + Width.GetHashCode();
            hash = hash * 23 + Height.GetHashCode();
            hash = hash * 23 + EmptyTile.x.GetHashCode();
            hash = hash * 23 + EmptyTile.y.GetHashCode();

            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                hash = hash * 23 + Tiles[y, x].GetHashCode();

            return hash;
        }
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
        sb.AppendLine($"LastMove: {LastMove}");

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