namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AoC.Common;

    public class Day5 : IPuzzle, IPuzzlePart2
    {
        private string Polymers { get; } = File.ReadAllText("Day5/input.txt");

        object IPuzzle.Answer()
        {
            var analyzer = new PolymerAnalyzer();
            analyzer.Reduce(this.Polymers);

            return analyzer.UnitsRemaining;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class PolymerAnalyzer
    {
        protected List<string> ReactableUnits { get; } = new List<string>()
        {
            "aA","bB","cC","dD","eE","fF","gG","hH","iI","jJ","kK","lL","mM","nN","oO","pP","qQ","rR","sS","tT","uU","vV","wW","xX","yY","zZ",
            "Aa","Bb","Cc","Dd","Ee","Ff","Gg","Hh","Ii","Jj","Kk","Ll","Mm","Nn","Oo","Pp","Qq","Rr","Ss","Tt","Uu","Vv","Ww","Xx","Yy","Zz"
        };

        public void Reduce(string polymer)
        {
            bool wasReduced;
            var prevPolymer = polymer;

            do
            {
                this.ReactableUnits.ForEach(u => polymer = polymer.Replace(u,string.Empty));

                wasReduced = prevPolymer.Length > polymer.Length;
                prevPolymer = polymer;
            }
            while (wasReduced);

            this.UnitsRemaining = polymer.Length;
        }

        public int UnitsRemaining { get; set; }
    }
}
