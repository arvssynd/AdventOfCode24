namespace AdventOfCode.Solutions.Day02;

public class Day02Tests
{
    private readonly Day02Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day02Solver : SolverBase
{
    // List<??> _data;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        int safeLinesCount = 0;

        foreach (var line in data)
        {
            var splittedLine = line.Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (IsLineSafe(splittedLine))
            {
                safeLinesCount++;
            }
        }

        return $"{safeLinesCount}";
    }

    private static bool IsLineSafe(List<int> line)
    {
        bool? isIncreasing = null;
        for (int i = 1; i < line.Count; i++)
        {
            var diff = line[i] - line[i - 1];

            var absDiff = Math.Abs(diff);
            if (absDiff is < 1 or > 3)
            {
                return false;
            }

            if (isIncreasing.HasValue)
            {
                if (diff > 0 && !isIncreasing.Value)
                {
                    return false;
                }

                if (diff < 0 && isIncreasing.Value)
                {
                    return false;
                }
            }
            else
            {
                isIncreasing = diff > 0;
            }
        }

        return true;
    }

    protected override object Solve2(List<string> data)
    {
        int safeLinesCount = 0;

        foreach (var line in data)
        {
            var splittedLine = line.Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (IsLineSafeAfterRemovingIndex(splittedLine))
            {
                safeLinesCount++;
            }
        }

        return $"{safeLinesCount}";
    }

    private static bool IsLineSafeAfterRemovingIndex(List<int> line, int index = 0)
    {
        if (index >= line.Count)
        {
            return false;
        }

        var copyList = new List<int>(line);
        copyList.RemoveAt(index);

        return IsLineSafe(copyList) || IsLineSafeAfterRemovingIndex(line, index + 1);
    }
}