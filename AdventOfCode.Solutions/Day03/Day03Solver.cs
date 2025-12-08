using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Day03;

public class Day03Tests
{
    private readonly Day03Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day03Solver : SolverBase
{
    // List<??> _data;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        int sum = 0;
        foreach (var line in data)
        {
            var matches = Regex.Matches(line, @"mul\((\d+),(\d+)\)");
            foreach (Match match in matches)
            {
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                sum += a * b;
            }
        }

        return $"{sum}";
    }

    protected override object Solve2(List<string> data)
    {
        int sum = 0;
        bool doIt = true;
        foreach (var line in data)
        {
            var matches = Regex.Matches(line, @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");
            foreach (Match match in matches)
            {
                switch (match.Value)
                {
                    case "do()":
                        doIt = true;
                        break;
                    case "don't()":
                        doIt = false;
                        break;
                    default:
                        if (doIt)
                        {
                            var a = int.Parse(match.Groups[1].Value);
                            var b = int.Parse(match.Groups[2].Value);
                            sum += a * b;
                        }
                        break;
                }
            }
        }

        return $"{sum}";
    }
}