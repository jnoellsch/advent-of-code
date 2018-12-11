namespace Aoc
{
    using System;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day2 : IPuzzle, IPuzzlePart2
    {
        public string[] BoxIds { get; set; } = File.ReadAllLines("Day02/input.txt");

        object IPuzzle.Answer()
        {
            var tracker = new BoxTracker();
            tracker.Calculate(this.BoxIds);

            return tracker.Checksum;
        }

        object IPuzzlePart2.Answer()
        {
            var finder = new BoxPrototypeFinder();
            finder.Evaluate(this.BoxIds);

            return finder.CommonBoxId;
        }
    }

    internal class BoxPrototypeFinder
    {
        public string CommonBoxId { get; private set; }

        public void Evaluate(string[] boxIds)
        {
            for (int index = 0; index < boxIds.Length - 1; index++)
            {
                string boxId = boxIds[index];

                for (int offsetIndex = index + 1; offsetIndex < boxIds.Length - 1; offsetIndex++)
                {
                    string candidateBoxId = boxIds[offsetIndex];
                    if (this.CheckForPrototype(boxId, candidateBoxId))
                    {
                        return;
                    }
                }
            }
        }

        private bool CheckForPrototype(string boxId, string candidateBoxId)
        {
            for (int i = 0; i < boxId.Length - 1; i++)
            {
                var boxPivotChar = boxId[i];
                var candidateBoxPivotChar = candidateBoxId[i];

                if (boxPivotChar != candidateBoxPivotChar)
                {
                    var boxIdL = boxId.Substring(0, i);
                    var boxIdR = boxId.Substring(i + 1);

                    var candidateIdL = candidateBoxId.Substring(0, i);
                    var candidateIdR = candidateBoxId.Substring(i + 1);

                    bool bothSidesOfPivotMatch = boxIdL == candidateIdL && boxIdR == candidateIdR;
                    if (bothSidesOfPivotMatch)
                    {
                        this.CommonBoxId = boxId.Replace(boxPivotChar.ToString(), string.Empty);
                        return true;
                    }
                }
            }

            return false;
        }
    }

    internal class BoxTracker
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
