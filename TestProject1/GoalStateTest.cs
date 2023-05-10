using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class GoalStateTest
{
    string orginalFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "input.txt");
    string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output.txt");
    string extraInformationFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "extraInformation.txt");
    [SetUp]
    public void Setup()
    {
        List<string> lines = new List<string>();
        string firstLine = "4 4";
        lines.Add(firstLine);
        
        string secondLine = "0 1 2 3";
        lines.Add(secondLine);

        string thirdLine = "4 5 6 7";
        lines.Add(thirdLine);

        string forthLine = "8 9 10 11";
        lines.Add(forthLine);

        string fifthLine = "12 13 14 15";
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
        string[] args = new string[] {"dfs","LRUD",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }
    
    [Test]
    public void BFS()
    {
        string[] args = new string[] {"bfs","LRUD",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    [Test]
    public void AStar()
    {
        string[] args = new string[] {"astar","hamn",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

}