namespace AoC
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Validates the authenticity of triangle sizes on the wall.
    /// http://adventofcode.com/2016/day/3
    /// </summary>
    public class Day3 : IPuzzle
    {
        public object Answer()
        {
            var checker = new TriangleChecker();
            checker.GoAllOcdOnThatWall(File.ReadAllLines("Day3/input.txt"));
            return checker.Real;
        }

        public class TriangleChecker
        {
            public int Real { get; private set; } = 0;

            public int Fake { get; private set; } = 0;

            public int Total => this.Real + this.Fake;

            public void GoAllOcdOnThatWall(string[] triangleMeasurementLines)
            {
                foreach (var measurementLine in triangleMeasurementLines)
                {
                    var measurement = measurementLine.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
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

            public bool IsLegitTriangle(int a, int b, int c)
            {
                bool abc = (a + b) > c;
                bool bca = (b + c) > a;
                bool acb = (a + c) > b;

                return abc && bca && acb;
            }
        }
    }
}
