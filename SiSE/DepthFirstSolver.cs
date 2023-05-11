using System.Diagnostics;
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
        Debug.WriteLine(puzzle.ToString());
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

        var egde = new Stack<(BoardState boardState, int depth)>();
        var parentsDictionary = new Dictionary<BoardState, BoardState>(); // Map each state to its parent index

        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 1;

        egde.Push((puzzle, 0));

        while (egde.Count > 0)
        {
            var (current, depth) = egde.Pop();
            if (maxDepth < depth) maxDepth = depth;


            foreach (var neighbor in current.GetNeighbours(_neighborhoodOrder))
            {
                encounteredStates++;
                // Store the parent of each neighbor
                parentsDictionary.TryAdd(neighbor, current);
                Debug.WriteLine("-----" + depth + "-----");
                Debug.WriteLine(neighbor.ToString());
                Debug.WriteLine(GetPath(neighbor, parentsDictionary));
                if (neighbor.IsGoal())
                {
                    var path = GetPath(neighbor, parentsDictionary);
                    solution.Path = path.Reverse();
                    solution.PathLength = path.Length;
                    solution.MaxDepth = depth + 1;
                    solution.EncounteredStates = encounteredStates;
                    solution.ProcessedStates = processedStates + 1;
                    return solution;
                }

                if (depth + 1 < _maxDepth)
                    egde.Push((neighbor,
                        depth + 1)); //It dosn't even check if it's aleready solved but whatever it works
                processedStates++;
            }
        }

        return null;
    }

    private string GetPath(BoardState endState, Dictionary<BoardState, BoardState> parentDictionary)
    {
        var stringBuilder = new StringBuilder();
        var currentState = endState;
        do
        {
            if (currentState.LastMove != null)
                stringBuilder.Append(
                    IPuzzleSolver.GetStringFromDirection((Direction)currentState.LastMove)); //Cannot be null thought
            else
                break;
        } while (parentDictionary.TryGetValue(currentState, out currentState));

        return stringBuilder.ToString();
    }
}