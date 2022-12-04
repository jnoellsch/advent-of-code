namespace AoC
{
    using AoC.Common;
    using System.Text.RegularExpressions;

    internal class Day4 : IPuzzle, IPuzzlePart2
    {
        private IEnumerable<WorkPair> WorkPairings = File.ReadAllLines("Day04/input.txt").Select(WorkPair.Parse);

        object IPuzzle.Answer() => this.WorkPairings.Count(wp => wp.HasOverlap());

        object IPuzzlePart2.Answer() => this.WorkPairings.Count(wp => wp.HasPartialOverlap());
    }

    internal class WorkPair
    {
        public (int Low, int High) Elf1 { get; private set; }
        
        public (int Low, int High) Elf2 { get; private set; }

        public bool HasOverlap()
        {
            var elf1Range = MakeRange(this.Elf1.Low, this.Elf1.High);
            var elf2Range = MakeRange(this.Elf2.Low, this.Elf2.High);

            var overlap = elf1Range.Intersect(elf2Range);
            return elf1Range.SequenceEqual(overlap) || elf2Range.SequenceEqual(overlap);
        }

        public bool HasPartialOverlap()
        {
            var elf1Range = MakeRange(this.Elf1.Low, this.Elf1.High);
            var elf2Range = MakeRange(this.Elf2.Low, this.Elf2.High);

            return elf1Range.Intersect(elf2Range).Any();
        }

        public static WorkPair Parse(string input)
        {
            Regex regex = new Regex(@"(?<e1l>\d+)-(?<e1h>\d+),(?<e2l>\d+)-(?<e2h>\d+)");
            Match match = regex.Match(input);

            return new WorkPair()
            {
                Elf1 = (Convert.ToInt32(match.Groups["e1l"].Value), Convert.ToInt32(match.Groups["e1h"].Value)),
                Elf2 = (Convert.ToInt32(match.Groups["e2l"].Value), Convert.ToInt32(match.Groups["e2h"].Value))
            };
        }

        private IEnumerable<int> MakeRange(int low, int high) => Enumerable.Range(low, (high - low) + 1);
    }
}
