namespace SiSE;

public struct BoardState
{
    public int[,] Tiles { get; set; }
    public int Width { get; }
    public int Height { get; }
    public (int x, int y) EmptyTile { get; set; }
    public Direction? LastMove { get; set; }

    // Constructor to create a new board state with given width and height
    public BoardState(int[,] inputTiles)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        EmptyTile = GetEmptyTile();
    }
    
    public BoardState(int[,] inputTiles,int x,int y,Direction direction)
    {
        Width = inputTiles.GetLength(0);
        Height = inputTiles.GetLength(1);
        Tiles = inputTiles;
        EmptyTile = (x,y);
        LastMove = direction;
    }

    // Check if two board states are equal
    public bool Equals(BoardState other)
    {
        if (Width != other.Width || Height != other.Height)
            return false;

        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            if (Tiles[x, y] != other.Tiles[x, y])
                return false;

        return true;
    }

    public List<BoardState> GetNeighbours(Direction[] directions)

    {
        var neighbours = new List<BoardState>();

        foreach (var direction in directions)
        {
            var state = Move(direction);
            if (state != null)
            {
                neighbours.Add((BoardState)state);
            }
        }
        return neighbours;
    }
    
    public List<BoardState> GetNeighbours()

    {
        Direction[] directions = new[] { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
        var neighbours = new List<BoardState>();

        foreach (var direction in directions)
        {
            var state = Move(direction);
            if (state != null)
            {
                neighbours.Add((BoardState)state);
            }
        }
        return neighbours;
    }

    // Move a tile in the given direction (if possible), returning a new board state
    public BoardState? Move(Direction direction)
    {
         (int emptyX,int emptyY) = EmptyTile;

         // Check if the move is possible
        var moveX = emptyX;
        var moveY = emptyY;

        switch (direction)
        {
            case Direction.Up:
                if (emptyY > 0)
                {
                    moveY--;
                }
                else
                {
                    return null;
                }
                break;
            case Direction.Down:
                if (emptyY < Height - 1)
                {
                    moveY++;
                }
                else
                {
                    return null;
                }
                break;
            case Direction.Left:
                if (emptyX > 0)
                {
                    moveX--;
                }
                else
                {
                    return null;
                }
                break;
            case Direction.Right:
                if (emptyX < Width - 1)
                {
                    moveX++;
                }
                else
                {
                    return null;
                }
                break;
        }

        // Create a new board state with the tile moved
        var newTiles = (int[,])Tiles.Clone();
        newTiles[emptyX, emptyY] = Tiles[moveX, moveY];
        newTiles[moveX, moveY] = 0;

        return new BoardState(newTiles,moveX,moveY,direction);
    }

    public bool IsGoal()
    {
        var value = 0;

        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            if (Tiles[x, y] != x*Width + y) return false;

            value++;
        }

        return true;
    }

    private (int x, int y) GetEmptyTile()
    {
   
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (Tiles[x, y] == 0)
                {
                    return (x, y);
                }
            throw new InvalidOperationException("Board contains no empty space.");
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}