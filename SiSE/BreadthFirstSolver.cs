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
                ProcessedStates = 0,
                MaxDepth = 0
            };

        var queue = new Queue<BoardState>();
        var visited = new HashSet<BoardState>();

        queue.Enqueue(puzzle);
        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 1;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current);
            var neighbors = current.GetNeighbours(_neighborhoodOrder);
            encounteredStates += neighbors.Count;
            foreach (var neighbor in neighbors)
            {
                Debug.WriteLine("-----" + current.Moves.Count + "-----");
                Debug.WriteLine(neighbor.ToString());
                Debug.WriteLine(GetPath(neighbor));
                maxDepth = Math.Max(maxDepth, neighbor.Moves.Count);
                if (neighbor.IsGoal())
                {
                    var path = GetPath(neighbor);
                    return new Solution
                    {
                        Path = path,
                        PathLength = path.Length,
                        EncounteredStates = encounteredStates,
                        ProcessedStates = processedStates,
                        MaxDepth = maxDepth
                    };
                }

                if (visited.Contains(neighbor) || queue.Contains(neighbor)) continue;
                queue.Enqueue(neighbor);
            }
            processedStates++;
        }

        return null;
    }

    private string GetPath(BoardState endState)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < endState.Moves.Count; i++)
        {
            stringBuilder.Append(IPuzzleSolver.GetStringFromDirection(endState.Moves[i])); //Cannot be null thought
        }
        return stringBuilder.ToString();
    }
}