using SiSE;

public class BoardStateSubclass
{
    private BoardState _boardState;

    public int[,] Tiles => _boardState.Tiles;
    public int Width => _boardState.Width;
    public int Height => _boardState.Height;
    public int fCost;
    public (int y, int x) EmptyTile
    {
        get { return _boardState.EmptyTile; }
        set { _boardState.EmptyTile = value; }
    }

    public List<Direction> Moves
    {
        get { return _boardState.Moves; }
        set { _boardState.Moves = value; }        
    }

    public BoardStateSubclass(int[,] inputTiles)
    {
        _boardState = new BoardState(inputTiles);
    }

    public List<BoardState> GetNeighbours(Direction[] directions)
    {
        return _boardState.GetNeighbours(directions);
    }

    public List<BoardState> GetNeighbours()
    {
        return _boardState.GetNeighbours();
    }

    public BoardState Move(Direction direction)
    {
        return (BoardState)_boardState.Move(direction);
    }

    public bool IsGoal()
    {
        return _boardState.IsGoal();
    }

    public string GetPath()
    {
        return _boardState.GetPath();
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (BoardStateSubclass)obj;
        return _boardState.GetHashCode() == other._boardState.GetHashCode();
    }

    public bool Equals(BoardStateSubclass other)
    {
        return _boardState.GetHashCode() == other._boardState.GetHashCode();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            // Add hash code of each tile value
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    hash = hash * 31 + Tiles[y, x].GetHashCode();
                }
            }

            return hash;
        }
    }

    public override string ToString()
    {
        return _boardState.ToString();
    }
    
    public static explicit operator BoardState(BoardStateSubclass subclass)
    {
        return new BoardState(
            subclass.Tiles,
            subclass.EmptyTile.y,
            subclass.EmptyTile.x,
            subclass.Moves,
            subclass.GetHashCode()
        );
    }
    
    public static explicit operator BoardStateSubclass(BoardState baseClass)
    {
        return new BoardStateSubclass(baseClass.Tiles)
        {
            EmptyTile = baseClass.EmptyTile,
            Moves = baseClass.Moves
        };
    }
}

