using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Solutions.Day11;

public class Day11Tests
{
    private readonly Day11Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day11Solver : SolverBase
{
    // List<??> _data;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        var stones = data[0].Split(' ').ToList();
        for (int i = 0; i < 25; i++)
        {
            stones = Blink(stones);
        }

        return stones.Count;
    }

    private static List<string> Blink(List<string> stones)
    {
        var newStones = new List<string>();
        foreach (var stone in stones)
        {
            if (stone == "0")
            {
                newStones.Add("1");
            }
            else if (stone.Length % 2 == 0)
            {
                var middle = stone.Length / 2;
                var left = stone[..middle];
                var right = stone[middle..].TrimStart('0');
                if (string.IsNullOrWhiteSpace(right))
                {
                    right = "0";
                }
                newStones.Add(left);
                newStones.Add(right);
            }
            else
            {
                newStones.Add($"{long.Parse(stone) * 2024}");
            }
        }

        return newStones;
    }

    private readonly Dictionary<(string number, int stepsLeft), long> _cachedCombinations = [];

    protected override object Solve2(List<string> data)
    {
        // se usassi la logica del solve 1 i tempi di esecuzione crescerebbero esponenzialmente
        // considerando la mole di dati che continuerebbe a ciclare
        // con la ricorsione e con l'utilizzo di una cache si va a calcolare ogni singola "pietra" iniziale e non tutte assieme
        // e si riduce di oltre un minuto il tempo considerando il numero di combinazioni che si ripetono
        var stones = data[0].Split(' ').ToList();
        var sum = stones.Sum(x => BlinkRecursive(x, 75));
        return sum;
    }

    private long BlinkRecursive(string stone, int stepsRemaining)
    {
        if (stepsRemaining is 0)
        {
            return 1;
        }

        var key = (stone, stepsRemaining);
        if (_cachedCombinations.TryGetValue(key, out var cached))
        {
            return cached;
        }

        long result;
        if (stone == "0")
        {
            result = BlinkRecursive("1", stepsRemaining - 1);
        }
        else if (stone.Length % 2 == 0)
        {
            var middle = stone.Length / 2;
            var left = stone[..middle];
            var right = stone[middle..].TrimStart('0');
            if (string.IsNullOrWhiteSpace(right))
            {
                right = "0";
            }
            result = BlinkRecursive(left, stepsRemaining - 1) + BlinkRecursive(right, stepsRemaining - 1);
        }
        else
        {
            result = BlinkRecursive($"{long.Parse(stone) * 2024}", stepsRemaining - 1);
        }

        _cachedCombinations[key] = result;
        return result;
    }
}