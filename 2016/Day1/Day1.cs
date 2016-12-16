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
    public class Day1 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var elfNav = new ElfNavigator();
            elfNav.LoadInstructionsIntoGps(File.ReadAllText("Day1/input.txt"));
            return elfNav.BlocksAway;
        }

        object IPuzzlePart2.Answer()
        {
            var elfNav = new ElfIntersectionNavigator();
            elfNav.LoadInstructionsIntoGps(File.ReadAllText("Day1/input.txt"));
            return elfNav.BlocksAway;
        }

        public class ElfNavigator
        {
            protected const int N = 0;
            protected const int S = 180;
            protected const int E = 90;
            protected const int W = 270;

            private int _angle;

            public ElfNavigator()
            {
                this.Angle = 0;
                this.MoveData = new Dictionary<int, int>(4) { { N, 0 }, { S, 0 }, { E, 0 }, { W, 0 } };
            }
            private IDictionary<int, int> MoveData { get; }

            public virtual int BlocksAway
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

            public virtual void LoadInstructionsIntoGps(string directions)
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

            protected virtual void Move(int dist)
            {
                this.MoveData[this.Angle] += dist;
            }

            protected virtual void TurnRight()
            {
                this.Angle += 90;
            }

            protected virtual void TurnLeft()
            {
                this.Angle -= 90;
            }
        }

        public class ElfIntersectionNavigator : ElfNavigator
        {
            public override int BlocksAway
            {
                get
                {
                    int taxiCabMetric = Math.Abs(this.X - 250) + Math.Abs(this.Y - 250);
                    return taxiCabMetric;
                }
            }

            public bool IntersectionFound { get; private set; } = false;

            private int[,] MoveIntersections { get; } = new int[500, 500];

            private int X { get; set; } = 250;

            private int Y { get; set; } = 250;

            public override void LoadInstructionsIntoGps(string directions)
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
                    
                    // track 
                    for (int i = 0; i < dist; i++)
                    {
                        // move ordinal direction based on angle
                        switch (this.Angle)
                        {
                            case N:
                                this.MoveAndTrack(0, -1);
                                break;
                            case S:
                                this.MoveAndTrack(0, 1);
                                break;
                            case E:
                                this.MoveAndTrack(1, 0);
                                break;
                            case W:
                                this.MoveAndTrack(-1, 0);
                                break;
                        }

                        // abort once we've found HQ
                        if (this.IntersectionFound)
                        {
                            return;
                        }
                    }
                }
            }

            private void MoveAndTrack(int xMove, int yMove)
            {
                this.IntersectionFound = this.MoveIntersections[this.X += xMove, this.Y += yMove] == 1;
                this.MoveIntersections[this.X, this.Y] = 1;
            }
        }
    }
}
