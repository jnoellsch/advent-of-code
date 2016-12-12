namespace AoC
{
    using System;

    public class Program
    {
        static void Main(string[] args)
        {
            ////IPuzzle part1 = new Day1();
            ////IPuzzlePart2 part1 = new Day1();
            IPuzzle part1 = new Day2();
            IPuzzlePart2 part2 = new Day2();

            Console.WriteLine("Part 1: " + part1.Answer());
            Console.WriteLine("Part 2: " + part2.Answer());
            Console.ReadKey();
        }
    }
}
