namespace AoC
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day7 : IPuzzle
    {
        object IPuzzle.Answer()
        {
            var finder = new EbhqIpAddressSniffer();
            finder.LoadData(File.ReadAllLines("Day7/input.txt"));
            return finder.SupportTls.Count;
        }

        private class EbhqIpAddressSniffer
        {
            public IList<IpLine> SupportTls { get; set; } = new List<IpLine>();

            public void LoadData(string[] ipLines)
            {
                foreach (var line in ipLines)
                {
                    var ipLine = IpLine.Parse(line);
                    if (ipLine.SupportsTls)
                    {
                        this.SupportTls.Add(ipLine);
                    }
                }
            }
        }

        public class IpLine
        {
            private IpLine()
            {
            }

            public IList<string> Others { get; private set; }

            public IList<string> Hypernets { get; private set; }

            public bool SupportsTls => this.AbbaInOthers() && this.NoAbbaInHypernets();

            public static IpLine Parse(string line)
            {
                var regexHypernets = new Regex(@"\[(\w*?)\]");
                var hypernets = (from Match h in regexHypernets.Matches(line) select h.Value.Replace("[", "").Replace("]", "")).ToList();

                return new IpLine()
                {
                    Hypernets = hypernets,
                    Others = regexHypernets.Replace(line, "-").Split('-')
                };
            }

            public bool NoAbbaInHypernets() => !this.AbbaChecker(this.Hypernets);

            public bool AbbaInOthers() => this.AbbaChecker(this.Others);

            private bool AbbaChecker(IList<string> stringChunks)
            {
                foreach (string h in stringChunks)
                {
                    int i = 0;
                    while (i < h.Length - 3)
                    {
                        if (this.IsAbba(h.Skip(i).Take(4).ToArray()))
                        {
                            return true;
                        }

                        i++;
                    }
                }

                // nothing found
                return false;
            }

            private bool IsAbba(char[] chunk)
            {
                if (chunk.Length != 4) throw new ArgumentOutOfRangeException(nameof(chunk), "ABBA test array is not 4 characters.");

                bool fulfillsPattern = chunk[0] == chunk[3] && chunk[1] == chunk[2];
                bool interiorCharsDiff = chunk[0] != chunk[1];

                return fulfillsPattern && interiorCharsDiff;
            }
        }
    }
}
