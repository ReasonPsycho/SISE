using System.Diagnostics;

namespace SiSE;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var algorithm = args[0]; // bfs, dfs, or astr
        var heuristic = args[1]; // LRUD, hamm, or manh
        var inputFile = args[2]; // name of input file
        var outputPath = args[3]; // name of output file for solution
        string? infoPath = null;
        if (args.Length > 3) infoPath = args[4];

        GameState startState;

        // read input file
        using (var sr = File.OpenText(inputFile))
        {
            var firstLine = sr.ReadLine().Split();
            var rows = int.Parse(firstLine[0]);
            var cols = int.Parse(firstLine[1]);
            var puzzle = new int[rows, cols];
            for (var i = 0; i < rows; i++)
            {
                var row = sr.ReadLine().Split();
                for (var j = 0; j < cols; j++) puzzle[j, i] = int.Parse(row[j]);
            }

            startState = new GameState(puzzle);
        }

        // initialize variables for search algorithm
        IPuzzleSolver solver;
        var timer = new Stopwatch();
        var maxDepth = 20; // for dfs

        // perform search algorithm
        switch (algorithm)
        {
            case "bfs":
                solver = new BreadthFirstSolver(heuristic);
                break;
            case "dfs":
                solver = new DepthFirstSolver(heuristic, maxDepth);
                break;
            case "astr":
                solver = new AStarSolver(heuristic);
                break;
            default:
                Console.WriteLine("Invalid algorithm choice.");
                return;
        }

        timer.Start();
        var result = solver.Solve(startState, heuristic);
        timer.Stop();

        // write solution to output file
        using (var sw = File.CreateText(outputPath))
        {
            sw.WriteLine(result?.PathLength ?? -1);
            foreach (var move in result?.Path ?? Array.Empty<char>()) sw.Write(move);
            sw.WriteLine();
        }

        if (result != null && infoPath != null)
            // write extra information to output file
            using (var sw = File.CreateText(infoPath))
            {
                sw.WriteLine(result?.PathLength);
                sw.WriteLine(result?.EncounteredStates);
                sw.WriteLine(result?.ProcessedStates);
                sw.WriteLine(result?.MaxDepth);
                sw.WriteLine(((double)(timer.ElapsedTicks / 10L) / 1000f).ToString("0.000"));
            }
    }
}