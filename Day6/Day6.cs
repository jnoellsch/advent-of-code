namespace AoC
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    public class Day6 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string[] instructions = File.ReadAllLines("Day6/input.txt");
            var grizwalds = new LightGrid(1000);

            foreach (var instr in instructions)
            {
                grizwalds.FlipLights(LightCommand.Parse(instr));
            }

            return grizwalds.NeighborAnnonyanceLevel();
        }


        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }

        public class LightGrid
        {
            private const int OnFlag = 1;
            private const int OffFlag = 0;

            private int[,] _grid;

            public LightGrid(int size)
            {
                this.Size = size;
                this._grid = new int[size, size];
            }

            public int Size { get; private set; }

            public void On(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this._grid[i,j] = OnFlag;
                    }
                }    
            }

            public void Off(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this._grid[i,j] = OffFlag;
                    }
                }        
            }

            public void Toggle(Point start, Point end)
            {
                for (int i = start.X; i <= end.X; i++)
                {
                    for (int j = start.Y; j <= end.Y; j++)
                    {
                        this._grid[i,j] = this._grid[i,j] == OnFlag ? OffFlag : OnFlag;
                    }
                }    
            }

            public int NeighborAnnonyanceLevel()
            {
                int sum = 0;

                for (int i = 0; i < this.Size - 1; i++)
                {
                    for (int j = 0; j < this.Size - 1; j++)
                    {
                        sum += this._grid[i, j];
                    }
                }

                return sum;
            }

            public void FlipLights(LightCommand command)
            {
                command.Run(this);
            }
        }

        public class LightCommand
        {
            private const string OffMode = "off";
            private const string OnMode = "on";
            private const string ToggleMode = "toggle";

            internal LightCommand()
            {
            }

            public Point Start { get; private set; }
            public Point End { get; private set; }
            public string Mode { get; private set; }

            public static LightCommand Parse(string command)
            {
                // normalize input
                command = command.Replace("turn ", string.Empty);
                command = command.Replace(" through ", ",");

                // extract mode, two sets of coordindates
                string mode = command.Substring(0, command.IndexOf(" ", StringComparison.Ordinal));
                string[] coords = command.Replace(mode, string.Empty).Split(',');

                return new LightCommand
                         {
                             Mode = mode,
                             Start = new Point(Convert.ToInt32(coords[0]), Convert.ToInt32(coords[1])),
                             End = new Point(Convert.ToInt32(coords[2]), Convert.ToInt32(coords[3])),
                         };
            }

            public void Run(LightGrid grid)
            {
                switch (this.Mode)
                {
                    case OffMode:
                        grid.On(this.Start, this.End);
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
