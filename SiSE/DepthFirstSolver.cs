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

        var edge = new Stack<BoardState>();
        var visited = new HashSet<BoardState>();

        var maxDepth = 1;
        var encounteredStates = 1;
        var processedStates = 1;

        edge.Push(puzzle);
        visited.Add(puzzle);
        while (edge.Count > 0)
        {
            var current = edge.Pop();

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
                    Debug.WriteLine(GetPath(neighbor));
                    if (maxDepth < neighbor.Moves.Count) maxDepth = neighbor.Moves.Count;
                    if (neighbor.IsGoal())
                    {
                        var path = GetPath(neighbor);
                        solution.Path = path;
                        solution.PathLength = path.Length;
                        solution.MaxDepth = maxDepth;
                        solution.EncounteredStates = encounteredStates;
                        solution.ProcessedStates = processedStates;
                        return solution;
                    }

                    if (current.Moves.Count + 1 < _maxDepth)
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

    private string GetPath(BoardState endState)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < endState.Moves.Count; i++)
        {
            stringBuilder.Append( IPuzzleSolver.GetStringFromDirection(endState.Moves[i])); //Cannot be null thought
        }
        return stringBuilder.ToString();
    }
}