namespace AdventOfCode.Solutions.Day10;

public class Day10Tests
{
    private readonly Day10Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

public class Day10Solver : SolverBase
{
    // List<??> _data;
    private int[,] _grid;
    private HashSet<(int row, int col)> _paths = []; // hashset per evitare la ripetizione di path
    private int _totalRows;
    private int _totalColumns;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    private void InitializeGridAndPaths(List<string> data)
    {
        _totalRows = data.Count;
        _totalColumns = data[0].Length;
        _grid = new int[_totalRows, _totalColumns];

        for (var i = 0; i < _totalRows; i++)
        {
            for (var j = 0; j < _totalColumns; j++)
            {
                _grid[i, j] = int.Parse(data[i][j].ToString());
                // creazione di tutti i path che partono da 0
                if (_grid[i, j] == 0)
                {
                    _paths.Add((i, j));
                }
            }
        }
    }

    protected override object Solve1(List<string> data)
    {
        InitializeGridAndPaths(data);

        var sum = 0;
        foreach (var (row, col) in _paths)
        {
            sum += GetPathScore(row, col);
        }

        return sum;
    }

    private int GetPathScore(int i, int j, HashSet<(int row, int col)>? paths = null, bool allowMultiplePaths = false)
    {
        paths ??= [];
        var value = _grid[i, j];
        if (value == 9 && paths is not null)
        {
            return paths.Add((i, j)) ? 1 : allowMultiplePaths ? 1 : 0;
        }

        var pathsCount = 0;
        HashSet<(int row, int col)> borders = GetCellBorders(i, j);
        foreach (var (row, col) in borders)
        {
            var loopValue = _grid[row, col];
            if (loopValue == value + 1)
            {
                pathsCount += GetPathScore(row, col, paths, allowMultiplePaths);
            }
        }

        return pathsCount;
    }

    private HashSet<(int row, int col)> GetCellBorders(int row, int col)
    {
        var neighbors = new HashSet<(int row, int col)>();

        // i percorsi non possono essere in diagonale
        // su
        if (row > 0)
        {
            neighbors.Add((row - 1, col));
        }

        // sinistra
        if (col > 0)
        {
            neighbors.Add((row, col - 1));
        }

        // giu
        if (row < _totalRows - 1)
        {
            neighbors.Add((row + 1, col));
        }

        // destra
        if (col < _totalColumns - 1)
        {
            neighbors.Add((row, col + 1));
        }

        return neighbors;
    }

    protected override object Solve2(List<string> data)
    {
        InitializeGridAndPaths(data);

        var sum = 0;
        foreach (var (row, col) in _paths)
        {
            sum += GetPathScore(row, col, allowMultiplePaths: true);
        }

        return sum;
    }
}