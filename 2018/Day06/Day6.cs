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
        private IList<Coordinate> Coordinates { get; } = File.ReadAllLines("Day06/input.txt").Select((_, i) => Coordinate.Parse(i, _)).ToList();

        object IPuzzle.Answer()
        {
            var calculator = new DistanceCalculator();
            calculator.Load(this.Coordinates);
            ////calculator.DebugMatrix();

            return calculator.LargestArea;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class DistanceCalculator
    {
        private int Columns { get; set; }

        private int Rows { get; set; }

        public int LargestArea { get; set; }

        private string[,] PointMatrix { get; set; }

        public void Load(IList<Coordinate> coordinates)
        {
            this.Rows = coordinates.Max(_ => _.Point.Y);
            this.Columns = coordinates.Max(_ => _.Point.X);
            this.PointMatrix = new string[this.Columns, this.Rows];

            // populate matrix
            for (int y = 0; y < this.Rows; y++)
            for (int x = 0; x < this.Columns; x++)
            {
                var p = new Point(x, y);
                this.PointMatrix[x, y] = this.FindClosest(p, coordinates);
            }

            // remove coordinates at the fringes (i.e. infinite)
            int startX = coordinates.Min(_ => _.Point.X);
            int startY = coordinates.Min(_ => _.Point.Y);
            var infiniteCoordinates = coordinates.Where(
                c => c.Point.X == startX
                  || c.Point.Y == startY
                  || c.Point.X == this.Columns
                  || c.Point.Y == this.Rows);

            var nonInfiniteCoordinates = coordinates.Except(infiniteCoordinates).ToList();

            var sums = new Dictionary<string, int>();
            nonInfiniteCoordinates.ForEach(_ => sums.Add(_.Id, 0));

            // sum
            foreach (var nic in nonInfiniteCoordinates)
            {
                for (int y = startY; y < this.Rows; y++)
                for (int x = startX; x < this.Columns; x++)
                {
                    var id = this.PointMatrix[x, y];
                    if (id == nic.Id)
                    {
                        sums[id]++;
                    }
                }
            }

            this.LargestArea = sums.OrderByDescending(_ => _.Value).First().Value;
        }

        public void DebugMatrix()
        {
            for (int y = 0; y < this.Rows; y++)
            {
                for (int x = 0; x < this.Columns; x++)
                {
                    Console.Write(this.PointMatrix[x, y]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private string FindClosest(Point start, IList<Coordinate> coordinates)
        {
            var calculations = coordinates.Select(c => new { Coordinate = c, Distance = c.DistanceTo(start) }).OrderBy(c => c.Distance).ToList();
            var smallest = calculations.First();
            var nextSmallest = calculations.Skip(1).First();

            if (smallest.Distance == nextSmallest.Distance)
            {
                return ".";
            }
            else if (smallest.Distance == 0)
            {
                return smallest.Coordinate.Id;
            }
            else
            {
                return smallest.Coordinate.Id;
            }
        }
    }

    internal class Coordinate
    {
        public string Id { get; private set; }

        public Point Point { get; private set; }

        public int DistanceTo(Point point) => Algorithms.ManhattanDistance(point, this.Point);

        public static Coordinate Parse(int id, string input)
        {
            var point = input.Split(',');
            return new Coordinate()
            {
                Id = id.ToString(),
                Point = new Point(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]))
            };
        }

        public override string ToString() => this.Id;
    }
}
