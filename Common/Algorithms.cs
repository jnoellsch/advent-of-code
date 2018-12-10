namespace AoC.Common
{
    using System;
    using System.Drawing;

    public class Algorithms
    {
        public static int ManhattanDistance(Point start, Point end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }
    }
}
