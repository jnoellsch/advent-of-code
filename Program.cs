namespace AoC
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            ////IPuzzle day = new Day1();
            ////IPuzzle day = new Day2();
            ////IPuzzle day = new Day3();
            IPuzzle day = new Day4();

            Console.WriteLine(day.Answer());
            Console.ReadKey();
        }
    }
}
