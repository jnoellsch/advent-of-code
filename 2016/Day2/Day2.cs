namespace AoC
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Determines the door code for the bathroom based on movement instructions.
    /// http://adventofcode.com/2016/day/2
    /// </summary>
    public class Day2 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var breaker = new KeypadCodeBreaker(new BasicNineDigitCracker());
            breaker.MustPeeNeedCodeNow(File.ReadAllLines("Day2/input.txt"));
            return breaker.DoorCode;
        }

        object IPuzzlePart2.Answer()
        {
            var breaker = new KeypadCodeBreaker(new DumbDiamondCracker());
            breaker.MustPeeNeedCodeNow(File.ReadAllLines("Day2/input.txt"));
            return breaker.DoorCode;
        }

        public class KeypadCodeBreaker
        {
            public ICracker Cracker { get; }

            public KeypadCodeBreaker(ICracker cracker)
            {
                if (cracker == null) throw new ArgumentNullException(nameof(cracker));
                this.Cracker = cracker;
            }

            public object[] Codes { get; private set; }

            public string DoorCode
            {
                get
                {
                    return this.Codes.Aggregate(string.Empty, (current, c) => string.Concat(current, c));
                }
            }

            public void MustPeeNeedCodeNow(string[] instructionLines)
            {
                this.Codes = new object[instructionLines.Length];

                for (var i = 0; i < instructionLines.Length; i++)
                {
                    this.Codes[i] = this.Cracker.Crack(instructionLines[i].ToCharArray());
                }
            }
        }

        public class BasicNineDigitCracker : ICracker
        {
            private static readonly int[] UpBoundaries = { 1, 2, 3 };
            private static readonly int[] LeftBoundaries = { 1, 4, 7 };
            private static readonly int[] RightBoundaries = { 3, 6, 9 };
            private static readonly int[] DownBoundaries = { 7, 8, 9 };

            public object Key { get; private set; } = 5;

            public char RequestedMove { get; private set; }

            public object Crack(char[] fingerPath)
            {
                foreach (char move in fingerPath)
                {
                    this.RequestedMove = move;

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

                return this.Key;
            }

            protected virtual void MoveRight()
            {
                if (this.CanExecuteMove(RightBoundaries))
                {
                    this.ExecuteMove(1);
                }
            }

            protected virtual void MoveLeft()
            {
                if (this.CanExecuteMove(LeftBoundaries))
                {
                    this.ExecuteMove(-1);
                }
            }

            protected virtual void MoveUp()
            {
                if (this.CanExecuteMove(UpBoundaries))
                {
                    this.ExecuteMove(-3);
                }
            }

            protected virtual void MoveDown()
            {
                if (this.CanExecuteMove(DownBoundaries))
                {
                    this.ExecuteMove(3);
                }
            }

            protected virtual void ExecuteMove(int i)
            {
                this.Key = (int)this.Key + i;
            }

            private bool CanExecuteMove(int[] boundaries)
            {
                bool notAtBoundary = !boundaries.Contains((int)this.Key);
                return notAtBoundary;
            }
        }

        public class DumbDiamondCracker : BasicNineDigitCracker
        {
            private const int Invalid = -1;

            public DumbDiamondCracker()
            {
                this.InitializeKeypad();
            }

            public int X { get; private set; } = 0;

            public int Y { get; private set; } = 2;

            public object[,] Keypad { get; } = new object[5, 5];

            public new object Key => this.Keypad[this.X, this.Y];

            protected override void MoveDown()
            {
                if (this.CanExecute(0, 1)) this.Y += 1;
            }

            protected override void MoveLeft()
            {
                if (this.CanExecute(-1, 0)) this.X += -1;
            }

            protected override void MoveRight()
            {
                if (this.CanExecute(1, 0)) this.X += 1;
            }

            protected override void MoveUp()
            {
                if (this.CanExecute(0, -1)) this.Y -= 1;
            }

            private bool CanExecute(int xMove, int yMove)
            {
                try
                {
        
                    object canidate = this.Keypad[this.X + xMove, this.Y + yMove];

                    if (canidate is string) return true;
                    if (canidate is int) return (int)canidate != Invalid;
                    else return false;
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
            }

            /// <summary>
            /// Initializes the keypad, using an X/Y array grid system, starting in the top left.
            /// </summary>
            private void InitializeKeypad()
            {
                // row 1
                this.Keypad[0, 0] = Invalid;
                this.Keypad[1, 0] = Invalid;
                this.Keypad[2, 0] = 1;
                this.Keypad[3, 0] = Invalid;
                this.Keypad[4, 0] = Invalid;

                // row 2
                this.Keypad[0, 1] = Invalid;
                this.Keypad[1, 1] = 2;
                this.Keypad[2, 1] = 3;
                this.Keypad[3, 1] = 4;
                this.Keypad[4, 1] = Invalid;

                // row 3
                this.Keypad[0, 2] = 5;
                this.Keypad[1, 2] = 6;
                this.Keypad[2, 2] = 7;
                this.Keypad[3, 2] = 8;
                this.Keypad[4, 2] = 9;

                // row 4
                this.Keypad[0, 3] = Invalid;
                this.Keypad[1, 3] = "A";
                this.Keypad[2, 3] = "B";
                this.Keypad[3, 3] = "C";
                this.Keypad[4, 3] = Invalid;

                // row 5
                this.Keypad[0, 4] = Invalid;
                this.Keypad[1, 4] = Invalid;
                this.Keypad[2, 4] = "D";
                this.Keypad[3, 4] = Invalid;
                this.Keypad[4, 4] = Invalid;
            }
        }

        public interface ICracker
        {
            object Crack(char[] fingerPath);
        }
    }
}
