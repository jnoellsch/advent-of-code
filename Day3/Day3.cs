namespace AoC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Plots the directions and delivery of presents to various houses. Determines the number of houses with 1 or more presents.
    /// http://adventofcode.com/day/3
    /// </summary>
    public class Day3 : IPuzzle
    {
        public object Answer()
        {
            string elfDirections = File.ReadAllText("Day3/input.txt");
            var giftTracker = new Dictionary<object, int>();
            var plotter = new HousePlotter(); 

            foreach (char nextStop in elfDirections)
            {
                plotter.Move(nextStop);

                var house = plotter.CoordinateKey;
                if (giftTracker.ContainsKey(house))
                {
                    giftTracker[house]++;
                }
                else
                {
                    giftTracker.Add(house, 1);
                }
            }

            return giftTracker.LuckyHouses();
        }

        public class HousePlotter
        {
            public HousePlotter()
            {
                this.X = 0;
                this.Y = 0;
            }

            public int X { get; private set; }

            public int Y { get; private set; }

            public object CoordinateKey
            {
                get
                {
                    return string.Format("{0}_{1}", this.X, this.Y);
                }
            }

            public void Move(char direction)
            {
                switch (direction.ToString())
                {
                    case ">":
                        this.X++;
                        break;
                    case "v":
                        this.Y++;
                        break;
                    case "<":
                        this.X--;
                        break;
                    case "^":
                        this.Y--;
                        break;
                }
            }
        }
    }

    public static class Day3Extensions
    {
        /// <summary>
        /// Counts the number of houses with 1 or more presents.
        /// </summary>
        public static int LuckyHouses(this IDictionary<object, int> input)
        {
            return input.Where(x => x.Value >= 1).Count();
        }
    }
}
