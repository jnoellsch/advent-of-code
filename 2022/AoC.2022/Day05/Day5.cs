using AoC.Common;
using System.Text.RegularExpressions;

namespace AoC
{
    internal class Day5 : IPuzzle, IPuzzlePart2
    {
        private IEnumerable<CraneMovement> Movements = File.ReadAllLines("Day05/input.txt").Where(line => line.StartsWith("move")).Select(CraneMovement.Parse);

        ////private IList<LinkedList<string>> StartingStacks = new List<LinkedList<string>>()
        ////{
        ////    new LinkedList<string>(new string[] {"Z","N"}),
        ////    new LinkedList<string>(new string[] {"M","C","D"}),
        ////    new LinkedList<string>(new string[] {"P"})
        ////};

        private IList<LinkedList<string>> StartingStacks = new List<LinkedList<string>>()
        {
            new LinkedList<string>(new string[] {"F","D","B","Z","T","J","R","N"}),
            new LinkedList<string>(new string[] {"R","S","N","J","H"}),
            new LinkedList<string>(new string[] {"C","R","N","J","G","Z","F","Q"}),
            new LinkedList<string>(new string[] {"F","V","N","G","R","T","Q" }),
            new LinkedList<string>(new string[] {"L","T","Q","F"}),
            new LinkedList<string>(new string[] {"Q","C","W","Z","B","R","G","N"}),
            new LinkedList<string>(new string[] {"F","C","L","S","N","H","M"}),
            new LinkedList<string>(new string[] {"D","N","Q","M","T","J"}),
            new LinkedList<string>(new string[] {"P","G","S"}),
        };

        object IPuzzle.Answer()
        {
            var mvr = new CrateMover(this.StartingStacks);
            mvr.Run(this.Movements);
            return mvr.GetTopCrates();
        }

        object IPuzzlePart2.Answer()
        {
            var mvr = new CrateMover9001(this.StartingStacks);
            mvr.Run(this.Movements);
            return mvr.GetTopCrates();
        }

        internal class CrateMover9001 : CrateMover
        {
            public CrateMover9001(IList<LinkedList<string>> startingStacks) : base(startingStacks)
            {
            }

            internal override void Run(IEnumerable<CraneMovement> movements)
            {
                foreach (var move in movements)
                {
                    var startStack = this.Stacks[move.StartPos - 1];
                    var endStack = this.Stacks[move.EndPos - 1];

                    // grab from start, copy to end, delete from start
                    var cratesToMove = startStack.TakeLast(move.Amount);
                    cratesToMove.ForEach(s => endStack.AddLast(s));
                    for (int i = 0; i < move.Amount; i++)
                    {
                        startStack.RemoveLast();
                    }
                }
            }
        }

        internal class CrateMover
        {
            public IList<LinkedList<string>> Stacks { get; private set; }

            public CrateMover(IList<LinkedList<string>> startingStacks)
            {
                this.Stacks = startingStacks;
            }

            internal virtual void Run(IEnumerable<CraneMovement> movements)
            {
                foreach (var move in movements)
                {
                    var startStack = this.Stacks[move.StartPos - 1];
                    var endStack = this.Stacks[move.EndPos - 1];

                    // grab from start, copy to end, delete from start.
                    // reverse chunked crates to move since they are technically moved 1 by 1
                    var cratesToMove = startStack.TakeLast(move.Amount);
                    cratesToMove.Reverse().ForEach(s => endStack.AddLast(s));
                    for (int i = 0; i < move.Amount; i++)
                    {
                        startStack.RemoveLast();
                    }
                }
            }

            internal string GetTopCrates()
            {
                string topCrates = string.Empty;
                this.Stacks.ForEach(s => topCrates = string.Concat(topCrates, s.Last()));
                return topCrates;
            }
        }

        internal class CraneMovement
        {
            public int Amount { get; init; }

            public int StartPos { get; init; }

            public int EndPos { get; init; }

            internal static CraneMovement Parse(string input)
            {
                Regex regex = new Regex(@"move (?<amt>\d+) from (?<start>\d+) to (?<end>\d+)");
                Match match = regex.Match(input);

                return new CraneMovement()
                {
                    Amount = Convert.ToInt32(match.Groups["amt"].Value),
                    StartPos = Convert.ToInt32(match.Groups["start"].Value),
                    EndPos = Convert.ToInt32(match.Groups["end"].Value)
                };
            }
        }
    }
}