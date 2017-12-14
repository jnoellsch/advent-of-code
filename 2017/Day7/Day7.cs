namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AoC.Common;

    public class Day7 : IPuzzle, IPuzzlePart2
    {
        public string[] StackInstructions = File.ReadAllLines("Day7/input.txt");

        public Day7()
        {
            this.Tower = new DiscTower();
            this.Tower.Load(this.StackInstructions);
        }

        public DiscTower Tower { get; }

        object IPuzzle.Answer()
        {

            return this.Tower.FindBottom().Name;
        }

        object IPuzzlePart2.Answer()
        {
            return this.Tower.FindBalanceWeight();
        }

        public class DiscTower
        {
            public List<Disc> Discs { get; } = new List<Disc>();

            public void Load(string[] stackInstructions)
            {
                foreach (var instruc in stackInstructions)
                {
                    var d = Disc.Create(instruc);
                    this.Discs.Add(d);
                }
            }

            public Disc FindBottom()
            {
                // ignore anything that doesn't have "parents" (they have to be leafs)
                var importantDiscs = this.Discs.Where(d => d.ParentNames.Any()).ToList();

                // find disc that isn't in the parents set
                foreach (var d in importantDiscs)
                {
                    if (importantDiscs.Any(id => id.ParentNames.Contains(d.Name)))
                    {
                        // not at bottom
                    }
                    else
                    {
                        // at bottom!
                        return d;
                    }
                }

                throw new Exception("No bottom. :( Jim...c'mon, brah. Debug harder.");
            }

            public int FindBalanceWeight()
            {
                var sumTracker = new Dictionary<string, int>();
                var importantDiscs = this.Discs.Where(d => d.ParentNames.Any()).ToList();

                // calculate the sums
                foreach (var disc in importantDiscs)
                {
                    int totalWeight = disc.Weight;

                    foreach (var pname in disc.ParentNames)
                    {
                        totalWeight += this.Discs.First(d => d.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).Weight;
                    }

                    sumTracker.Add(disc.Name, totalWeight);
                }

                // find the highest, lowest, and their difference
                var lowest = sumTracker.OrderByDescending(x => x.Value).Last();
                var highest = sumTracker.OrderByDescending(x => x.Value).First();
                var weightDelta = highest.Value - lowest.Value;
                var fatAssDisc = this.Discs.First(d => d.Name.Equals(highest.Key, StringComparison.OrdinalIgnoreCase));

                return fatAssDisc.Weight - weightDelta;
            }
        }

        public class Disc
        {
            public string Name { get; private set; }

            public int Weight { get; private set; }

            public IEnumerable<string> ParentNames { get; private set; }

            public string RawInstruction { get; private set; }

            public static Disc Create(string instruction)
            {
                var regex = new Regex(@"(?<name>\w*?)\s{1}\((?<weight>\d*)\)( -> (?<parents>.*))?", RegexOptions.Compiled);
                var groups = regex.Match(instruction).Groups;

                string parentsRaw = groups["parents"].Value;

                return new Disc()
                {
                    Name = groups["name"].Value,
                    Weight = Convert.ToInt32(groups["weight"].Value),
                    ParentNames = !string.IsNullOrEmpty(parentsRaw) ? parentsRaw.Split(',').Select(x => x.Trim()) : Enumerable.Empty<string>(),
                    RawInstruction = instruction
                };
            }
        }

        ////public class TreeNode<T>
        ////{
        ////    private TreeNode(T data)
        ////    {
        ////        this.Data = data;
        ////    }

        ////    private T Data { get; }

        ////    private LinkedList<TreeNode<T>> Children { get; } = new LinkedList<TreeNode<T>>();

        ////    public TreeNode<T> Parent { get; private set; }

        ////    public TreeNode<T> AddChild(T data)
        ////    {
        ////        var node = new TreeNode<T>(data) { Parent = this };
        ////        this.Children.AddLast(node);

        ////        return node;
        ////    }
        ////}
    }
}
