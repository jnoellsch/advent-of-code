namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using AoC.Common;

    public class Day11 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var pp = new PowerPicker(9005);
            pp.Calculate();

            return pp.BestFuelCell;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class PowerPicker
    {
        private const int GridSize = 300;

        public PowerPicker(int serialNumber)
        {
            this.SerialNumber = serialNumber;
        }

        public int SerialNumber { get; }

        protected int[,] FuelGrid { get; } = new int[GridSize, GridSize];

        private Dictionary<Point, int> FuelSums { get; } = new Dictionary<Point, int>(GridSize);

        public string BestFuelCell { get; private set; }

        public void Calculate()
        {
            this.Fill();
            this.Sum();

            var best = this.FuelSums.OrderByDescending(_ => _.Value).First().Key;
            this.BestFuelCell = $"{best.X},{best.Y}"; 
        }

        public int CalculateCell(int x, int y)
        {
            var rackId = x + 10;
            var power = rackId * y;
            var powerWithSerial = power + this.SerialNumber;
            var powerTimesRack = powerWithSerial * rackId;
            var hundredsDigit = Math.Abs(powerTimesRack / 100 % 10);
            var result = hundredsDigit - 5;

            return result;
        }

        protected void Fill()
        {
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    this.FuelGrid[x, y] = this.CalculateCell(x + 1, y + 1);
                }
            }
        }

        private void Sum()
        {
            for (int y = 0; y < GridSize - 3; y++)
            {
                for (int x = 0; x < GridSize - 3; x++)
                {
                    this.FuelSums.Add(new Point(x + 1, y + 1), this.PowerOf3X3CellGrid(x, y));
                }
            }
        }

        private int PowerOf3X3CellGrid(int startX, int startY)
        {
            int sum = 0;

            for (int y = startY; y < startY + 3; y++)
            {
                for (int x = startX; x < startX + 3; x++)
                {
                    sum += this.FuelGrid[x, y];
                }
            }

            return sum;
        }
    }
}
