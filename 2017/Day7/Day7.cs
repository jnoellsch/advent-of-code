namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AoC.Common;

    public class Day7 : IPuzzle
    {
        public string[] StackInstructions = File.ReadAllLines("Day7/input.txt");

        public object Answer()
        {
            var disc = new DiscTower();
            disc.Parse(this.StackInstructions);

            return disc.FindBottom().Name;
        }

        public class DiscTower
        {
            public List<Disc> Discs { get; } = new List<Disc>();

            public void Parse(string[] stackInstructions)
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
