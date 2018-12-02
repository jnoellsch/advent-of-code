namespace Aoc
{
    using System;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day2 : IPuzzle, IPuzzlePart2
    {
        public string[] BoxIds { get; set; } = File.ReadAllLines("Day2/input.txt");

        object IPuzzle.Answer()
        {
            var tracker = new BoxTracker();
            tracker.Calculate(this.BoxIds);

            return tracker.Checksum;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    public class BoxTracker
    {
        public int Checksum { get; private set; }

        public void Calculate(string[] boxIds)
        {
            int twoCount = 0;
            int threeCount = 0;

            foreach (var id in boxIds)
            {
                // group by characters in box id
                var groupedLetters = id
                    .GroupBy(c => c)
                    .Select(grp => new { Key = grp.Key, Count = grp.Count() })
                    .ToList();

                // count number of two and three groups. if multiple, just count it as one
                twoCount += Math.Min(groupedLetters.Count(grp => grp.Count == 2), 1);
                threeCount += Math.Min(groupedLetters.Count(grp => grp.Count == 3), 1);
            }

            this.Checksum = twoCount * threeCount;
        }
    }
}
