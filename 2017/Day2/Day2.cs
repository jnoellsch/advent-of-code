namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day2 : IPuzzle
    {
        object IPuzzle.Answer()
        {
            var input = File.ReadAllLines("Day2/input.txt");

            var decorrupter = new SpreadsheetDecorrupter();
            decorrupter.Inspect(input);

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

        public class HighLowRowData : IRowData
        {
            private HighLowRowData(int high, int low)
            {
                this.High = high;
                this.Low = low;
            }

            public int High { get; }

            public int Low { get; }

            public int Outcome => this.High - this.Low;

            public static IRowData Create(int[] numbers)
            {
                return new HighLowRowData(numbers.Max(), numbers.Min());
            }
        }

        public interface IRowData
        {
            int Outcome { get; }
        }
    }
}
