namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AoC.Common;

    public class Day10 : IPuzzle, IPuzzlePart2
    {
        private IList<Star> Stars { get; } = File.ReadAllLines("Day10/input.txt").Select(Star.Parse).ToList();

        object IPuzzle.Answer()
        {
            var gazer = new StarGazer(this.Stars);
            gazer.Move();

            return "...use eyeballs";
        }

        object IPuzzlePart2.Answer()
        {
            return "N/A";
        }
    }

    internal class StarGazer
    {
        private const int SkyViewportHeight = 10;

        public IList<Star> Stars { get; }

        public int Count { get; set; }

        public StarGazer(IList<Star> stars)
        {
            this.Stars = stars;
        }

        public void Move()
        {
            while (true)
            {
                this.Shift();

                if (this.HaveTheStarsAligned())
                {
                    this.Print();
                    break;
                }
            }
        }


        private bool HaveTheStarsAligned()
        {
            int maxY = this.Stars.Max(_ => _.Y);
            int minY = this.Stars.Min(_ => _.Y);

            return (maxY - minY + 1) <= SkyViewportHeight;
        }

        private void Print()
        {
            var msg = new StringBuilder();

            int startY = this.Stars.Min(_ => _.Y);
            int startX = this.Stars.Min(_ => _.X);
            int endY = this.Stars.Max(_ => _.Y);
            int endX = this.Stars.Max(_ => _.X);

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (this.Stars.Any(s => s.Y == y && s.X == x))
                    {
                        msg.Append("#");
                    }
                    else
                    {
                        msg.Append(" ");
                    }
                }

                msg.Append(Environment.NewLine);
            }

            // display
            Console.WriteLine();
            Console.Write(msg.ToString().Trim());
            Console.WriteLine();
            Console.Write($"{this.Count} seconds");
            Console.WriteLine();
        }

        private void Shift()
        {
            foreach (var star in this.Stars)
            {
                star.Move();
            }

            this.Count++;
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
