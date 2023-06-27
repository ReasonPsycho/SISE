using System.Diagnostics;
using System.Text;

namespace SiSE;

public enum HeuristicMethod
{
    Hamming,
    Manhattan
}

public class AStarSolver : IPuzzleSolver
{
    private readonly HeuristicMethod _heuristicMethod;

    public AStarSolver(string heuristicMethod)
    {
        if (heuristicMethod == "hamm")
            _heuristicMethod = HeuristicMethod.Hamming;
        else
            _heuristicMethod = HeuristicMethod.Manhattan;
    }

    public Solution? Solve(GameState puzzle, params object[] parameters)
    {
        var encounteredStates = 1;
        var processedStates = 0;
        var maxDepth = 0;

        Debug.WriteLine(puzzle.ToString());

        // Priority queue to store the states to be explored
        var priorityQueue = new PriorityQueue<GameState, int>();
        // Set to keep track of the explored states
        var closedSet = new HashSet<BoardState>();
        // Dictionary to keep track of the cost to reach a state from the initial state
        priorityQueue.Enqueue(puzzle, Heuristic(puzzle.BoardState));

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Dequeue();
            var currentMoves = current.Moves.Count();
            Debug.WriteLine("-----" + current.GetPath() + "-----");
            Debug.WriteLine(current.ToString());
            //Debug.WriteLine(GetPath(neighbor,parentsDictionary));

            if (currentMoves > maxDepth)
            {
                maxDepth = currentMoves;
            }

            if (current.IsGoal())
            {
                return new Solution
                {
                    Path = current.GetPath(),
                    PathLength = current.Moves.Count(),
                    EncounteredStates = encounteredStates,
                    ProcessedStates = processedStates,
                    MaxDepth = maxDepth
                };
            }

            if (!closedSet.Contains(current.BoardState))
            {
                closedSet.Add(current.BoardState);
                var neighbours = current.GetNeighbours();
                foreach (var neighbor in neighbours)
                {
                    if (!closedSet.Contains(neighbor.BoardState))
                    {
                        encounteredStates++;
                        var fScore = currentMoves + 1 + Heuristic(neighbor.BoardState);
                        priorityQueue.Enqueue(neighbor, fScore);
                    }
                }

                processedStates++;
            }
        }

        return null;
    }

    // Heuristic function that calculates the estimated cost to reach the goal state from the current state
    private int Heuristic(BoardState state)
    {
        if (_heuristicMethod == HeuristicMethod.Hamming)
        {
            var distance = 0;

            for (var y = 0; y < state.Height; y++)
            for (var x = 0; x < state.Width; x++)
                if (state.Tiles[x, y] != y * state.Width + x + 1 &&
                    !(x == state.Width - 1 && y == state.Height - 1 && state.Tiles[x, y] == 0))
                    distance++;

            return distance;
        }
        else // Manhattan
        {
            var distance = 0;

            for (var y = 0; y < state.Height; y++)
            for (var x = 0; x < state.Width; x++)
            {
                var value = state.Tiles[x, y];

                if (value != 0)
                {
                    var targetX = (value - 1) % state.Width;
                    var targetY = (value - 1) / state.Width;

                    distance += Math.Abs(x - targetX) + Math.Abs(y - targetY);
                }
            }

            return distance;
        }
    }
}