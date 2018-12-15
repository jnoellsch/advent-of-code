namespace Aoc
{
    using System;
    using AoC.Common;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Day12 : IPuzzle, IPuzzlePart2
    {
        private string InitialState = "###..###....####.###...#..#...##...#..#....#.##.##.#..#.#..##.#####..######....#....##..#...#...#.#";

        private string SampleInitialState = "#..#.#..##......###...###";

        private IList<GrowthPattern> Patterns = File.ReadAllLines("Day12/input.txt").Select(GrowthPattern.Parse).Where(_ => _.Outcome == "#").ToList();

        private IList<GrowthPattern> SamplePatterns = File.ReadAllLines("Day12/sampleinput.txt").Select(GrowthPattern.Parse).ToList();

        object IPuzzle.Answer()
        {
            var gt = new GreenThumb(this.InitialState);
            gt.WithPatterns(this.Patterns);
            gt.GrowCycles(20);

            return gt.PlantSumValue;
        }

        object IPuzzlePart2.Answer()
        {
            var gt = new GreenThumb(this.InitialState);
            gt.WithPatterns(this.Patterns);
            gt.GrowCycles(20);
            gt.DebugFinalOutcome();
            gt.GrowCycles(80);
            gt.DebugFinalOutcome();
            gt.GrowCycles(100);
            gt.DebugFinalOutcome();
            gt.GrowCycles(1);
            gt.DebugFinalOutcome();
            gt.GrowCycles(1);
            gt.DebugFinalOutcome();
            gt.GrowCycles(1);
            gt.DebugFinalOutcome();
            gt.GrowCycles(97);
            gt.DebugFinalOutcome();

            return gt.PlantSumValue;
        }
    }

    internal class GreenThumb
    {
        public string State { get; private set; }

        public IList<GrowthPattern> Patterns { get; private set; }

        public IList<string> Outcomes = new List<string>();

        public int NegativeDelta { get; private set; }

        public GreenThumb(string initialState)
        {
            this.InitialState = initialState;
            this.State = initialState;
            this.Outcomes.Add(this.State);
        }

        public string InitialState { get; set; }

        public int PlantSumValue;


        public void WithPatterns(IList<GrowthPattern> patterns)
        {
            this.Patterns = patterns;
        }

        public void GrowCycles(int numOfCycles)
        {
            for (int cycle = 0; cycle < numOfCycles; cycle++)
            {
                var sb = new StringBuilder();

                for (int i = 0; i < this.State.Length; i++)
                {
                    var wasFound = this.Patterns.Any(_ => _.Pattern == this.Chunk(i));
                    sb.Append(wasFound ? "#" : ".");
                }

                this.PrependFix(sb);
                this.AppendFix(sb);

                this.State = sb.ToString();
                this.Outcomes.Add(this.State);
            }

            this.PlantSumValue = this.CalculatePlantSum();
        }

        private int CalculatePlantSum()
        {
            string plantState = this.State;
            int sum = 0;

            for (int whatToAdd = this.NegativeDelta; whatToAdd < this.State.Length + this.NegativeDelta; whatToAdd++)
            {
                if (plantState[0].Equals('#'))
                {
                    sum += whatToAdd;
                }

                plantState = plantState.Remove(0, 1);
            }

            return sum;
        }

        private void PrependFix(StringBuilder sb)
        {
            string pattern1 = "..." + new string(this.State.Take(2).ToArray());
            if (this.Patterns.Any(_ => _.Pattern == pattern1))
            {
                sb.Insert(0, "#");
                this.NegativeDelta -= 1;
            }

            string pattern2 = "...." + new string(this.State.Take(1).ToArray());
            if (this.Patterns.Any(_ => _.Pattern == pattern2))
            {
                sb.Insert(0, "#.");
                this.NegativeDelta -= 2;
            }
        }

        private void AppendFix(StringBuilder sb)
        {
            string pattern1 = new string(this.State.Skip(this.State.Length - 2).Take(2).ToArray()) + "...";
            if (this.Patterns.Any(_ => _.Pattern == pattern1))
            {
                sb.Append("#");
            }
            string pattern2 = new string(this.State.Skip(this.State.Length - 1).Take(1).ToArray()) + "....";
            if (this.Patterns.Any(_ => _.Pattern == pattern2))
            {
                sb.Append(".#");
            }
        }

        private string Chunk(int i)
        {
            if (i == 0)
                return ".." + new string(this.State.Take(3).ToArray());
            else if (i == 1)
                return "." + new string(this.State.Take(4).ToArray());
            else if (i == this.State.Length - 2)
                return new string(this.State.Skip(i - 2).Take(4).ToArray()) + ".";
            else if (i == this.State.Length - 1)
                return new string(this.State.Skip(i - 2).Take(3).ToArray()) + "..";
            else
                return new string(this.State.Skip(i - 2).Take(5).ToArray());
        }

        public void DebugOutcomes()
        {
            Console.WriteLine("--: ");
            Console.WriteLine("--: 0");

            for (var i = 0; i < this.Outcomes.Count; i++)
            {
                Console.WriteLine($"{i:D2}: {this.Outcomes[i]}");
            }

            Console.WriteLine($"Negative delta: {this.NegativeDelta}");
        }

        public void DebugFinalOutcome()
        {
            Console.WriteLine($"{this.Outcomes.Count - 1}: {this.PlantSumValue}");
        }
    }

    internal class GrowthPattern
    {
        public string Pattern { get; private set; }

        public string Left { get; private set; }

        public string Center { get; private set; }

        public string Right { get; private set; }

        public string Outcome { get; private set; }

        public static GrowthPattern Parse(string input)
        {
            var regex = new Regex(@"(?<pattern>.*?) => (?<outcome>.*)");
            var match = regex.Match(input);

            string pattern = match.Groups["pattern"].Value;
            string lh = pattern.Substring(0, pattern.Length / 2);
            string rh = pattern.Substring(pattern.Length / 2 + 1, lh.Length);

            return new GrowthPattern()
            {
                Pattern = pattern,
                Outcome = match.Groups["outcome"].Value,
                Left = lh,
                Right = rh,
                Center = pattern.Replace(lh, string.Empty).Replace(rh, string.Empty)
            };
        }
    }
}
