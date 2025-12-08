namespace AdventOfCode.Solutions.Day01;

public class Day01Tests
{
    private readonly Day01Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("1222801");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("22545250");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day01Solver : SolverBase
{
    protected override void Parse(List<string> data)
    {

    }

    protected override object Solve1(List<string> data)
    {
        List<int> left = [];
        List<int> right = [];

        foreach (var line in data)
        {
            var splittedLine = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(splittedLine[0]));
            right.Add(int.Parse(splittedLine[1]));
        }

        left.Sort();
        right.Sort();

        var totalDistance = left.Select((x, index) => Math.Abs(x - right[index])).Sum();
        return totalDistance;
    }

    protected override object Solve2(List<string> data)
    {
        Dictionary<int, int> leftNumbers = [];
        Dictionary<int, int> rightCounts = [];

        foreach (var line in data)
        {
            var splittedLine = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            var left = int.Parse(splittedLine[0]);
            var right = int.Parse(splittedLine[1]);

            leftNumbers.TryAdd(left, 1);

            if (!rightCounts.TryAdd(right, 1))
            {
                rightCounts[right]++;
            }
        }

        var total = rightCounts.Sum(x => x.Key * x.Value * leftNumbers.GetValueOrDefault(x.Key, 0));
        return total;
    }
}