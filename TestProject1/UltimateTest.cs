using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class UltimateTest
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

        var thirdLine = "5 6 11 7";
        lines.Add(thirdLine);

        var forthLine = "9 10 8 0";
        lines.Add(forthLine);

        var fifthLine = "13 14 15 12";
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
        string[] args = { "dfs", "RDUL", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    [Test]
    [Timeout(60000)]
    public void BFS()
    {
        string[] args = { "bfs", "ULRD", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }


    [Test]
    [Timeout(60000)]
    public void AStar()
    {
        string[] args = { "astr", "hamn", orginalFile, outputFile, extraInformationFile };
        Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }
}