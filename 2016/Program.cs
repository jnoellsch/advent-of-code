namespace AoC
{
    using System;

    public class Program
    {
        static void Main(string[] args)
        {
            IPuzzle part1 = new Day1();
            ////IPuzzlePart2 part2 = new Day1();

            Console.WriteLine("Part 1: " + part1.Answer());
            Console.ReadKey();
        }
    }
}
