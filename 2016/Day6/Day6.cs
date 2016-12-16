namespace AoC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day6 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var dejammer = new SantaMessageDejammer();
            return dejammer.UnJam(File.ReadAllLines("Day6/input.txt"), new MostFrequentLetter());
        }

        object IPuzzlePart2.Answer()
        {
            var dejammer = new SantaMessageDejammer();
            return dejammer.UnJam(File.ReadAllLines("Day6/input.txt"), new LeastFrequentLetter());
        }

        public class SantaMessageDejammer
        {
            public string UnJam(string[] msgRows, IDejammingStrategy strategy)
            {
                var msgCols = msgRows.Transpose().ToList();
                string realMsg = string.Empty;

                foreach (var col in msgCols)
                {
                    realMsg += strategy.Run(col);
                }

                return realMsg;
            }
        }

        public interface IDejammingStrategy
        {
            char Run(IEnumerable<char> src);
        }

        public class MostFrequentLetter : IDejammingStrategy
        {
            public char Run(IEnumerable<char> src) => src.GroupBy(x => x).Select(x => new { Letter = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).First().Letter;
        }

        public class LeastFrequentLetter : IDejammingStrategy
        {
            public char Run(IEnumerable<char> src) => src.GroupBy(x => x).Select(x => new { Letter = x.Key, Count = x.Count() }).OrderBy(x => x.Count).First().Letter;
        }
    }
}
