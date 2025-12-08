using System.Numerics;

namespace AdventOfCode.Solutions.Day07;

public class Day07Tests
{
    private readonly Day07Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}
internal record Equation(long TestValue, List<int> Numbers);

public class Day07Solver : SolverBase
{
    // List<??> _data;

    private enum Operator
    {
        Add,
        Multiply,
        Concatenate
    }

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        var equations = new List<Equation>();
        foreach (var line in data)
        {
            var parts = line.Split(':');
            if (parts.Length != 2)
            {
                continue;
            }

            var numbers = parts[1].Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n.Trim()))
                .ToList();

            if (numbers.Count > 0)
            {
                equations.Add(new Equation(long.Parse(parts[0]), numbers));
            }
        }

        long sum = 0;
        foreach (var equation in equations)
        {
            if (EquationFinder(equation.TestValue, equation.Numbers, [Operator.Add, Operator.Multiply]))
            {
                sum += equation.TestValue;
            }
        }

        return sum;
    }

    private static bool EquationFinder(long testValue, List<int> numbers, Operator[] allowedOperators)
    {
        // numero di combinazioni possibili 2^(n-1)
        long maxCombinations = 1L << numbers.Count - 1;

        for (long i = 0; i < maxCombinations; i++)
        {
            long currentResult = numbers[0];
            var operatorsUsed = new List<Operator>();

            BigInteger tempI = i;

            for (int opIndex = 0; opIndex < numbers.Count - 1; opIndex++)
            {
                int opCode = (int)(tempI % allowedOperators.Length);
                tempI /= allowedOperators.Length;

                Operator op = opCode switch
                {
                    0 => Operator.Add,
                    1 => Operator.Multiply,
                    _ => Operator.Concatenate
                };

                operatorsUsed.Add(op);

                long nextNumber = numbers[opIndex + 1];

                // Valutazione da sinistra a destra
                if (op == Operator.Add)
                {
                    currentResult += nextNumber;
                }
                else if (op == Operator.Multiply)
                {
                    currentResult *= nextNumber;
                }
            }

            if (currentResult == testValue)
            {
                return true;
            }
        }

        return false;
    }

    protected override object Solve2(List<string> data)
    {
        return 0;
    }
}