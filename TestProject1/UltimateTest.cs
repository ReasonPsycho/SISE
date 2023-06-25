using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using SiSE;

namespace TestProject1;

public class UltimateTest
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
        
        string secondLine = "1 2 3 4";
        lines.Add(secondLine);

        string thirdLine = "5 6 11 7";
        lines.Add(thirdLine);

        string forthLine = "9 10 8 0";
        lines.Add(forthLine);

        string fifthLine = "13 14 15 12";
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
        string[] args = new string[] {"dfs","RDUL",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }
    
    [Test, Timeout(60000)]
    public void BFS()
    {
        string[] args = new string[] {"bfs","ULRD",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    }

    
    [Test, Timeout(60000)]
    public void AStar()
    {
        string[] args = new string[] {"astr","hamn",orginalFile,outputFile,extraInformationFile};
        SiSE.Program.Main(args);
        Console.Write(File.ReadAllText(outputFile) + File.ReadAllText(extraInformationFile));
    } 

}