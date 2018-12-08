namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day6 : IPuzzle, IPuzzlePart2
    {
        private IList<Point> Coordinates { get; } = File.ReadAllLines("Day6/input.txt")
            .Select(_ => _.Split(','))
            .Select(_ => new Point(Convert.ToInt32(_[0]), Convert.ToInt32(_[1])))
            .ToList();

        object IPuzzlePart2.Answer()
        {
            var calculator = new DistanceCalculator();
            calculator.Load(this.Coordinates);

            return calculator.LargestArea;
        }

        object IPuzzle.Answer()
        {
            return string.Empty;
        }
    }

    internal class DistanceCalculator
    {
        public void Load(IList<Point> coordinates)
        {
        }

        public int LargestArea { get; set; }
    }
}
