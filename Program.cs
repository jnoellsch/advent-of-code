﻿namespace AoC
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            ////IDay day = new Day1();
            ////IDay day = new Day2();
            IPuzzle day = new Day3();

            Console.WriteLine(day.Answer());
            Console.ReadKey();
        }
    }
}
