namespace AoC
{
    using AoC.Common;
    using System;
    using System.Security.Claims;
    using System.Text.RegularExpressions;

    internal class Day2 : IPuzzle, IPuzzlePart2
    {
        private IEnumerable<SubmarineInstruction> Movements = File.ReadAllLines("Day02/input.txt").Select(SubmarineInstruction.Parse);

        object IPuzzle.Answer()
        {
            var sub = new Submarine();
            sub.Move(this.Movements);
            return sub.Position;
        }

        object IPuzzlePart2.Answer()
        {
            var sub = new ShootingSubmarine();
            sub.Move(this.Movements);
            return sub.Position;
        }
    }

    internal class ShootingSubmarine : Submarine
    {
        public int Aim { get; private set; }

        public override void Move(IEnumerable<SubmarineInstruction> movements)
        {
            foreach (var m in movements)
            {
                if (m.IsHorizontalMove)
                {
                    this.Xpos += m.Amount;
                    this.Ypos += m.Amount * this.Aim;
                }
                else
                {   
                    this.Aim += m.Amount;
                }
            }
        }
    }

    internal class Submarine
    {
        public int Xpos { get; protected set; } = 0;
        public int Ypos { get; protected set; } = 0;
        public int Position => this.Xpos * this.Ypos;

        public virtual void Move(IEnumerable<SubmarineInstruction> movements)
        {
            foreach (var m in movements)
            {
                if(m.IsHorizontalMove)
                {
                    this.Xpos += m.Amount;
                }
                else
                {
                    this.Ypos += m.Amount;
                }
            }
        }
    }

    internal class SubmarineInstruction
    {
        public string Movement { get; private set; } = string.Empty;
        public int Amount { get; private set; } = -1;
        public bool IsHorizontalMove => this.Movement.Equals("forward", StringComparison.OrdinalIgnoreCase);

        internal static SubmarineInstruction Parse(string input)
        {
            Regex regex = new Regex(@"(?<mv>\w+) (?<amt>\d+)");
            Match match = regex.Match(input);

            int amt = Convert.ToInt32(match.Groups["amt"].Value);
            string mv = match.Groups["mv"].Value;

            return new SubmarineInstruction()
            {
                Movement = mv,
                Amount = IsNegativeMove(mv) ? -amt : amt
            };
        }

        private static bool IsNegativeMove(string mv) => mv.Equals("up", StringComparison.OrdinalIgnoreCase);
    }
}
