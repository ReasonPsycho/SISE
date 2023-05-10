using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class UltimateTest3x3
{
    string orginalFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "input.txt");
    string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output.txt");
    string extraInformationFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "extraInformation.txt");
    [SetUp]
    public void Setup()
    {
        List<string> lines = new List<string>();
        string firstLine = "3 3";
        lines.Add(firstLine);
        
        string secondLine = "4 1 2";
        lines.Add(secondLine);

        string thirdLine = "3 0 5";
        lines.Add(thirdLine);

        string forthLine = "6 7 8";
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

    /*
    [Test]
    public void AStar()
    {
        string[] args = new string[] {"astar","hamn",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    } 
*/
}