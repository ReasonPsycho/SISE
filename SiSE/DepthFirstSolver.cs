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
        var edge = new Stack<BoardState>();
        var visited = new HashSet<BoardState>();
        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 0;

        edge.Push(puzzle);
        while (edge.Count > 0)
        {
            var current = edge.Pop();
            var currentDepth = current.Moves.Count;
            if (maxDepth < current.Moves.Count) maxDepth = currentDepth;
            
            if (current.IsGoal())
            {
                var path = current.GetPath();
                solution.Path = path;
                solution.PathLength = path.Length;
                solution.MaxDepth = maxDepth;
                solution.EncounteredStates = encounteredStates;
                solution.ProcessedStates = processedStates;
                return solution;
            }
            
            visited.Add(current);
            
            var neighbors = current.GetNeighbours(_neighborhoodOrder);

            neighbors.Reverse(); // THE last shall be the first!
            encounteredStates += neighbors.Count;

            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    // Store the parent of each neighbor
                    Debug.WriteLine("-----" + current.Moves.Count + "-----");
                    Debug.WriteLine(neighbor.ToString());
                    Debug.WriteLine(neighbor.GetPath());

                    if (currentDepth + 1 <= _maxDepth)
                    {
                        visited.Add(neighbor);
                        edge.Push(neighbor);
                    }
                }
            }
            processedStates++;

        }

        return null;
    }


}