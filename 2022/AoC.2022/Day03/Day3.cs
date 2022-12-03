namespace AoC
{
    using AoC.Common;

    internal class Day3 : IPuzzle, IPuzzlePart2
    {
        private IEnumerable<string> Rucksacks = File.ReadAllLines("Day03/input.txt");

        object IPuzzle.Answer()
        {
            var reorger = new RuckackReorganizer();
            return reorger.SumPriorities(this.Rucksacks);
        }

        object IPuzzlePart2.Answer()
        {
            var reorger = new GroupedRucksackReorganizer();
            return reorger.SumPriorities(this.Rucksacks);
        }
    }

    internal class GroupedRucksackReorganizer : RuckackReorganizer
    {
        public override int SumPriorities(IEnumerable<string> rucksacks)
        {
            int sum = 0;

            for (int i = 0; i < rucksacks.Count(); i += 3)
            {
                var groupOfThree = rucksacks.Skip(i).Take(3).ToArray();
                
                foreach (var c in groupOfThree[0])
                {
                    if (groupOfThree[1].Contains(c) && groupOfThree[2].Contains(c))
                    {
                        sum += ConvertToAlphabetValue(c);
                        break;
                    }
                }
            }

            return sum;
        }
    }

    internal class RuckackReorganizer
    {
        public virtual int SumPriorities(IEnumerable<string> rucksacks)
        {
            int sum = 0;

            foreach (var r in rucksacks)
            {
                var compartmentRucksack = r.SplitInHalf();
                var overlap = compartmentRucksack.Left.Intersect(compartmentRucksack.Right);
                sum += overlap.Select(c => ConvertToAlphabetValue(c)).Sum();
            }

            return sum;
        }

        /// <summary>
        /// Lowercase item types a through z have priorities 1 through 26. Uppercase item types A through Z have priorities 27 through 52.
        /// </summary>
        protected int ConvertToAlphabetValue(char c)
        {
            return Char.IsLower(c) ? c.ToInt() - 96 : c.ToInt() - 38;
        }
    }
}
