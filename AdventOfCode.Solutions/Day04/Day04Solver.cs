namespace AdventOfCode.Solutions.Day04;

public class Day04Tests
{
    private readonly Day04Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day04Solver : SolverBase
{
    // List<??> _data;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        var sum = 0;
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[0].Length; j++)
            {
                sum += SearchWord(data, i, j);
            }
        }

        return $"{sum}";
    }

    private static int SearchWord(List<string> data, int i, int j)
    {
        if (data[i][j] is not 'X')
        {
            return 0;
        }

        int count = 0;

        // su
        if (i - 3 >= 0 && data[i - 1][j] == 'M' && data[i - 2][j] == 'A' && data[i - 3][j] == 'S')
        {
            count++;
        }

        // giù
        if (i + 3 < data.Count && data[i + 1][j] == 'M' && data[i + 2][j] == 'A' && data[i + 3][j] == 'S')
        {
            count++;
        }

        // sinistra
        if (j - 3 >= 0 && data[i][j - 1] == 'M' && data[i][j - 2] == 'A' && data[i][j - 3] == 'S')
        {
            count++;
        }

        // right
        if (j + 3 < data[0].Length && data[i][j + 1] == 'M' && data[i][j + 2] == 'A' && data[i][j + 3] == 'S')
        {
            count++;
        }

        // sinistra-su
        if (i - 3 >= 0 && j - 3 >= 0 && data[i - 1][j - 1] == 'M' && data[i - 2][j - 2] == 'A' && data[i - 3][j - 3] == 'S')
        {
            count++;
        }

        // destra-su    
        if (i - 3 >= 0 && j + 3 < data[0].Length && data[i - 1][j + 1] == 'M' && data[i - 2][j + 2] == 'A' && data[i - 3][j + 3] == 'S')
        {
            count++;
        }

        // sinistra-giu
        if (i + 3 < data.Count && j - 3 >= 0 && data[i + 1][j - 1] == 'M' && data[i + 2][j - 2] == 'A' && data[i + 3][j - 3] == 'S')
        {
            count++;
        }

        // destra-giu
        if (i + 3 < data.Count && j + 3 < data[0].Length && data[i + 1][j + 1] == 'M' && data[i + 2][j + 2] == 'A' && data[i + 3][j + 3] == 'S')
        {
            count++;
        }

        return count;
    }

    protected override object Solve2(List<string> data)
    {
        // ricerca mas a forma di x, il centro diventa la A
        var count = 0;
        for (int i = 0; i < data.Count - 1; i++)
        {
            for (int j = 0; j < data[0].Length - 1; j++)
            {
                if (data[i][j] is not 'A')
                {
                    continue;
                }

                if (
                    (
                        (i > 0 && j > 0 && data[i - 1][j - 1] == 'M' && data[i + 1][j + 1] == 'S') || // MAS da sinistra sopra a destra sotto
                        (i > 0 && j > 0 && data[i - 1][j - 1] == 'S' && data[i + 1][j + 1] == 'M') // SAM da sinistra sopra a destra sotto
                    ) &&
                    (
                        (i > 0 && j < data[0].Length - 1 && data[i - 1][j + 1] == 'M' && data[i + 1][j - 1] == 'S') || // MAS da destra sopra a sinistra sotto
                        (i > 0 && j < data[0].Length - 1 && data[i - 1][j + 1] == 'S' && data[i + 1][j - 1] == 'M') // SAM da destra sopra a sinistra sotto
                    )
                )
                {
                    count++;
                }
            }
        }

        return $"{count}";
    }
}