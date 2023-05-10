using System.Text;

namespace SiSE;

public class DepthFirstSolver : IPuzzleSolver
{
    private readonly int _maxDepth;
    private readonly Direction[] _neighborhoodOrder;

    public DepthFirstSolver(string neighborhoodOrder, int maxDepth)
    {
        _neighborhoodOrder = IPuzzleSolver.GetDirectionsOrder(neighborhoodOrder);
        _maxDepth = maxDepth;
    }

    public Solution? Solve(BoardState puzzle, params object[] parameters)
    {
        var solution = new Solution();
        
        if (puzzle.IsGoal())
        {
            // If the initial state is the goal, return an empty path
            solution.Path = "";
            solution.PathLength = 0;
            solution.MaxDepth = 0;
            solution.EncounteredStates = 1;
            solution.ProcessedStates = 1;

            return solution;
        }
        
        var egde = new Stack<(BoardState,int)>();
        var explored = new HashSet<BoardState>();
        var parentsDictionary = new Dictionary<BoardState, BoardState>(); // Map each state to its parent index

        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 1;

        egde.Push((puzzle,0));
        explored.Add(puzzle);

        while (egde.Count > 0)
        {
            (var current,var depth) = egde.Pop(); 
            if (maxDepth < depth)
            {
                maxDepth = depth;
            }

            if (maxDepth > _maxDepth)
            {
                return null;   //CHECK I don't know if returning here quickly will caouse errors
            }
            
            explored.Add(current);

            foreach (var neighbor in current.GetNeighbours(_neighborhoodOrder))
            {
                encounteredStates++;
                // Store the parent of each neighbor
                parentsDictionary.Add(neighbor,current);
                if (neighbor.IsGoal())
                {
                    var path = GetPath(neighbor,parentsDictionary);
                    solution.Path = path;
                    solution.PathLength = path.Length;
                    solution.MaxDepth = depth + 1;
                    solution.EncounteredStates = encounteredStates;
                    solution.ProcessedStates = processedStates + 1;
                    return solution;
                }

                if (!explored.Contains(neighbor))
                {
                    egde.Push((neighbor,depth + 1));
                    explored.Add(neighbor);
                }
            }
        }

        return null;
    }
    
    private string GetPath(BoardState endState,Dictionary<BoardState,BoardState> parentDictionary)
    {
        StringBuilder stringBuilder = new StringBuilder();
        BoardState currentState = endState;
        do
        {
            if (currentState.LastMove != null)
            {
                stringBuilder.Append(IPuzzleSolver.GetStringFromDirection((Direction)currentState.LastMove)); //Cannot be null thought
            }
            else
            {
                break;
            }
        } while (parentDictionary.TryGetValue(currentState, out currentState));

        return stringBuilder.ToString();
    }
}