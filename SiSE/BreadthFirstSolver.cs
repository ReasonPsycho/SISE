using System.Diagnostics;
using System.Text;

namespace SiSE;

public class BreadthFirstSolver : IPuzzleSolver
{
    private readonly Direction[] _neighborhoodOrder;

    public BreadthFirstSolver(string neighborhoodOrder)
    {
        _neighborhoodOrder = IPuzzleSolver.GetDirectionsOrder(neighborhoodOrder);
    }

    public Solution? Solve(BoardState puzzle, params object[] parameters)
    {
        Debug.WriteLine("Solution - BreadthFirstSolver ");
        Debug.WriteLine(puzzle.ToString());
        if (puzzle.IsGoal())
            return new Solution
            {
                Path = "",
                PathLength = 0,
                EncounteredStates = 1,
                ProcessedStates = 1,
                MaxDepth = 0
            };

        var queue = new Queue<(BoardState boardState, int depth)>();
        var visited = new HashSet<BoardState>();
        var parentsDictionary = new Dictionary<BoardState, BoardState>(); // Map each state to its parent index

        queue.Enqueue((puzzle, 0));
        visited.Add(puzzle);
        var maxDepth = 1;
        var encounteredStates = 1;
        var processedStates = 1;

        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();
            visited.Add(current);
            maxDepth = Math.Max(maxDepth, depth);
            foreach (var neighbor in  current.GetNeighbours(_neighborhoodOrder,current.LastMove))
            {
                encounteredStates++;
                if (!visited.Any(v => v.CheckIfTilesAreSame(neighbor)))
                {
                    parentsDictionary.Add(neighbor, current);
                    processedStates++;
                    Debug.WriteLine("-----" + depth + "-----");
                    Debug.WriteLine(neighbor.ToString());
                    Debug.WriteLine(GetPath(neighbor, parentsDictionary));
                    if (neighbor.IsGoal())
                    {
                        var path = GetPath(neighbor, parentsDictionary);
                        return new Solution
                        {
                            Path = path.Reverse(),
                            PathLength = path.Length,
                            EncounteredStates = encounteredStates,
                            ProcessedStates = processedStates,
                            MaxDepth = maxDepth
                        };
                    }

                    if (!queue.Any(item => Equals(item.boardState, neighbor))) queue.Enqueue((neighbor, depth + 1));
                }
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