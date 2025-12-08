using System;

namespace AdventOfCode.Solutions.Day09;

public class Day09Tests
{
    private readonly Day09Solver _solver = new();
    //[Test] public async Task Step1WithExample() => await _solver.ExecuteExample1("??");
    //[Test] public async Task Step2WithExample() => await _solver.ExecuteExample2("??");
    [Test] public void Step1WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle1());
    [Test] public void Step2WithPuzzleInput() => TestContext.Current?.OutputWriter.WriteLine(_solver.ExecutePuzzle2());
}

internal class Block(int id, int size, int freeSpace)
{
    public int Id { get; } = id;
    public int Size { get; set; } = size;
    public int FreeSpace { get; set; } = freeSpace;
}

public class Day09Solver : SolverBase
{
    // List<??> _data;

    protected override void Parse(List<string> data)
    {
        // _data = new();
    }

    private static List<Block> GenerateBlocks(List<string> data)
    {
        string diskMap = data.First();
        var blocks = new List<Block>();

        for (var i = 0; i < diskMap.Length; i += 2)
        {
            var size = int.Parse(diskMap[i].ToString());
            var freeSpace = i + 1 < diskMap.Length ? int.Parse($"{diskMap[i + 1]}") : 0;
            blocks.Add(new Block(i / 2, size, freeSpace));
        }

        return blocks;
    }

    protected override object Solve1(List<string> data)
    {
        var blocks = GenerateBlocks(data);

        // riordinamento degli spazi partendo dalla fine per riempire lo spazio libero
        var availableSpace = blocks.FirstOrDefault(x => x.FreeSpace > 0);
        while (availableSpace is not null)
        {
            var lastBlock = blocks.Last();
            if (lastBlock.Size <= availableSpace.FreeSpace)
            {
                var newBlock = new Block(lastBlock.Id, lastBlock.Size, availableSpace.FreeSpace - lastBlock.Size);
                blocks.Insert(blocks.IndexOf(availableSpace) + 1, newBlock);
                blocks.Remove(lastBlock);
            }
            else
            {
                var newBlock = new Block(lastBlock.Id, availableSpace.FreeSpace, 0);
                lastBlock.Size -= availableSpace.FreeSpace;
                blocks.Insert(blocks.IndexOf(availableSpace) + 1, newBlock);
            }

            availableSpace.FreeSpace = 0;
            availableSpace = blocks.FirstOrDefault(x => x.FreeSpace > 0);
        }

        // calcolo della somma index * id
        return CheckSum(blocks);
    }

    private static long CheckSum(List<Block> blocks)
    {
        long sum = 0;
        int index = 0;
        foreach (var block in blocks)
        {
            for (var i = 0; i < block.Size; i++)
            {
                sum += index * block.Id;
                index += block.FreeSpace;
                index++;
            }
        }

        return sum;
    }

    protected override object Solve2(List<string> data)
    {
        var blocks = GenerateBlocks(data);

        // al posto di riempire i componenti di un blocco negli spazi
        // bisogna provare a spostare il blocco intero partendo dal fondo
        var reversedBlocks = new List<Block>(blocks);
        reversedBlocks.Reverse();

        foreach (var rBlock in reversedBlocks)
        {
            foreach (var block in blocks)
            {
                if (block == rBlock)
                {
                    break;
                }

                if (rBlock.Size > block.FreeSpace)
                {
                    continue;
                }

                var newBlock = new Block(rBlock.Id, rBlock.Size, block.FreeSpace - rBlock.Size);
                blocks.Insert(blocks.IndexOf(block) + 1, newBlock);

                // il blocco rimosso deve diventare spazio libero per il blocco precedente
                var emptiedBlock = blocks[blocks.IndexOf(rBlock) - 1];
                emptiedBlock.FreeSpace += rBlock.Size + rBlock.FreeSpace;
                blocks.Remove(rBlock);
                block.FreeSpace -= rBlock.Size;
                break;
            }
        }

        return CheckSum(blocks);
    }
}