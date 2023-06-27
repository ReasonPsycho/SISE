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
    private Dictionary<Node, Node> parentDictionary;

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

        var priorityQueue = new List<Node>();
        parentDictionary = new Dictionary<Node, Node>();
        var closedSet = new HashSet<Node>();
        var initialNode = new Node(puzzle.BoardState.Tiles);

        priorityQueue.Add(initialNode);

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Min();
            priorityQueue.Remove(current);
            var currentDepth = current.GScore;

            closedSet.Add(current);
            priorityQueue.Remove(current);
            processedStates++;

            if (currentDepth > maxDepth) maxDepth = currentDepth;

            Debug.WriteLine("-----" + "CURRENT" + "-----");
            Debug.WriteLine("-----" + current.GScore + "-----");
            Debug.WriteLine(current.ToString());
            Debug.WriteLine(GetPath(current));

            if (current.BoardState.IsGoal())
                return new Solution
                {
                    Path = GetPath(current),
                    PathLength = currentDepth,
                    EncounteredStates = encounteredStates,
                    ProcessedStates = processedStates,
                    MaxDepth = maxDepth
                };

            var neighbours = current.GetNeighbours(_heuristicMethod);

            foreach (var neighbor in neighbours)
            {
                var n = neighbor;
                Debug.WriteLine(n.GScore);
                if (closedSet.Contains(n))
                    continue;

                Debug.WriteLine("-----" + "NEIGHBOR" + "-----");
                Debug.WriteLine("-----" + n.GScore + "-----");
                Debug.WriteLine(n.ToString());
                Debug.WriteLine(GetPath(current));

                encounteredStates++;

                if (parentDictionary.ContainsKey(n))
                {
                    var n2 = parentDictionary[n];
                    if (n.GScore < n2.GScore)
                    {
                        parentDictionary[n] = n2;
                        UpdateChildParentValues(n);
                    }
                }
                else
                {
                    parentDictionary.Add(n, current);
                }

                priorityQueue.Add(n);
            }
        }

        return null;
    }

    public string GetPath(Node goalNode)
    {
        var path = new List<Node>();
        var currentNode = goalNode;

        while (currentNode.GScore != 0)
        {
            path.Add(currentNode);
            currentNode = parentDictionary[currentNode];
        }

        path.Reverse();

        var sb = new StringBuilder();
        foreach (var node in path) sb.Append(IPuzzleSolver.GetStringFromDirection(((Direction)node.LastMove)!));

        return sb.ToString();
    }

    private void UpdateChildParentValues(Node parentNode)
    {
        foreach (var kvp in parentDictionary)
            if (kvp.Value.Equals(parentNode))
            {
                parentDictionary[kvp.Key] = parentNode;
                UpdateChildParentValues(kvp.Key);
            }
    }
}