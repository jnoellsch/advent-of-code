namespace AoC
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            IPuzzle part1 = new Day1();
            IPuzzlePart2 part2 = new Day1();
            ////IPuzzle part1 = new Day2();
            ////IPuzzle part1 = new Day3();
            ////IPuzzle part1 = new Day4();

            Console.WriteLine("Part 1: " + part1.Answer());
            Console.WriteLine("Part 2: " + part2.Answer());
            Console.ReadKey();
        }
    }
}
