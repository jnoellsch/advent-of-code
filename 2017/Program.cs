namespace AoC
{
    using System;
    using AoC.Common;

    public class Program
    {
        public static void Main(string[] args)
        {
            ////IPuzzle part1 = new Day1();
            IPuzzlePart2 part2 = new Day1();

            ////Console.WriteLine("Part 1: " + part1.Answer());
            Console.WriteLine("Part 2: " + part2.Answer());
            Console.ReadKey();
        }
    }
}
