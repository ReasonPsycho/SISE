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
        if (heuristicMethod == "hamn")
            _heuristicMethod = HeuristicMethod.Hamming;
        else
            _heuristicMethod = HeuristicMethod.Manhattan;
    }

    public Solution? Solve(BoardState puzzle, params object[] parameters)
    {
        var encounteredStates = 0;
        var processedStates = 0;
        var maxDepth = 0;

        Debug.WriteLine(puzzle.ToString());

        // Priority queue to store the states to be explored
        var priorityQueue = new PriorityQueue<BoardState, int>();
        // Set to keep track of the explored states
        var closedSet = new HashSet<BoardState>();
        // Dictionary to keep track of the cost to reach a state from the initial state
        var gScore = new Dictionary<BoardState, int>();
        // Dictionary to keep track of the previous state in the optimal path
        var cameFrom = new Dictionary<BoardState, BoardState>();

        priorityQueue.Enqueue(puzzle, Heuristic(puzzle));
        gScore[puzzle] = 0;

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Dequeue();
            processedStates++;

            Debug.WriteLine("-----" + cameFrom.ToString() + "-----");
            Debug.WriteLine(current.ToString());
            //Debug.WriteLine(GetPath(neighbor,parentsDictionary));
            if (current.IsGoal())
            {
                StringBuilder solutionPath = new StringBuilder();
                var previousState = new BoardState();

                while (cameFrom.TryGetValue(current, out previousState))
                {
                    if (current.LastMove != null)
                        solutionPath.Append(IPuzzleSolver.GetStringFromDirection((Direction)current.LastMove));

                    current = previousState;
                }

                maxDepth = cameFrom.Count;

                return new Solution
                {
                    Path = solutionPath.ToString().Reverse(), //CHECK Not sure about that tbh
                    PathLength = solutionPath.Length,
                    EncounteredStates = encounteredStates,
                    ProcessedStates = processedStates,
                    MaxDepth = maxDepth
                };
            }

            if (!closedSet.Contains(current))
            {
                closedSet.Add(current);
                encounteredStates++;

                foreach (var direction in Enum.GetValues<Direction>())
                {
                    var neighbor = current.Move(direction);
                    
                    if(neighbor != null){
                    if (neighbor.Equals(current)) continue;

                    var tentativeGScore = gScore[current] + 1;

                    if (!gScore.TryGetValue((BoardState)neighbor, out var neighborGScore)) neighborGScore = int.MaxValue;

                    if (tentativeGScore < neighborGScore) //CHECK not sure if should be g + h buw thatever
                    {
                        cameFrom[(BoardState)neighbor] = current;
                        gScore[(BoardState)neighbor] = tentativeGScore;

                        var fScore = tentativeGScore + Heuristic((BoardState)neighbor);
                        priorityQueue.Enqueue((BoardState)neighbor, fScore);
                    }
                    }
                }
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