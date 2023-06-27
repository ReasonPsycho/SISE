using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class UltimateTest2x2
{
    private readonly string extraInformationFile =
        Path.Combine(TestContext.CurrentContext.TestDirectory, "extraInformation.txt");

    private readonly string orginalFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "input.txt");
    private readonly string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output.txt");

    [SetUp]
    public void Setup()
    {
        var lines = new List<string>();
        var firstLine = "2 2";
        lines.Add(firstLine);

        var secondLine = "3 2";
        lines.Add(secondLine);

        var thirdLine = "1 0";
        lines.Add(thirdLine);

        File.WriteAllLines(orginalFile, lines);
        using (File.Create(outputFile))
        {
        }

        using (File.Create(extraInformationFile))
        {
        }
    }

    [Test]
    [Timeout(60000)]
    public void DFS()
    {
        string[] args = { "dfs", "LRUD", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    [Test]
    [Timeout(60000)]
    public void BFS()
    {
        string[] args = { "bfs", "LRUD", orginalFile, outputFile, extraInformationFile };
        Console.WriteLine("Test started...");
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }


    [Test]
    [Timeout(60000)]
    public void AStar()
    {
        string[] args = { "astar", "hamn", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }
}