namespace AdventOfCode.Solutions.Common;

public abstract class SolverBase
{
    private string DayDirectory =>
        $@"{AppContext.BaseDirectory[..AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal)]}\{GetType().Name[..5]}\";

    private List<string> Load(string inputFileName)
        => File.ReadAllLines($"{DayDirectory}{inputFileName}").ToList();

    private string Save(string outputFileName, object? data)
    {
        var dataStr = data?.ToString() ?? "";
        File.WriteAllText($"{DayDirectory}{outputFileName}", dataStr);
        return dataStr;
    }

    public string ExecutePuzzle1(string inputFileName = "input.txt", string outputFileName = "output1.txt")
    {
        //Parse(Load(inputFileName));
        return Save(outputFileName, Solve1(Load(inputFileName)));
    }

    public string ExecutePuzzle2(string inputFileName = "input.txt", string outputFileName = "output2.txt")
    {
        //Parse(Load(inputFileName));
        return Save(outputFileName, Solve2(Load(inputFileName)));
    }

    public async Task ExecuteExample1(string expectedResult)
    {
        //Parse(Load("Example.txt"));
        await Assert.That(expectedResult).IsEqualTo(Solve1(Load("Example.txt"))?.ToString());
    }

    public async Task ExecuteExample2(string expectedResult)
    {
        //Parse(Load("Example.txt"));
        await Assert.That(expectedResult).IsEqualTo(Solve2(Load("Example.txt"))?.ToString());
    }

    protected abstract void Parse(List<string> data);

    protected abstract object Solve1(List<string> data);

    protected abstract object Solve2(List<string> data);
}