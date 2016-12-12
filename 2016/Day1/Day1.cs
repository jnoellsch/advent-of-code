namespace AoC
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Calculates the number of blocks away by using the taxi cab geometry. Instructions execute turns and travel distance. 
    /// http://adventofcode.com/2016/day/1
    /// </summary>
    public class Day1 : IPuzzle
    {
        public object Answer()
        {
            var elfNav = new ElfNavigator();
            elfNav.LoadInstructionsIntoGps(File.ReadAllText("Day1/input.txt"));
            return elfNav.BlocksAway;
        }

        public class ElfNavigator
        {
            private const int N = 0;
            private const int S = 180;
            private const int E = 90;
            private const int W = 270;

            private int _angle;

            public ElfNavigator()
            {
                this.Angle = 0;
                this.MoveData = new Dictionary<int, int>(4) { { N, 0 }, { S, 0 }, { E, 0 }, { W, 0 } };
            }
            private IDictionary<int, int> MoveData { get; }

            public int BlocksAway
            {
                get
                {
                    // taxi cab metric equation = |p1 - q1| + |p2 - q2|
                    int taxiCabMetric = Math.Abs(this.MoveData[N] - this.MoveData[S]) + Math.Abs(this.MoveData[E] - this.MoveData[W]);
                    return taxiCabMetric;
                }
            }

            public int Angle
            {
                get
                {
                    return this._angle;
                }

                set
                {
                    this._angle = value;

                    // ensure angle is locked to 4 options
                    if (this._angle == -90) this._angle = W;
                    if (this._angle == 360) this._angle = N;
                }
            }

            public void LoadInstructionsIntoGps(string directions)
            {
                foreach (string move in directions.Split(',').Select(x => x.Trim()))
                {
                    // extract move details
                    string turn = move.Substring(0, 1);
                    int dist = Convert.ToInt32(move.Substring(1, move.Length - 1));

                    // turn and hoof it
                    if (turn == "L") { this.TurnLeft(); }
                    else if (turn == "R") { this.TurnRight(); }
                    this.Move(dist);
                }
            }

            private void Move(int dist)
            {
                this.MoveData[this.Angle] += dist;
            }

            private void TurnRight()
            {
                this.Angle += 90;
            }

            private void TurnLeft()
            {
                this.Angle -= 90;
            }
        }
    }
}
