﻿namespace AoC
{
    using System;
    using AoC.Common;

    public class Program
    {
        static void Main(string[] args)
        {
            ////IPuzzle part1 = new Day1();
            ////IPuzzlePart2 part2 = new Day1();
            ////IPuzzle part1 = new Day2();
            ////IPuzzlePart2 part2 = new Day2();
            ////IPuzzle part1 = new Day3();
            ////IPuzzlePart2 part2 = new Day3();
            ////IPuzzle part1 = new Day4();
            ////IPuzzlePart2 part2 = new Day4();
            ////IPuzzle part1 = new Day5();
            ////IPuzzlePart2 part2 = new Day5();
            ////IPuzzle part1 = new Day6();
            ////IPuzzlePart2 part2 = new Day6();
            ////IPuzzle part1 = new Day7();
            ////IPuzzlePart2 part2 = new Day7();
            IPuzzle part1 = new Day8();

            Console.WriteLine("Part 1: " + part1.Answer());
            ////Console.WriteLine("Part 2: " + part2.Answer());
            Console.ReadKey();
        }
    }
}
