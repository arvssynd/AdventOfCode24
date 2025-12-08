namespace AdventOfCode.Solutions.Day08;

public class Day08Tests
{
    private readonly Day08Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

internal record Coordinate(int Row, int Col);

public class Day08Solver : SolverBase
{
    // List<??> _data;
    private char[][] _grid;
    private Dictionary<int, HashSet<Coordinate>> _antennasMapCoordinates = new();
    private int _totalRows;
    private int _totalColumns;
    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    private void PopulateGrid(List<string> data)
    {
        _totalRows = data.Count;
        _totalColumns = data[0].Length;

        _grid = new char[_totalRows][];
        for (var i = 0; i < _totalRows; i++)
        {
            _grid[i] = data[i].ToCharArray();
        }

        // trovo le coordinate delle antenne
        for (var i = 0; i < _totalRows; i++)
        {
            for (var j = 0; j < _totalColumns; j++)
            {
                var value = _grid[i][j];
                if (value is '.')
                {
                    continue;
                }

                if (_antennasMapCoordinates.TryGetValue(value, out var coordinates))
                {
                    coordinates.Add(new Coordinate(i, j));
                }
                else
                {
                    _antennasMapCoordinates[value] = [new Coordinate(i, j)];
                }
            }
        }
    }

    protected override object Solve1(List<string> data)
    {
        PopulateGrid(data);

        // coppie di antenne
        int antinodes = 0;
        foreach (var (_, coordinates) in _antennasMapCoordinates)
        {
            var coordinateList = coordinates.ToList();
            var couples = new List<(Coordinate, Coordinate)>();
            for (var i = 0; i < coordinateList.Count; i++)
            {
                for (var j = i + 1; j < coordinateList.Count; j++)
                {
                    var coord1 = coordinateList[i];
                    var coord2 = coordinateList[j];
                    couples.Add((coord1, coord2));
                }
            }

            // distanza tra le coppie si va a ritroso dalla prima e in avanti dalla seconda
            // di quanti spazi tanto quanto è la distanza tra le coppie
            foreach (var (c1, c2) in couples)
            {
                var rowDiff = c2.Row - c1.Row;
                var colDiff = c2.Col - c1.Col;

                if (IsAnAntinode(c2.Row + rowDiff, c2.Col + colDiff))
                {
                    antinodes++;
                }

                if (IsAnAntinode(c1.Row - rowDiff, c1.Col - colDiff))
                {
                    antinodes++;
                }
            }
        }

        return antinodes;
    }

    private bool IsAnAntinode(int row, int col)
    {
        if (row < 0 || row >= _totalRows || col < 0 || col >= _totalColumns || _grid[row][col] == '#')
        {
            return false;
        }

        _grid[row][col] = '#';
        return true;
    }

    protected override object Solve2(List<string> data)
    {
        PopulateGrid(data);

        var antinodeCount = 0;
        foreach (var (_, coordinates) in _antennasMapCoordinates)
        {
            var coordinateList = coordinates.ToList();
            var couples = new List<(Coordinate, Coordinate)>();
            for (var i = 0; i < coordinateList.Count; i++)
            {
                for (var j = i + 1; j < coordinateList.Count; j++)
                {
                    var coord1 = coordinateList[i];
                    var coord2 = coordinateList[j];
                    couples.Add((coord1, coord2));
                }
            }

            foreach (var (c1, c2) in couples)
            {
                var rowDiff = c2.Row - c1.Row;
                var colDiff = c2.Col - c1.Col;

                var lastCoordinate = c2;
                while (lastCoordinate.Row >= 0 && lastCoordinate.Row < _totalRows && lastCoordinate.Col >= 0 && lastCoordinate.Col < _totalColumns)
                {
                    if (IsAnAntinode(lastCoordinate.Row, lastCoordinate.Col))
                    {
                        antinodeCount++;
                    }

                    lastCoordinate = new Coordinate(lastCoordinate.Row + rowDiff, lastCoordinate.Col + colDiff);
                }

                var firstCoordinate = c1;
                while (firstCoordinate.Row >= 0 && firstCoordinate.Row < _totalRows && firstCoordinate.Col >= 0 && firstCoordinate.Col < _totalColumns)
                {
                    if (IsAnAntinode(firstCoordinate.Row, firstCoordinate.Col))
                    {
                        antinodeCount++;
                    }

                    firstCoordinate = new Coordinate(firstCoordinate.Row - rowDiff, firstCoordinate.Col - colDiff);
                }
            }
        }

        return antinodeCount;
    }
}