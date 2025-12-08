namespace AdventOfCode.Solutions.Day05;

public class Day05Tests
{
    private readonly Day05Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day05Solver : SolverBase
{
    // List<??> _data;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        Dictionary<int, List<int>> sequenceRules = [];
        List<List<int>> updateSequence = [];

        var instructions = true;
        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                instructions = false;
                continue;
            }

            if (instructions)
            {
                var pair = line.Split('|').Select(int.Parse).ToList();

                // 1|2 indica la lista di numeri da stampare prima del 2
                if (sequenceRules.TryGetValue(pair[1], out var incList))
                {
                    incList.Add(pair[0]);
                }
                else
                {
                    sequenceRules[pair[1]] = [pair[0]];
                }
            }
            else
            {
                updateSequence.Add([.. line.Split(',').Select(int.Parse)]);
            }
        }

        return $"{updateSequence.Where(x => IsOrdered(x, sequenceRules)).Sum(pageNumbers => pageNumbers[(pageNumbers.Count - 1) / 2])}";
    }

    private static bool IsOrdered(List<int> pageNumbers, Dictionary<int, List<int>> sequenceRules)
    {
        HashSet<int> disallowedPageNumbers = [];
        foreach (var pageNumber in pageNumbers)
        {
            if (disallowedPageNumbers.Contains(pageNumber))
            {
                return false;
            }

            if (sequenceRules.TryGetValue(pageNumber, out var rule))
            {
                disallowedPageNumbers.UnionWith(rule);
            }
        }

        return true;
    }

    protected override object Solve2(List<string> data)
    {
        Dictionary<int, List<int>> sequenceRules = [];
        Dictionary<int, List<int>> reverseSequenceRules = [];
        List<List<int>> updateSequence = [];

        var instructions = true;
        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                instructions = false;
                continue;
            }

            if (instructions)
            {
                var pair = line.Split('|').Select(int.Parse).ToList();

                // 1|2 indica che l'1 deve essere stampato prima del 2
                if (reverseSequenceRules.TryGetValue(pair[0], out var incListRev))
                {
                    incListRev.Add(pair[1]);
                }
                else
                {
                    reverseSequenceRules[pair[0]] = [pair[1]];
                }

                // 1|2 indica la lista di numeri da stampare prima del 2
                if (sequenceRules.TryGetValue(pair[1], out var incList))
                {
                    incList.Add(pair[0]);
                }
                else
                {
                    sequenceRules[pair[1]] = [pair[0]];
                }
            }
            else
            {
                updateSequence.Add([.. line.Split(',').Select(int.Parse)]);
            }
        }

        int sum = 0;
        var sequencesToReorder = updateSequence.Where(x => !IsOrdered(x, sequenceRules));
        foreach (var item in sequencesToReorder)
        {
            item.Sort((x, y) =>
            {
                // x deve essere prima di y
                if (reverseSequenceRules.TryGetValue(x, out var ruleX))
                {
                    if (ruleX.Contains(y))
                    {
                        return -1;
                    }
                }

                // y deve essere prima di x
                if (reverseSequenceRules.TryGetValue(y, out var ruleY))
                {
                    if (ruleY.Contains(x))
                    {
                        return 1;
                    }
                }

                return 0;
            });

            sum += item[(item.Count - 1) / 2];
        }

        return $"{sum}";
    }
}