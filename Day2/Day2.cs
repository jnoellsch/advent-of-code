namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Calculates the total wrapping paper needs for elfs by summing all sides and some slack.
    /// http://adventofcode.com/day/2
    /// </summary>
    public class Day2 : IDay
    {
        public object Answer()
        {
            var boxes = ParseBoxes();
            return boxes.Sum(b => b.CalculateElfSurfaceArea());
        }

        private static List<Box> ParseBoxes()
        {
            var boxes = new List<Box>();
            Regex regex = new Regex("\\d+");
            string[] allLines = File.ReadAllLines("Day2/input.txt");

            foreach (var line in allLines)
            {
                var match = regex.Matches(line);
                boxes.Add(new Box(Convert.ToInt32(match[0].Value), Convert.ToInt32(match[1].Value), Convert.ToInt32(match[2].Value)));
            }

            return boxes;
        }

        public class Box
        {
            public Box(int l, int h, int w)
            {
                this.Length = l;
                this.Height = h;
                this.Width = w;
            }

            public int Length { get; private set; }

            public int Height { get; private set; }

            public int Width { get; private set; }

            public int LengthWidthSufaceArea
            {
                get
                {
                    return this.Length * this.Width;
                }
            }

            public int WidthHeightSurfaceArea
            {
                get
                {
                    return this.Width * this.Height;
                }
            }

            public int HeightLengthSufaceArea
            {
                get
                {
                    return this.Height * this.Length;
                }
            }

            /// <summary>
            /// Gets the smallest surface area side.
            /// </summary>
            public int Slack
            {
                get
                {
                    return new List<int>() { this.LengthWidthSufaceArea, this.HeightLengthSufaceArea, this.WidthHeightSurfaceArea }.Min();
                }
            }

            /// <summary>
            /// Calculates the standard surface area.
            /// </summary>
            public int CalculateSurfaceArea()
            {
                return 
                    (2 * this.LengthWidthSufaceArea) + 
                    (2 * this.WidthHeightSurfaceArea) + 
                    (2 * this.HeightLengthSufaceArea);
            }

            /// <summary>
            /// Calculates the stanard surface area, including slack.
            /// </summary>
            public int CalculateElfSurfaceArea()
            {
                return this.CalculateSurfaceArea() + this.Slack;
            }
        }
    }
}
