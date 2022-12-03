namespace AoC
{
    using AoC.Common;
    using System;

    internal class Day1 : IPuzzle, IPuzzlePart2
    {
        private int[] SonarReads { get; } = File.ReadAllLines("Day01/input.txt").Select(sr => int.Parse(sr)).ToArray();

        object IPuzzle.Answer()
        {
            var ss = new SonarSweep();
            return ss.CalculateDepth(this.SonarReads);
        }

        object IPuzzlePart2.Answer()
        {
            var sss = new SlidingSonarSweep();
            return sss.CalculateDepth(this.SonarReads);
        }
    }

    internal class SonarSweep
    {
        public virtual int CalculateDepth(int[] sonarReads)
        {
            var prevRead = -1;
            var depth = -1;

            foreach (var read in sonarReads)
            {
                if (read > prevRead)
                {
                    depth++;
                }

                prevRead = read;
            }

            return depth;
        }
    }

    internal class SlidingSonarSweep : SonarSweep
    {
        public override int CalculateDepth(int[] sonarReads)
        {
            var prevRead = -1;
            var depth = -1;
            var read = 0;
            sonarReads = sonarReads.ToArray();

            for (int i = 0; i < sonarReads.Length - 2; i++)
            {
                read = sonarReads[i] + sonarReads[i + 1] + sonarReads[i + 2];

                if (read > prevRead)
                {
                    depth++;
                }

                prevRead = read;
            }

            return depth;
        }
    }
}
