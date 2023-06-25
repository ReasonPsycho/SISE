using System.Text;

namespace SiSE;

public struct BoardState : IEquatable<BoardState>
{
    private static int _nextId = 1; // Static variable to store the next available ID
    public int Id { get; } // Unique ID for each instance
    public int[,] Tiles { get; }
    public int TilesHashCode { get;  }
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
        TilesHashCode = GetTilesHashCode();
        EmptyTile = GetEmptyTile();

        // Assign the next available ID and increment the counter
        Id = _nextId++;
    }

    public BoardState(int[,] inputTiles, int y, int x, Direction direction)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        TilesHashCode = GetTilesHashCode();
        EmptyTile = (y, x);
        LastMove = direction;

        // Assign the next available ID and increment the counter
        Id = _nextId++;
    }

    public List<BoardState> GetNeighbours(Direction[] directions,Direction? lastMove)

    {
        var neighbours = new List<BoardState>();
        if (lastMove != null)
        {
            Direction? skip = IPuzzleSolver.Reverse(lastMove);
        
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
    
    public override bool Equals(object? obj)
    {
        return obj is BoardState other && Id == other.Id;
    }
    
    public bool Equals(BoardState other)
    {
        return Id == other.Id;;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    
    public int GetTilesHashCode()
    {
        unchecked
        {
            int hash = 17;
        
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    hash = hash * 23 + Tiles[i, j].GetHashCode();
                }
            }

            return hash;
        }
    }
    
    public bool CheckIfTilesAreSame( BoardState other)
    {
        if (this.TilesHashCode != other.TilesHashCode) return false;
           // All values are the same
        return true;
    }

    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"ID: {Id}");
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