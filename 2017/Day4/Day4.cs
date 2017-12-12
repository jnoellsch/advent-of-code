namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day4 : IPuzzle, IPuzzlePart2
    {
        public string[] Passphrases { get; } = File.ReadAllLines("Day4/input.txt");

        object IPuzzle.Answer()
        {
            var checker = new PassphraseChecker();
            checker.Evaluate(this.Passphrases);

            return checker.ValidCount;
        }

        object IPuzzlePart2.Answer()
        {
            var checker = new ComplexPassPhraseChecker();
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

            protected virtual bool AllWordsAreDistinct(string[] words) => words.Distinct().Count() == words.Length;

            public virtual void Evaluate(string[] passphrases)
            {
                foreach (var phrase in passphrases)
                {
                    var words = phrase.Split(' ');

                    if (this.AllWordsAreDistinct(words))
                    {
                        this.ValidPassphrases.Add(phrase);
                    }
                }
            }
        }

        /// <summary>
        /// Checks a bunch of passphrases for validity but words can be anagrams. A valid passphrases must contain no duplicate words 
        /// or words with letters that can be rearranged to form other words. 
        /// </summary>
        public class ComplexPassPhraseChecker : PassphraseChecker
        {
            protected override bool AllWordsAreDistinct(string[] words)
            {
                var alphabeticalWords = words.Select(w => new string(w.ToCharArray().OrderBy(c => c).ToArray()));
                return alphabeticalWords.Distinct().Count() == words.Length;
            }
        }
    }
}
