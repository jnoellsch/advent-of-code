namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day2 : IPuzzle, IPuzzlePart2
    {
        private string[] Input { get; } = File.ReadAllLines("Day2/input.txt");

        object IPuzzle.Answer()
        {
            var decorrupter = new SpreadsheetDecorrupter();
            decorrupter.Inspect(this.Input);

            return decorrupter.Checksum;
        }

        object IPuzzlePart2.Answer()
        {
            var decorrupter = new YourChangeInRequirementsSpreadsheetDecorrupter();
            decorrupter.Inspect(this.Input);

            return decorrupter.Checksum;
        }

        /// <summary>
        /// Calculates a spreadsheet's checksum by determining the difference between the largest value and the 
        /// smallest value within a row and then summing all of those differences.
        /// </summary>
        public class SpreadsheetDecorrupter
        {
            public List<IRowData> RowData { get; } = new List<IRowData>();

            public int Checksum { get; private set; }

            public virtual void Inspect(string[] rows)
            {
                this.SplitAndConvertLineStrings(rows);
                this.GenerateChecksum();
            }

            protected virtual void ToRowDataFrom(IEnumerable<int> columns)
            {
                this.RowData.Add(HighLowRowData.Create(columns.ToArray()));
            }

            protected virtual void GenerateChecksum()
            {
                this.Checksum = this.RowData.Aggregate(0, (current, row) => current + row.Outcome);
            }

            private void SplitAndConvertLineStrings(string[] rows)
            {
                foreach (var row in rows)
                {
                    var columnsAsNumbers = row.Split('\t').Select(c => Convert.ToInt32(c));
                    this.ToRowDataFrom(columnsAsNumbers);
                }
            }
        }

        /// <summary>
        /// Calculates a spreadsheet's checksum by instead determining the openly divisible pair within a row 
        /// and then summing all of those divided outcomes.
        /// </summary>
        public class YourChangeInRequirementsSpreadsheetDecorrupter : SpreadsheetDecorrupter
        {
            protected override void ToRowDataFrom(IEnumerable<int> columns)
            {
                this.RowData.Add(EvenDivisibleRowData.Create(columns.ToArray()));
            }
        }

        public class HighLowRowData : IRowData
        {
            protected HighLowRowData(int high, int low)
            {
                this.High = high;
                this.Low = low;
            }

            public int High { get; }

            public int Low { get; }

            public virtual int Outcome => this.High - this.Low;

            public static IRowData Create(int[] numbers)
            {
                return new HighLowRowData(numbers.Max(), numbers.Min());
            }
        }

        public class EvenDivisibleRowData : HighLowRowData
        {
            protected EvenDivisibleRowData(int high, int low)
                : base(high, low)
            {
            }

            public override int Outcome => base.High / base.Low;

            public new static IRowData Create(int[] numbers)
            {
                var lowToHighNumbers = numbers.OrderBy(n => n).ToList();

                for (int i = lowToHighNumbers.Count - 1; i >= 0; i--)
                {
                    var high = lowToHighNumbers[i];
                    var low = lowToHighNumbers.Where(x => x != high).FirstOrDefault(x => high / (decimal)x % 1 == 0);

                    if (low != default(int))
                    {
                       return new EvenDivisibleRowData(high, low);
                    }
                }

                throw new Exception("Dammit, Jim. No evenly divisible pair was found. Your algorithim sucks. :(");
            }
        }

        public interface IRowData
        {
            int Outcome { get; }
        }
    }
}
