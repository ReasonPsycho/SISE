using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class UltimateTest3x3
{
    private readonly string extraInformationFile =
        Path.Combine(TestContext.CurrentContext.TestDirectory, "extraInformation.txt");

    private readonly string orginalFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "input.txt");
    private readonly string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output.txt");

    [SetUp]
    public void Setup()
    {
        var lines = new List<string>();
        var firstLine = "3 3";
        lines.Add(firstLine);

        var secondLine = "3 1 2";
        lines.Add(secondLine);

        var thirdLine = "4 5 0";
        lines.Add(thirdLine);

        var forthLine = "6 7 8";
        lines.Add(forthLine);


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