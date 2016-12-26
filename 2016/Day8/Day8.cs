namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day8 : IPuzzle
    {
        private const int ON = 1;
        private const int OFF = 0;
        ////private const int ROW_SIZE = 50;
        ////private const int COLUMN_SIZE = 6;
        private const int ROW_DEPTH = 7;
        private const int COLUMN_DEPTH = 3;

        object IPuzzle.Answer()
        {
            var screen = new ScreenFixer();
            screen.ReadInstructionsFromCard(File.ReadAllLines("Day8/sample.txt"));
            screen.SwipeCard();

            return screen.LitPixels;
        }

        public class ScreenFixer
        {
            public int LitPixels { get; set; }

            public int[,] ScreenPixels { get; set; } = new int[COLUMN_DEPTH, ROW_DEPTH];

            public void ReadInstructionsFromCard(string[] instructions)
            {
                var factory = new LightFlipperFactory();

                foreach (var line in instructions)
                {
                    var cmd = factory.Create(line);
                    cmd.Flip(this.ScreenPixels);

                    this.DisplayLights();
                }
            }

            public void SwipeCard()
            {
                // reset
                this.LitPixels = 0;

                // count  
                for (int i = 0; i < COLUMN_DEPTH; i++)
                {
                    for (int j = 0; j < ROW_DEPTH; j++)
                    {
                        if(this.ScreenPixels[i, j] == ON)
                        {
                            this.LitPixels++;
                        }
                    }
                }
            }

            private void DisplayLights()
            {
                for (int i = 0; i < COLUMN_DEPTH; i++)
                {
                    Console.Write("{0}: ", i);

                    for (int j = 0; j < ROW_DEPTH; j++)
                    {
                        Console.Write("{0}", this.ScreenPixels[i, j] == ON ? "#" : ".");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        public class LightFlipperFactory
        {
            public ILightFlipper Create(string line)
            {
                var regexAction = new Regex(@"(rect|rotate)");
                var regexRect = new Regex(@"(?:rect)\s+?(?<sizeX>\d*)x(?<sizeY>\d*)");
                var regexRotate = new Regex(@"(?:rotate)\s+?(?<rowcol>row|column)*?\s+?(?<xy>x|y)=(?<index>[0-9]+?)\s+?by\s+?(?<amount>[0-9]+?)");

                string action = regexAction.Match(line).Value;

                switch (action)
                {
                    case "rect":
                        var matchRect = regexRect.Match(line);
                        int x = Convert.ToInt32(matchRect.Groups["sizeX"].Value);
                        int y = Convert.ToInt32(matchRect.Groups["sizeY"].Value);

                        return new BlockLightFlipper(x, y);
                    case "rotate":
                        var matchRotate = regexRotate.Match(line);
                        string rowcol = matchRotate.Groups["rowcol"].Value;
                        int index = Convert.ToInt32(matchRotate.Groups["index"].Value);
                        int amount = Convert.ToInt32(matchRotate.Groups["amount"].Value);

                        return new RotatedLightFlipper(rowcol, index, amount);
                    default:
                        return new NoopLightFlipper();
                }
            }
        }

        public class NoopLightFlipper : ILightFlipper
        {
            public void Flip(int[,] pixels)
            {
                // noop
            }
        }

        public class BlockLightFlipper : ILightFlipper
        {
            public int X { get; }

            public int Y { get; }

            public BlockLightFlipper(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public void Flip(int[,] pixels)
            {
                for (int i = 0; i < this.X; i++)
                {
                    for (int j = 0; j < this.Y; j++)
                    {
                        pixels[j, i] = ON;
                    }
                }
            }
        }

        public class RotatedLightFlipper : ILightFlipper
        {
            public int Index { get; }

            public int Amount { get; }

            public MoveType Type { get; private set; }

            public RotatedLightFlipper(string rowcol, int index, int amount)
            {
                this.Type = rowcol == "row" ? MoveType.Row : MoveType.Column;
                this.Index = index;
                this.Amount = amount;
            }

            public void Flip(int[,] pixels)
            {
                if (this.Type == MoveType.Column)
                {
                    this.FlipColumn(pixels);
                }
                else if (this.Type == MoveType.Row)
                {
                    this.FlipRow(pixels);
                }
            }

            private void FlipRow(int[,] pixels)
            {
                // convert row to linked list
                var queue = new Queue<int>();
                for (int i = ROW_DEPTH - 1; i >= 0; i--)
                {
                    queue.Enqueue(pixels[this.Index, i]);
                }

                // shift
                this.ShiftQueue(queue);

                // convert queue back to array (reverse read)
                for (int i = ROW_DEPTH - 1; i >= 0; i--)
                {
                    pixels[this.Index, i] = queue.Dequeue();
                }
            }

            private void FlipColumn(int[,] pixels)
            {
                // convert column to queue
                var queue = new Queue<int>();
                for (int i = COLUMN_DEPTH - 1; i >= 0; i--)
                {
                    queue.Enqueue(pixels[i, this.Index]);
                }

                // shift
                this.ShiftQueue(queue);

                // convert queue back to array (reverse read)
                for (int i = COLUMN_DEPTH - 1; i >= 0; i--)
                {
                    pixels[i, this.Index] = queue.Dequeue();
                }
            }

            private void ShiftQueue(Queue<int> queue)
            {
                for (int i = 0; i < this.Amount; i++)
                {
                    queue.Enqueue(queue.Dequeue());
                }
            }

            public enum MoveType
            {
                Row,
                Column
            }
        }

        public interface ILightFlipper
        {
            void Flip(int[,] pixels);
        }
    }
}
