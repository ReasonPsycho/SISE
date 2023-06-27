namespace SiSE;

public struct Node: IComparable<Node>
{
    public BoardState BoardState;
    bool processed = false;

    public int FScore = 0;
    public  int HScore = 0;
    public  int GScore = 0;
    public  Direction? LastMove;

    public Node(int[,] inputTiles)
    {
        BoardState.Width = inputTiles.GetLength(0);
        BoardState.Height = inputTiles.GetLength(1);
        BoardState.Tiles = inputTiles;
        BoardState.EmptyTile = BoardState.GetEmptyTile();
    }

    public Node(int[,] inputTiles, int y, int x,int gScore,HeuristicMethod heuristicMethod,Direction direction)
    {
        BoardState.Width = inputTiles.GetLength(0);
        BoardState.Height = inputTiles.GetLength(1);
        BoardState.Tiles = inputTiles;
        BoardState.EmptyTile = (y, x);
        GScore = gScore;
        Heuristic(heuristicMethod);
        FScore = gScore + HScore;
        LastMove = direction;
    }

    public List<Node> GetNeighbours(HeuristicMethod heuristicMethod)

    {
        var neighbours = new List<Node>();
        foreach (var direction in Enum.GetValues<Direction>())
        {
            var state = Move(direction,heuristicMethod);
            if (state != null) neighbours.Add((Node)state);
        }
        return neighbours;
    }
    
    public Node? Move(Direction direction,HeuristicMethod heuristicMethod)
    {
        (var emptyX, var emptyY) = BoardState.EmptyTile;

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
        return new Node(newTiles, moveX, moveY,GScore + 1,heuristicMethod,direction);
    }
    
    public void Heuristic(HeuristicMethod heuristicMethod)
    {
        if (heuristicMethod == HeuristicMethod.Hamming)
        {
            var distance = 0;

            for (var y = 0; y < BoardState.Height; y++)
            for (var x = 0; x < BoardState.Width; x++)
                if (BoardState.Tiles[x, y] != y * BoardState.Width + x + 1 &&
                    !(x == BoardState.Width - 1 && y == BoardState.Height - 1 && BoardState.Tiles[x, y] == 0))
                    distance++;

            HScore = distance;
        }
        else // Manhattan
        {
            var distance = 0;

            for (var y = 0; y < BoardState.Height; y++)
            for (var x = 0; x < BoardState.Width; x++)
            {
                var value = BoardState.Tiles[x, y];

                if (value != 0)
                {
                    var targetX = (value - 1) % BoardState.Width;
                    var targetY = (value - 1) / BoardState.Width;

                    distance += Math.Abs(x - targetX) + Math.Abs(y - targetY);
                }
            }

            HScore = distance;
        }
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Node other = (Node)obj;

        return BoardState.Equals(other.BoardState);
    }


    public bool Equals(Node other)
    {
        return BoardState.Equals(other.BoardState);
    }

    public override int GetHashCode()
    {
        return BoardState.GetHashCode();
    }
    
    public int CompareTo(Node other)
    {
        return FScore.CompareTo(other.FScore);
    }
}