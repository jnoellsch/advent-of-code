namespace AoC
{
    using System.IO;
    using System.Linq;

    public class Day2 : IPuzzle
    {
        public object Answer()
        {
            var breaker = new NineDigitKeypadCodeBreaker();
            breaker.MustPeeNeedCodeNow(File.ReadAllLines("Day2/input.txt"));
            return breaker.DoorCode;
        }

        public class NineDigitKeypadCodeBreaker
        {
            public int[] Codes { get; private set; }

            public string DoorCode
            {
                get
                {
                    return this.Codes.Aggregate(string.Empty, (current, c) => string.Concat(current, c));
                }
            }

            public void MustPeeNeedCodeNow(string[] instructionLines)
            {
                this.Codes = new int[instructionLines.Length];

                for (var i = 0; i < instructionLines.Length; i++)
                {
                    this.Codes[i] = new NineDigitCracker().Crack(instructionLines[i]);
                }
            }
        }

        public class NineDigitCracker
        {
            private static readonly int[] UpBoundaries = { 1, 2, 3 };
            private static readonly int[] LeftBoundaries = { 1, 4, 7 };
            private static readonly int[] RightBoundaries = { 3, 6, 9 };
            private static readonly int[] DownBoundaries = { 7, 8, 9 };

            public int Number { get; private set; } = 5;

            public int Crack(string fingerPath)
            {
                foreach (char move in fingerPath)
                {
                    switch (move)
                    {
                        case 'U':
                            this.MoveUp();
                            break;
                        case 'D':
                            this.MoveDown();
                            break;
                        case 'L':
                            this.MoveLeft();
                            break;
                        case 'R':
                            this.MoveRight();
                            break;
                    }
                }

                return this.Number;
            }

            private void MoveRight()
            {
                if (this.CanExecuteMove(RightBoundaries))
                {
                    this.ExecuteMove(1);
                }
            }
            
            private void MoveLeft()
            {
                if (this.CanExecuteMove(LeftBoundaries))
                {
                    this.ExecuteMove(-1); 
                }
            }

            private void MoveUp()
            {
                if (this.CanExecuteMove(UpBoundaries))
                {
                    this.ExecuteMove(-3); 
                }
            }

            private void MoveDown()
            {
                if (this.CanExecuteMove(DownBoundaries))
                {
                    this.ExecuteMove(3);
                }
            }

            private void ExecuteMove(int i)
            {
                this.Number += i;
            }

            private bool CanExecuteMove(int[] boundaries)
            {
                bool notAtBoundary = !boundaries.Contains(this.Number);
                return notAtBoundary;
            }
        }
    }
}
