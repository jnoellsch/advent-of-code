namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day5 : IPuzzle, IPuzzlePart2
    {
        private string Polymers { get; } = File.ReadAllText("Day05/input.txt");

        object IPuzzle.Answer()
        {
            var analyzer = new PolymerAnalyzer();
            return analyzer.Reduce(this.Polymers);
        }

        object IPuzzlePart2.Answer()
        {
            var analyzer = new PolymerAnalyzer();
            return analyzer.Shortest(this.Polymers);
        }
    }

    internal class PolymerAnalyzer
    {
        protected List<string> ReactableUnits { get; } = new List<string>()
        {
            "Aa","Bb","Cc","Dd","Ee","Ff","Gg","Hh","Ii","Jj","Kk","Ll","Mm","Nn","Oo","Pp","Qq","Rr","Ss","Tt","Uu","Vv","Ww","Xx","Yy","Zz",
            "aA","bB","cC","dD","eE","fF","gG","hH","iI","jJ","kK","lL","mM","nN","oO","pP","qQ","rR","sS","tT","uU","vV","wW","xX","yY","zZ"
        };

        protected List<char> IndividualUnits { get; } = "abcdefghijklmnopqrstuvwxyz".ToList();

        public int Reduce(string polymer)
        {
            polymer = this.RunReducer(polymer);
            return polymer.Length;
        }

        public int Shortest(string polymer)
        {
            var results = new List<string>();

            foreach (var unit in this.IndividualUnits)
            {
                var polymerToModify = polymer;
                polymerToModify = polymerToModify.Replace(unit.ToString(), string.Empty).Replace(unit.ToString().ToUpper(), string.Empty);

                results.Add(this.RunReducer(polymerToModify));
            }

            return results.OrderBy(_ => _.Length).First().Length;
        }

        private string RunReducer(string polymer)
        {
            bool wasReduced;
            var prevPolymer = polymer;

            do
            {
                this.ReactableUnits.ForEach(u => polymer = polymer.Replace(u, string.Empty));

                wasReduced = prevPolymer.Length > polymer.Length;
                prevPolymer = polymer;
            }
            while (wasReduced);

            return polymer;
        }
    }
}
