using System;

namespace AdventOfCode.Solutions.Day06;

public class Day06Tests
{
    private readonly Day06Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day06Solver : SolverBase
{
    // List<??> _data;
    private char[,] _grid;
    private (int row, int col)? _position = null;
    private HashSet<(int row, int col)> _visitedGrid = [];
    private int _direction = 0; // 0=up, 1=right, 2=down, 3=left, si potrebbe fare una gestione dell'enum 

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    protected override object Solve1(List<string> data)
    {
        _grid = new char[data.Count, data[0].Length];
        int tilesCovered = 0;

        // Initialize grid and find starting position
        for (var i = 0; i < data.Count; i++)
        {
            for (var j = 0; j < data[i].Length; j++)
            {
                _grid[i, j] = data[i][j];
                if (data[i][j] is '^')
                {
                    _position = (i, j);
                }
            }
        }

        if (_position is not null)
        {
            tilesCovered = Move();
        }

        return $"{tilesCovered}";
    }

    private int Move()
    {
        _visitedGrid.Add(_position.Value);

        while (true)
        {
            var nextPosition = GetNextPosition(_position.Value, _direction);
            if (!IsValidPosition(nextPosition))
            {
                break;
            }

            if (_grid[nextPosition.row, nextPosition.col] is '#')
            {
                _direction++;
                if (_direction > 3)
                {
                    _direction = 0;
                }
                continue;
            }

            _position = nextPosition;
            _visitedGrid.Add(_position.Value);
        }

        return _visitedGrid.Count;
    }

    private static (int row, int col) GetNextPosition((int row, int col) position, int direction)
    {
        return direction switch
        {
            0 => (position.row - 1, position.col),
            1 => (position.row, position.col + 1),
            2 => (position.row + 1, position.col),
            3 => (position.row, position.col - 1),
            _ => throw new ArgumentException($"Invalid direction: {direction}")
        };
    }

    private bool IsValidPosition((int row, int col) pos) =>
       pos.row >= 0 && pos.row < _grid.GetLength(0) &&
       pos.col >= 0 && pos.col < _grid.GetLength(1);

    protected override object Solve2(List<string> data)
    {
        return 0;
    }
}