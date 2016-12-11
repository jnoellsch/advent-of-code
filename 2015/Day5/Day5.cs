namespace AoC
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day5 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string[] allLines = File.ReadAllLines("Day5/input.txt");
            var tracker = new NaughtNiceTracker();

            foreach (string line in allLines)
            {
                tracker.UpdateSantasList(line);
            }

            return tracker.NiceCount;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    public class NaughtNiceTracker
    {
        private IEnumerable<IPassRule> _passRules = new List<IPassRule>()
                                              {
                                                  new ThreeVowelRule(),
                                                  new DoubleOrMoreCharRule(),
                                                  new NeverAnyAbCdPqXyRule(),
                                              };

        public NaughtNiceTracker()
        {
            this.NaughtyCount = 0;
            this.NiceCount = 0;
        }

        public int NiceCount { get; private set; }

        public int NaughtyCount { get; private set; }

        public bool IsNaughty(string input)
        {
            return !this.IsNice(input);
        }

        public bool IsNice(string input)
        {
            return this._passRules.All(r => r.Passes(input));
        }

        public void UpdateSantasList(string input)
        {
            if (this.IsNice(input))
            {
                this.NiceCount++;
                Debug.WriteLine(input);
            }
            else
            {
                this.NaughtyCount++;
            }
        }
    }

    public interface IPassRule
    {
        bool Passes(string input);
    }

    public interface IFailRule
    {
        bool Fails(string input);
    }

    public class ThreeVowelRule : IPassRule
    {
        public bool Passes(string input)
        {
            Regex regex = new Regex("[aeiou]");
            return regex.Matches(input).Count >= 3;
        }
    }

    public class DoubleOrMoreCharRule : IPassRule
    {
        public bool Passes(string input)
        {
            // my regex is terrible... :(
            ////Regex regex = new Regex("\\w{2,}?");
            ////return regex.IsMatch(input);

            var candidates = new List<string> { "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "jj", "kk", "ll", "mm", "nn", "oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "ww", "xx", "yy", "zz" };
            return candidates.Any(input.Contains);
        }
    }

    public class NeverAnyAbCdPqXyRule : IPassRule
    {
        public bool Passes(string input)
        {
            var candidates = new List<string> { "ab", "cd", "pq", "xy" };
            return !candidates.Any(input.Contains);
        }
    }
}
