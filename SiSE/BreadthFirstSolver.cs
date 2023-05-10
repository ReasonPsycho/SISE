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
        if (puzzle.IsGoal())
            return new Solution
            {
                Path = "",
                PathLength = 0,
                EncounteredStates = 1,
                ProcessedStates = 1,
                MaxDepth = 0
            };

        var queue = new Queue<(BoardState boardState,int depth)>();
        var visited = new HashSet<BoardState>();
        var parentsDictionary = new Dictionary<BoardState, BoardState>(); // Map each state to its parent index

        queue.Enqueue((puzzle , 0));
        visited.Add(puzzle);
        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 1;

        while (queue.Count > 0)
        {
             (var current ,var depth)  = queue.Dequeue();
            visited.Add(current);
            maxDepth = Math.Max(maxDepth, depth);

            foreach (var neighbor in current.GetNeighbours(_neighborhoodOrder))
            {
                encounteredStates++;
                parentsDictionary.Add(neighbor,current);
                
                if (!visited.Contains(neighbor))
                {
                    processedStates++;
                    if (neighbor.IsGoal())
                    {
                        var path = GetPath(neighbor,parentsDictionary);
                        return new Solution
                        {
                            Path = path,
                            PathLength = path.Length,
                            EncounteredStates = encounteredStates,
                            ProcessedStates = processedStates + 1,
                            MaxDepth = maxDepth
                        };
                    }

                    if (!queue.Any(item => Equals(item.boardState, neighbor)))
                    {
                        queue.Enqueue((neighbor,depth + 1));
                    }
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