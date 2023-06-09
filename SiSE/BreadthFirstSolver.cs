﻿using System.Diagnostics;
using System.Text;

namespace SiSE;

public class BreadthFirstSolver : IPuzzleSolver
{
    private readonly Direction[] _neighborhoodOrder;

    public BreadthFirstSolver(string neighborhoodOrder)
    {
        _neighborhoodOrder = IPuzzleSolver.GetDirectionsOrder(neighborhoodOrder);
    }

    public Solution? Solve(GameState puzzle, params object[] parameters)
    {
        Debug.WriteLine("Solution - BreadthFirstSolver ");
        Debug.WriteLine(puzzle.ToString());
        if (puzzle.BoardState.IsGoal())
            return new Solution
            {
                Path = "",
                PathLength = 0,
                EncounteredStates = 1,
                ProcessedStates = 0,
                MaxDepth = 0
            };

        var queue = new Queue<GameState>();
        var visited = new HashSet<GameState>();

        queue.Enqueue(puzzle);
        var maxDepth = 0;
        var encounteredStates = 1;
        var processedStates = 1;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current);
            var neighbors = current.GetNeighbours(_neighborhoodOrder);
            Debug.WriteLine("-----" + "CURRENT" + "-----");
            Debug.WriteLine("-----" + current.Moves.Count + "-----");
            Debug.WriteLine(current.ToString());
            Debug.WriteLine(GetPath(current));
            foreach (var neighbor in neighbors)
            {
                maxDepth = Math.Max(maxDepth, neighbor.Moves.Count);
                if (neighbor.BoardState.IsGoal())
                {
                    Debug.WriteLine("-----" + "NEIGHBOR" + "-----");
                    Debug.WriteLine("-----" + current.Moves.Count + "-----");
                    Debug.WriteLine(current.ToString());
                    Debug.WriteLine(GetPath(current));
                    var path = GetPath(neighbor);
                    return new Solution
                    {
                        Path = path,
                        PathLength = path.Length,
                        EncounteredStates = visited.Count,
                        ProcessedStates = processedStates,
                        MaxDepth = maxDepth
                    };
                }

                if (!visited.Contains(neighbor))
                {
                    encounteredStates++;
                    if (!queue.Contains(neighbor)) queue.Enqueue(neighbor);
                }
            }

            processedStates++;
        }

        return null;
    }

    private string GetPath(GameState endState)
    {
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < endState.Moves.Count; i++)
            stringBuilder.Append(IPuzzleSolver.GetStringFromDirection(endState.Moves[i])); //Cannot be null thought
        return stringBuilder.ToString();
    }
}