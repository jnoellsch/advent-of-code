namespace AoC
{
    using AoC.Common;

    internal class Day1 : IPuzzle, IPuzzlePart2
    {
        private IEnumerable<string> CalorieItems { get; } = File.ReadAllLines("Day01/input.txt");

        object IPuzzle.Answer()
        {
            var counter = new CalorieCounter();
            return counter.FindMax(this.CalorieItems);
        }

        object IPuzzlePart2.Answer()
        {
            var counter = new TopThreeCalorieCounter();
            return counter.FindMax(this.CalorieItems);
        }
    }

    internal class CalorieCounter
    {
        public IList<int> Sums { get; set; } = new List<int>();

        public virtual int FindMax(IEnumerable<string> calorieItems)
        {
            ParseSums(calorieItems);
            return this.Sums.Max();
        }

        protected void ParseSums(IEnumerable<string> calorieItems)
        {
            var runningSum = 0;

            foreach (var i in calorieItems)
            {
                int calorieAmt;
                if (int.TryParse(i, out calorieAmt))
                {
                    runningSum += calorieAmt;
                }
                else
                {
                    this.Sums.Add(runningSum);
                    runningSum = 0;
                }
            }
        }
    }

    internal class TopThreeCalorieCounter : CalorieCounter
    {
        public override int FindMax(IEnumerable<string> calorieItems)
        {
            ParseSums(calorieItems);
            return this.Sums.OrderByDescending(_ => _).Take(3).Sum();
        }
    }
}
