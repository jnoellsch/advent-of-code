namespace AoC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day4 : IPuzzle
    {
        public string[] Passphrases { get; } = File.ReadAllLines("Day4/input.txt");

        object IPuzzle.Answer()
        {
            var checker = new PassphraseChecker();
            checker.Evaluate(this.Passphrases);

            return checker.ValidCount;
        }

        /// <summary>
        /// Checks a bunch of passphrases for validity. A valid passphrase must contain no duplicate words.
        /// </summary>
        public class PassphraseChecker
        {
            public int ValidCount => this.ValidPassphrases.Count;

            public List<string> ValidPassphrases { get; } = new List<string>();

            public void Evaluate(string[] passphrases)
            {
                foreach (var phrase in passphrases)
                {
                    var words = phrase.Split(' ');
                    bool allWordsDistinct = words.Distinct().Count() == words.Length;

                    if (allWordsDistinct)
                    {
                        this.ValidPassphrases.Add(phrase);
                    }
                }
            }
        }
    }
}
