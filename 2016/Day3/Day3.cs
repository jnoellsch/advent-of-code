namespace AoC
{
    using System;
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
            return checker.RealTrianglesCount;
        }

        public class TriangleChecker
        {
            public int RealTrianglesCount { get; private set; } = 0;

            public void GoAllOcdOnThatWall(string[] triangleMeasurementLines)
            {
                foreach (var measurementLine in triangleMeasurementLines)
                {
                    var measurement = measurementLine.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    int side1 = Convert.ToInt32(measurement[0]);
                    int side2 = Convert.ToInt32(measurement[1]);
                    int side3 = Convert.ToInt32(measurement[2]);

                    if (this.IsLegitTriangle(side1, side2, side3)) this.RealTrianglesCount++;
                }
            }

            public bool IsLegitTriangle(int side1, int side2, int side3)
            {
                return (side1 + side2) > side3;
            }
        }
    }
}
