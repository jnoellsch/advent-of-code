namespace AoC
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    /// <summary>
    /// Validates the authenticity of triangle sizes on the wall.
    /// http://adventofcode.com/2016/day/3
    /// </summary>
    public class Day3 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var checker = new TriangleChecker();
            checker.GoAllOcdOnThatWall(File.ReadAllLines("Day3/input.txt"));
            return checker.Real;
        }

        object IPuzzlePart2.Answer()
        {
            var checker = new GroupedTriangleChecker();
            checker.GoAllOcdOnThatWall(File.ReadAllLines("Day3/input.txt"));
            return checker.Real;
        }

        public class GroupedTriangleChecker : TriangleChecker
        {
            public IList<Tuple<int, int, int>> TriangleInputs { get; private set; } = new List<Tuple<int, int, int>>();

            public override void GoAllOcdOnThatWall(string[] triangleMeasurementLines)
            {
                // populate
                foreach (string measurementLine in triangleMeasurementLines)
                {
                    var measurement = measurementLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int side1 = Convert.ToInt32(measurement[0]);
                    int side2 = Convert.ToInt32(measurement[1]);
                    int side3 = Convert.ToInt32(measurement[2]);

                    this.TriangleInputs.Add(new Tuple<int, int, int>(side1, side2, side3));
                }

                // chunk into column-based groups of 3
                int ff = 0;
                while (ff < this.TriangleInputs.Count)
                {
                    // get next chunk
                    var chunk = this.TriangleInputs.Skip(ff).Take(3).ToList();

                    // check column 1 + 2 + 3 chunks
                    if (this.IsLegitTriangle(chunk.ElementAt(0).Item1, chunk.ElementAt(1).Item1, chunk.ElementAt(2).Item1))
                        this.Real++;
                    else
                        this.Fake++;

                    if (this.IsLegitTriangle(chunk.ElementAt(0).Item2, chunk.ElementAt(1).Item2, chunk.ElementAt(2).Item2))
                        this.Real++;
                    else
                        this.Fake++;

                    if (this.IsLegitTriangle(chunk.ElementAt(0).Item3, chunk.ElementAt(1).Item3, chunk.ElementAt(2).Item3))
                        this.Real++;
                    else
                        this.Fake++;

                    // prepare for next chunk...
                    ff += 3;
                }
            }
        }
    }

    public class TriangleChecker
    {
        public virtual int Real { get; protected set; } = 0;

        public virtual int Fake { get; protected set; } = 0;

        public virtual int Total => this.Real + this.Fake;

        public virtual void GoAllOcdOnThatWall(string[] triangleMeasurementLines)
        {
            foreach (var measurementLine in triangleMeasurementLines)
            {
                var measurement = measurementLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int side1 = Convert.ToInt32(measurement[0]);
                int side2 = Convert.ToInt32(measurement[1]);
                int side3 = Convert.ToInt32(measurement[2]);
                Debug.WriteLine("{0} {1} {2}", side1, side2, side3);

                if (this.IsLegitTriangle(side1, side2, side3))
                {
                    this.Real++;
                }
                else
                {
                    this.Fake++;
                }
            }
        }

        public virtual bool IsLegitTriangle(int a, int b, int c)
        {
            bool abc = (a + b) > c;
            bool bca = (b + c) > a;
            bool acb = (a + c) > b;

            return abc && bca && acb;
        }
    }
}
