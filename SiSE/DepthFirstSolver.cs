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

        var edge = new Stack<(BoardState boardState, int depth)>();
        var visited = new HashSet<(BoardState, int depth)>();
        var parentsDictionary = new Dictionary<BoardState, BoardState>(); // Map each state to its parent index

        var maxDepth = 1;
        var encounteredStates = 1;
        var processedStates = 1;

        edge.Push((puzzle, 1));
        visited.Add((puzzle, 1));
        while (edge.Count > 0)
        {
            var (current, depth) = edge.Pop();
            if (maxDepth < depth) maxDepth = depth;

            var neighbors = current.GetNeighbours(_neighborhoodOrder,current.LastMove);
            neighbors.Reverse(); // THE last shall be the first!
            foreach (var neighbor in neighbors)
            {
                encounteredStates++;
                // Store the parent of each neighbor
                parentsDictionary.Add(neighbor, current);
                Debug.WriteLine("-----" + depth + "-----");
                Debug.WriteLine(neighbor.ToString());
                Debug.WriteLine(GetPath(neighbor, parentsDictionary));
                if (neighbor.IsGoal())
                {
                    var path = GetPath(neighbor, parentsDictionary);
                    solution.Path = path.Reverse();
                    solution.PathLength = path.Length;
                    solution.MaxDepth = depth;
                    solution.EncounteredStates = encounteredStates;
                    solution.ProcessedStates = processedStates + 1;
                    return solution;
                }

                if (depth < _maxDepth)
                {
                    if (!visited.Any(v => v.Item1.CheckIfTilesAreSame(neighbor)))
                    {
                        visited.Add((neighbor,
                            depth + 1));
                        edge.Push((neighbor,
                            depth + 1)); 
                    }
                }
                  
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