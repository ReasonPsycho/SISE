using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class UltimateTest2x2
{
    string orginalFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "input.txt");
    string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output.txt");
    string extraInformationFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "extraInformation.txt");
    [SetUp]
    public void Setup()
    {
        List<string> lines = new List<string>();
        string firstLine = "2 2";
        lines.Add(firstLine);
        
        string secondLine = "3 2";
        lines.Add(secondLine);

        string thirdLine = "1 0";
        lines.Add(thirdLine);

        File.WriteAllLines(orginalFile, lines);
        using (File.Create(outputFile))
        {
        }
        using (File.Create(extraInformationFile))
        {
        }
    }

    [Test, Timeout(60000)]
    public void DFS()
    {
        string[] args = new string[] {"dfs","LRUD",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }
    
    [Test, Timeout(60000)]
    public void BFS()
    {
        string[] args = new string[] {"bfs","LRUD",orginalFile,outputFile,extraInformationFile};
        Console.WriteLine("Test started...");
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    
    [Test, Timeout(60000)]
    public void AStar()
    {
        string[] args = new string[] {"astar","hamn",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    } 

}