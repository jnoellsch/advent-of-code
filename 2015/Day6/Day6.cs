namespace AoC
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Day6 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string[] instructions = File.ReadAllLines("Day6/input.txt");
            var grizwolds = new LightGrid(1000);

            foreach (var instr in instructions)
            {
                grizwolds.FlipLights(LightCommand.Parse(instr));
            }

            return grizwolds.NeighborAnnonyanceLevel();
        }


        object IPuzzlePart2.Answer()
        {
            string[] instructions = File.ReadAllLines("Day6/input.txt");
            var grizwolds = new DimmableLightGrid(1000);

            foreach (var instr in instructions)
            {
                grizwolds.FlipLights(LightCommand.Parse(instr));
            }

            return grizwolds.NeighborAnnonyanceLevel();
        }

        public class LightGrid
        {
            protected int[,] Grid;

            public LightGrid(int size)
            {
                this.Size = size;
                this.StapleToHouse();
                this.ConservePower();
            }

            private void StapleToHouse()
            {
                this.Grid = new int[this.Size, this.Size];
            }

            public int Size { get; private set; }

            public void ConservePower()
            {
                this.Off(new Point(0, 0), new Point(this.Size - 1, this.Size - 1));
            }

            public virtual void On(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this.Grid[i, j] = 1;
                    }
                }
            }

            public virtual void Off(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this.Grid[i, j] = 0;
                    }
                }
            }

            public virtual void Toggle(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this.Grid[i, j] = this.Grid[i, j] == 1 ? 0 : 1;
                    }
                }
            }

            public int NeighborAnnonyanceLevel()
            {
                int sum = 0;

                for (int i = 0; i < this.Size; i++)
                {
                    for (int j = 0; j < this.Size; j++)
                    {
                        sum += this.Grid[i, j];
                    }
                }

                return sum;
            }

            public void FlipLights(LightCommand command)
            {
                command.Run(this);
            }
        }

        public class DimmableLightGrid : LightGrid
        {
            public DimmableLightGrid(int size)
                : base(size)
            {
            }

            public override void On(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this.Grid[i, j]++;
                    }
                }
            }

            public override void Off(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this.Grid[i, j] = Math.Max(this.Grid[i, j] - 1, 0);
                    }
                }
            }

            public override void Toggle(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this.Grid[i, j] = this.Grid[i, j] + 2;
                    }
                }
            }
        }

        public class LightCommand
        {
            private const string OffMode = "turn off";
            private const string OnMode = "turn on";
            private const string ToggleMode = "toggle";

            internal LightCommand()
            {
            }

            public Point Start { get; private set; }
            public Point End { get; private set; }
            public string Mode { get; private set; }

            public static LightCommand Parse(string command)
            {
                Regex regex = new Regex(@"(?<cmd>toggle|turn on|turn off) (?<c1>\d+),(?<c2>\d+) through (?<c3>\d+),(?<c4>\d+)");
                Match match = regex.Match(command);

                return new LightCommand()
                       {
                             Mode = match.Groups["cmd"].Value,
                             Start = new Point(Convert.ToInt32(match.Groups["c1"].Value), Convert.ToInt32(match.Groups["c2"].Value)),
                             End = new Point(Convert.ToInt32(match.Groups["c3"].Value), Convert.ToInt32(match.Groups["c4"].Value)),
                       };
            }

            public void Run(LightGrid grid)
            {
                switch (this.Mode)
                {
                    case OffMode:
                        grid.Off(this.Start, this.End);
                        break;
                    case OnMode:
                        grid.On(this.Start, this.End);
                        break;
                    case ToggleMode:
                        grid.Toggle(this.Start, this.End);
                        break;
                }
            }
        }
    }
}
