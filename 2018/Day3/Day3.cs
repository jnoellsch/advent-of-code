namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AoC.Common;

    public class Day3 : IPuzzle, IPuzzlePart2
    {
        private IEnumerable<Claim> Claims { get; } = File.ReadAllLines("Day3/input.txt").Select(Claim.Parse);

        object IPuzzle.Answer()
        {
            var measurer = new FabricMeasurer();
            measurer.Plot(this.Claims);

            return measurer.OverlapArea;
        }

        object IPuzzlePart2.Answer()
        {
            var measurer = new FabricMeasurer();
            measurer.Plot(this.Claims);

            return measurer.SoloClaim.Id;
        }
    }

    internal class FabricMeasurer
    {
        private const int MaxWidth = 1000;
        private const int MaxHeight = 1000;
        
        public string[,] Fabric { get; set; } = new string[MaxWidth,MaxHeight];

        public object OverlapArea { get; set; }

        public Claim SoloClaim { get; set; }

        public void Plot(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                this.MarkUpFabric(claim);
            }

            this.OverlapArea = this.CalculateArea();
            this.SoloClaim = this.FindSoloClaim(claims);
        }

        private Claim FindSoloClaim(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                bool hasOverlap = false;

                for (int i = claim.OffsetX; i < claim.OffsetX + claim.Width; i++)
                for (int j = claim.OffsetY; j < claim.OffsetY + claim.Height; j++)
                {
                    if (this.Fabric[i, j] == "X")
                    {
                        hasOverlap = true;
                        break;
                    }
                }

                if (!hasOverlap)
                {
                    return claim;
                }
            }

            throw new Exception("No overlap found. :'(");
        }

        private int CalculateArea()
        {
            int area = 0;

            for (int i = 0; i < MaxWidth; i++)
            for (int j = 0; j < MaxHeight; j++)
            {
                if (this.Fabric[i, j] == "X")
                {
                    area++;
                }
            }

            return area;
        }

        private void MarkUpFabric(Claim claim)
        {
            for (int i = claim.OffsetX; i < claim.OffsetX + claim.Width; i++)
            for (int j = claim.OffsetY; j < claim.OffsetY + claim.Height; j++)
            {
                string current = this.Fabric[i, j];
                if (string.IsNullOrEmpty(current))
                {
                    this.Fabric[i, j] = "1";
                }
                else if (current == "1")
                {
                    this.Fabric[i, j] = "X";
                }
            }
        }
    }

    internal class Claim
    {
        public string Id { get; private set; }
        public int OffsetX { get; private set; }
        public int OffsetY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public static Claim Parse(string input)
        {
            Regex regex = new Regex(@"#(?<id>\d+) @ (?<ox>\d+),(?<oy>\d+): (?<w>\d+)x(?<h>\d+)");
            Match match = regex.Match(input);

            return new Claim()
                   {
                       Id = match.Groups["id"].Value,
                       OffsetX = Convert.ToInt32(match.Groups["ox"].Value),
                       OffsetY = Convert.ToInt32(match.Groups["oy"].Value),
                       Width = Convert.ToInt32(match.Groups["w"].Value),
                       Height = Convert.ToInt32(match.Groups["h"].Value)
                   };
        }
    }
}
