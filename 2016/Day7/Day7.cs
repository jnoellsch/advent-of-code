namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Takes a set of "IP addresses" and determines if they are TLS or SSL based on various ABBA or ABA patterns.
    /// http://adventofcode.com/2016/day/7
    /// </summary>
    public class Day7 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var finder = new EbhqIpAddressSniffer<TlsIpLine>();
            finder.Sniff(File.ReadAllLines("Day7/input.txt"));
            return finder.Hits.Count;
        }

        object IPuzzlePart2.Answer()
        {
            var finder = new EbhqIpAddressSniffer<SslIpLine>();
            finder.Sniff(File.ReadAllLines("Day7/input.txt"));
            return finder.Hits.Count;
        }

        public class EbhqIpAddressSniffer<T> where T : IpLine, new()
        {
            public IList<T> Hits { get; } = new List<T>();

            public void Sniff(string[] ipLines)
            {
                foreach (var line in ipLines)
                {
                    var ipLine = new T();
                    ipLine.Parse(line);

                    if (ipLine.SupportsProtocol)
                    {
                        this.Hits.Add(ipLine);
                    }
                }
            }
        }

        public class SslIpLine : IpLine
        {
            public override bool SupportsProtocol => this.AbaBabChecker();

            private IList<char[]> SupernetAbas { get; } = new List<char[]>();

            private bool AbaBabChecker()
            {
                // find all ABA sequences in supernets
                foreach (string h in this.Supernets)
                {
                    int i = 0;
                    while (i < h.Length - 2)
                    {
                        var chunk = h.Skip(i).Take(3).ToArray();
                        if (this.IsAba(chunk))
                        {
                            this.SupernetAbas.Add(chunk);
                        }

                        i++;
                    }
                }

                // convert them all to BAB sequences
                var babCandiates = this.SupernetAbas.Select(x => new string(this.AbaToBab(x))).ToList();

                // match BAB sequences in hypernets 
                foreach (string h in this.Hypernets)
                {
                    int i = 0;
                    while (i < h.Length - 2)
                    {
                        var chunk = h.Skip(i).Take(3).ToArray();
                        if (this.IsBabMatch(chunk, babCandiates))
                        {
                            return true;
                        }

                        i++;
                    }
                }
                // nothing found (i.e. no ABA in supernets match BAB in hypernets)
                return false;
            }

            private bool IsBabMatch(char[] chunk, IEnumerable<string> candiates)
            {
                if (chunk.Length != 3) throw new ArgumentOutOfRangeException(nameof(chunk), "Array is not 3 characters.");
                
                return candiates.Any(x => x.Equals(new string(chunk)));
            }

            private bool IsAba(char[] chunk)
            {
                if (chunk.Length != 3) throw new ArgumentOutOfRangeException(nameof(chunk), "Array is not 3 characters.");

                return chunk[0] == chunk[2] && chunk[0] != chunk[1];
            }

            private char[] AbaToBab(char[] chunk)
            {
                if (chunk.Length != 3) throw new ArgumentOutOfRangeException(nameof(chunk), "Array is not 3 characters.");

                return new[] { chunk[1], chunk[0], chunk[1] };
            }
        }

        public class TlsIpLine : IpLine
        {
            public override bool SupportsProtocol => this.AbbaInSupernets() && this.NoAbbaInHypernets();

            private bool NoAbbaInHypernets() => !this.AbbaChecker(this.Hypernets);

            private bool AbbaInSupernets() => this.AbbaChecker(this.Supernets);

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

        public abstract class IpLine
        {
            public IList<string> Supernets { get; private set; }

            public IList<string> Hypernets { get; private set; }

            public abstract bool SupportsProtocol { get; }

            public void Parse(string line)
            {
                var regexHypernets = new Regex(@"\[(\w*?)\]");
                var hypernets = (from Match h in regexHypernets.Matches(line) select h.Value.Replace("[", "").Replace("]", "")).ToList();

                this.Hypernets = hypernets;
                this.Supernets = regexHypernets.Replace(line, "-").Split('-');
            }
        }
    }
}
