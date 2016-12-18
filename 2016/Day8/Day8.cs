namespace AoC
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Day8 : IPuzzle
    {
        object IPuzzle.Answer()
        {
            var screen = new ScreenFixer();
            screen.ReadInstructionsFromCard(File.ReadAllLines("Day8/input.txt"));
            screen.SwipeCard();

            return screen.LitPixels;
        }

        public class ScreenFixer
        {
            public int LitPixels { get; set; }

            public int[,] ScreenPixels { get; set; } = new int[50, 6];

            public void ReadInstructionsFromCard(string[] instructions)
            {
                var factory = new LightFlipperFactory();

                foreach (var line in instructions)
                {
                    var cmd = factory.Create(line);
                    cmd.Flip(this.ScreenPixels);
                }
            }

            public void SwipeCard()
            {
                // cha-ching
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
