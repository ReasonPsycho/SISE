using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class GoalStateTest
{
    private readonly string extraInformationFile =
        Path.Combine(TestContext.CurrentContext.TestDirectory, "extraInformation.txt");

    private readonly string orginalFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "input.txt");
    private readonly string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output.txt");

    [SetUp]
    public void Setup()
    {
        var lines = new List<string>();
        var firstLine = "4 4";
        lines.Add(firstLine);

        var secondLine = "1 2 3 4";
        lines.Add(secondLine);

        var thirdLine = "5 6 7 8";
        lines.Add(thirdLine);

        var forthLine = "9 10 11 12";
        lines.Add(forthLine);

        var fifthLine = "13 14 15 0";
        lines.Add(fifthLine);

        File.WriteAllLines(orginalFile, lines);
        using (File.Create(outputFile))
        {
        }

        using (File.Create(extraInformationFile))
        {
        }
    }

    [Test]
    public void DFS()
    {
        string[] args = { "dfs", "LRUD", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    [Test]
    public void BFS()
    {
        string[] args = { "bfs", "LRUD", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    [Test]
    public void AStar()
    {
        string[] args = { "astar", "hamn", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }
}