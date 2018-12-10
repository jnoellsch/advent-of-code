namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using AoC.Common;

    public class Day10 : IPuzzle, IPuzzlePart2
    {
        private IList<Star> Stars { get; } = File.ReadAllLines("Day10/sampleinput.txt").Select(Star.Parse).ToList();

        object IPuzzlePart2.Answer()
        {
            var gazer = new StarGazer(this.Stars);
            gazer.SkipTo(3);
            gazer.Move();

            return "...use eyeballs";
        }

        object IPuzzle.Answer()
        {
            return string.Empty;
        }
    }

    internal class StarGazer
    {
        public IList<Star> Stars { get; }

        public int Count { get; set; }

        public int BoundX { get; set; }

        public int BoundY { get; set; }

        public StarGazer(IList<Star> stars)
        {
            this.Stars = stars;
            this.BoundY = this.Stars.Max(_ => Math.Abs(_.Y));
            this.BoundX = this.Stars.Max(_ => Math.Abs(_.X));
        }

        public void Move()
        {
            while (true)
            {
                this.Shift();
                this.Print();
                Console.ReadKey();
            }
        }

        private void Print()
        {
            Console.WriteLine($"#{this.Count}:");

            for (int y = 0; y < this.BoundY; y++)
            {
                for (int x = 0; x < this.BoundX; x++)
                {
                    if (this.Stars.Any(s => s.X == x && s.Y == y))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
        }

        private void Shift()
        {
            foreach (var star in this.Stars)
            {
                star.Move();
            }

            this.Count++;
        }

        public void SkipTo(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Stars.ForEach(_ => _.Move());
            }

            this.Count = count;
            this.Print();
            Console.ReadKey();
        }
    }

    internal class Star
    {
        public int Y { get; private set; }
        public int X { get; private set; }
        
        public int VelocityX { get; private set; }

        public int VelocityY { get; private set; }

        public void Move()
        {
            this.X += this.VelocityX;
            this.Y += this.VelocityY;
        }

        public static Star Parse(string input)
        {
            var regex = new Regex(@"position=<(?<px>.*?), (?<py>.*?)> velocity=<(?<vx>.*?), (?<vy>.*?)>");
            var match = regex.Match(input);

            return new Star()
            {
                X = match.Groups["px"].Value.ToInt(),
                Y = match.Groups["py"].Value.ToInt(),
                VelocityX = match.Groups["vx"].Value.ToInt(),
                VelocityY = match.Groups["vy"].Value.ToInt()
            };
        }
    }
}
