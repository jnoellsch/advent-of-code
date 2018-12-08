namespace Aoc
{
    using AoC.Common;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    public class Day8 : IPuzzle, IPuzzlePart2
    {
        private IList<int> Input { get; } = File.ReadAllText("Day8/input.txt").Split(' ').Select(int.Parse).ToList();

        object IPuzzle.Answer()
        {
            var checker = new LicenseFileChecker();
            checker.Process(this.Input);

            return checker.MetadataSum;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class LicenseFileChecker
    {
        private Node Root { get; set; }

        public void Process(IList<int> input)
        {
            this.Root = this.BuildTree(input); 
        }

        private Node BuildTree(IList<int> input)
        {
            // pop two entries: first = child count, second = metadata count
            int childQuantity = input[0];
            input.RemoveAt(0);

            int metadataQuantity = input[0];
            input.RemoveAt(0);

            // build node
            var node = new Node();

            if (childQuantity > 0)
            {
                for (int i = 0; i < childQuantity; i++)
                {
                    node.Children.Add(this.BuildTree(input));
                }
            }

            for (int i = 0; i < metadataQuantity; i++)
            {
                node.MetadataValues.Add(input[0]);
                input.RemoveAt(0);
            }

            return node;
        }

        public int MetadataSum => this.SumTreeMetadata(this.Root);

        private int SumTreeMetadata(Node node)
        {
            if (node.NoChildren)
            {
                return node.Sum;
            }

            return node.Children.Aggregate(node.Sum, (rollingSum, childNode) => rollingSum + this.SumTreeMetadata(childNode));
        }
    }

    internal class Node
    {
        public IList<Node> Children { get; set; } = new List<Node>();

        public IList<int> MetadataValues { get; set; } = new List<int>();

        public int Sum => this.MetadataValues.Sum();

        public bool HasChildren => this.Children.Any();

        public bool NoChildren => !this.HasChildren;
    }
}
