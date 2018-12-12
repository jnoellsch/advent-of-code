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
            var upp = new UltimatePowerPicker(9005);
            upp.Calculate();

            return upp.BestFuelCell;
        }
    }

    internal class UltimatePowerPicker : PowerPicker
    {
        public UltimatePowerPicker(int serialNumber) : base(serialNumber)
        {
        }

        private IList<CellPointWithSize> FuelSums { get; } = new List<CellPointWithSize>();

        public override void Calculate()
        {
            this.Fill();
            this.Sum();

            var best = this.FuelSums.OrderByDescending(_ => _.Sum).First();
            this.BestFuelCell = $"{best.Point.X},{best.Point.Y},{best.Size}";
        }

        private void Sum()
        {
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    int maxIndex = Math.Max(x, y);
                    var point = new Point(x + 1, y + 1);

                    for (int size = 0; size + maxIndex < GridSize; size++)
                    {
                        if (size == 0)
                            continue;

                        var cpws = new CellPointWithSize()
                                   {
                                       Point = point, 
                                       Size = size + 1, 
                                       Sum = this.PowerOfGrid(x, y, size + 1)
                                   };

                        if(cpws.Sum > 0)
                            this.FuelSums.Add(cpws);
                    }
                }
            }
        }

        private long PowerOfGrid(int startX, int startY, int size)
        {
            long sum = 0;

            for (int y = startY; y < startY + size && y < GridSize; y++)
            {
                for (int x = startX; x < startX + size && x < GridSize; x++)
                {
                    sum += this.FuelGrid[x,y];
                }
            }

            return sum;
        }

        private class CellPointWithSize
        {
            public Point Point { get; set; }
            public int Size { get; set; }
            public long Sum { get; set; }
        }
    }

    internal class PowerPicker
    {
        protected const int GridSize = 300;

        public PowerPicker(int serialNumber)
        {
            this.SerialNumber = serialNumber;
        }

        public int SerialNumber { get; }

        protected int[,] FuelGrid { get; } = new int[GridSize, GridSize];

        private Dictionary<Point, int> FuelSums { get; } = new Dictionary<Point, int>(GridSize);

        public string BestFuelCell { get; protected set; }

        public virtual void Calculate()
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
