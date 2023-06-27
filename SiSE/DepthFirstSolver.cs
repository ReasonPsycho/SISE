using System.Diagnostics;

namespace SiSE;

public class DepthFirstSolver : IPuzzleSolver
{
    private readonly int _maxDepth;
    private readonly Direction[] _neighborhoodOrder;

    public DepthFirstSolver(string neighborhoodOrder, int maxDepth)
    {
        _neighborhoodOrder = IPuzzleSolver.GetDirectionsOrder(neighborhoodOrder);
        for (int i = 0; i < 4; i++)
        {
            _neighborhoodOrder[i] = (Direction)IPuzzleSolver.Reverse(_neighborhoodOrder[i]);
        }
        _maxDepth = maxDepth;
    }

    public Solution? Solve(GameState puzzle, params object[] parameters)
    {
        var solution = new Solution();
        var edge = new Stack<GameState>();
        var visited = new HashSet<GameState>();
        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 0;

        edge.Push(puzzle);
        while (edge.Count > 0)
        {
            var current = edge.Pop();
            var currentDepth = current.Moves.Count;
            if (maxDepth < current.Moves.Count) maxDepth = currentDepth;

            if (current.BoardState.IsGoal())
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

            foreach (var neighbor in neighbors)
                if (!visited.Contains(neighbor))
                {
                    encounteredStates++;

                    Debug.WriteLine("-----" + "NEIGHBOR" + "-----");
                    Debug.WriteLine("-----" + neighbor.Moves.Count + "-----");
                    Debug.WriteLine(neighbor.ToString());
                    Debug.WriteLine(neighbor.GetPath());

                    if (currentDepth + 1 <= _maxDepth)
                    {
                        visited.Add(neighbor);
                        edge.Push(neighbor);
                    }
                }

            processedStates++;
        }

        return null;
    }
}