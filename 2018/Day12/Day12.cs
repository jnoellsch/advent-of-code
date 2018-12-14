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

        private IList<GrowthPattern> Patterns = File.ReadAllLines("Day12/input.txt").Select(GrowthPattern.Parse).ToList();

        private IList<GrowthPattern> SamplePatterns = File.ReadAllLines("Day12/sampleinput.txt").Select(GrowthPattern.Parse).ToList();

        object IPuzzle.Answer()
        {
            var gt = new GreenThumb(this.SampleInitialState);
            gt.WithPatterns(this.SamplePatterns);
            gt.GrowCycles(3);
            gt.DebugOutcomes();

            return gt.PlantCount;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class GreenThumb
    {
        public string State { get; private set; }

        public IList<GrowthPattern> Patterns { get; private set; }

        public IList<string> Outcomes = new List<string>();

        public GreenThumb(string initialState)
        {
            this.InitialState = initialState;
            this.State = initialState;
            this.Outcomes.Add(this.State);
        }

        public string InitialState { get; set; }

        public void WithPatterns(IList<GrowthPattern> patterns)
        {
            this.Patterns = patterns;
        }

        public void GrowCycles(int numOfCycles)
        {
            for (int cycle = 0; cycle < numOfCycles; cycle++)
            {
                Console.WriteLine($"{cycle + 1} start length (State) = {this.State.Length}");

                var sb = new StringBuilder();

                for (int i = 0; i < this.State.Length; i++)
                {
                    var chunk = this.GenerateChunk(i);
                    var patternMath = this.Patterns.FirstOrDefault(_ => _.Pattern == chunk);
                    if (patternMath != null)
                    {
                        sb.Append(patternMath.Outcome);
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }

                Console.WriteLine($"{cycle + 1} end length (sb) = {sb.Length}");

                this.State = sb.ToString();
                this.Outcomes.Add(this.State);
            }
        }

        private string GenerateChunk(int i)
        {
            // if i = 0, add ".."
            // if i = 1, add "."
            // if i = len - 2, add "."
            // if i = len - 1, add ".."
            return new string(this.State.Skip(i).Take(5).ToArray());
        }

        public int PlantCount => this.State.Where(_ => _ == '#').Count();

        public void DebugOutcomes()
        {
            Console.WriteLine("--:            ");
            Console.WriteLine("--:           0");

            for (var i = 0; i < this.Outcomes.Count; i++)
            {
                Console.WriteLine($"{i:D2}: {this.Outcomes[i]}");
            }
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
