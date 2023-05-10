using System.Text;

namespace SiSE;

public interface IPuzzleSolver
{
    Solution? Solve(BoardState puzzle, params object[] parameters);

    public static Direction GetDirectionFromString(string directionString)
    {
        switch (directionString)
        {
            case "U":
                return Direction.Up;
            case "D":
                return Direction.Down;
            case "L":
                return Direction.Left;
            case "R":
                return Direction.Right;
            default:
                throw new ArgumentException("Invalid direction string", nameof(directionString));
        }
    }
    
    public static string GetStringFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return "U";
            case Direction.Down:
                return "D";
            case Direction.Left:
                return "L";
            case Direction.Right:
                return "R";
            default:
                throw new ArgumentException("Invalid direction string", nameof(direction));
        }
    }

    public static Direction[] GetDirectionsOrder(string input)
    {
        if (input.Length != 4)
            throw new ArgumentException("Invalid input string", nameof(input));

        Direction[] directions = new Direction[4];
        
        for (int i = 0; i < 4; i++) { 
            directions[i] = GetDirectionFromString(input.Substring(i,1));
        }
        
        return directions;
    }
}

public struct Solution
{
    public IEnumerable<char> Path { get; set; } // sequence of movements needed to solve the puzzle
    public int PathLength { get; set; } // length of solution
    public int EncounteredStates { get; set; } // number of states encountered during search
    public int ProcessedStates { get; set; } // number of states processed during search
    public int MaxDepth { get; set; } // maximum depth of recursion reached
}