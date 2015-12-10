namespace AoC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Plots the directions and delivery of presents to various houses. Determines the number of houses with 1 or more presents.
    /// http://adventofcode.com/day/3
    /// </summary>
    public class Day3 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string elfDirections = File.ReadAllText("Day3/input.txt");
            var giftTracker = new GiftTracker();
            var plotter = new HousePlotter();

            foreach (char nextStop in elfDirections)
            {
                plotter.Move(nextStop);
                giftTracker.Deliver(plotter.CoordinateKey);
            }

            return giftTracker.LuckyHouses();
        }

        object IPuzzlePart2.Answer()
        {
            string elfDirections = File.ReadAllText("Day3/input.txt");
            var giftTracker = new GiftTracker();
            var santaPlotter = new HousePlotter();
            var roboSantaPlotter = new HousePlotter();

            // gift the starting house
            giftTracker.Deliver(santaPlotter.CoordinateKey);

            // follow directions for santa and robo santa
            foreach (char nextStop in elfDirections.SantaOnly())
            {
                santaPlotter.Move(nextStop);
                giftTracker.Deliver(santaPlotter.CoordinateKey);
            }

            foreach (char nextStop in elfDirections.RoboSantaOnly())
            {
                roboSantaPlotter.Move(nextStop);
                giftTracker.Deliver(roboSantaPlotter.CoordinateKey);
            }

            return giftTracker.LuckyHouses();
        }

        public class GiftTracker
        {
            private IDictionary<object, int> _giftTracker = new Dictionary<object, int>();

            public void Deliver(object house)
            {
                if (this._giftTracker.ContainsKey(house))
                {
                    this._giftTracker[house]++;
                }
                else
                {
                    this._giftTracker.Add(house, 1);
                }
            }

            /// <summary>
            /// Counts the number of houses with 1 or more presents.
            /// </summary>
            public int LuckyHouses()
            {
                return this._giftTracker.Where(x => x.Value >= 1).Count();
            }
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
        /// Extracts santa directions only (aka, odd slots).
        /// </summary>
        public static IEnumerable<char> SantaOnly(this string directions)
        {
            return directions.Where((x, i) => i % 2 != 0);
        }

        /// <summary>
        /// Extracts robo santa directions only (aka, even slots).
        /// </summary>
        public static IEnumerable<char> RoboSantaOnly(this string directions)
        {
            return directions.Where((x, i) => i % 2 == 0);
        }
    }
}
