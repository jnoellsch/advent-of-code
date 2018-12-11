namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day9 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var game = new MarbleGame();
            game.WithPlayerCount(427);
            game.PlayTo(70723);

            return game.HighScore;
        }

        object IPuzzlePart2.Answer()
        {
            var game = new FasterMarbleGame();
            game.WithPlayerCount(427);
            game.PlayTo(70723 * 100);

            return game.HighScore;
        }
    }

    internal class FasterMarbleGame : MarbleGame
    {
        public FasterMarbleGame()
        {
            this.Marbles = new LinkedList<Marble>();
            this.CurrentMarble = new LinkedListNode<Marble>(new Marble(0));
            this.Marbles.AddFirst(this.CurrentMarble);
            this.DebugTurn();
        }

        private LinkedListNode<Marble> CurrentMarble { get; set; }

        private LinkedList<Marble> Marbles { get; }

        public override void PlayTo(int marbleMax)
        {
            for (int i = 1; i <= marbleMax; i++)
            {
                var marble = new LinkedListNode<Marble>(new Marble(i));
                this.CurrentPlayer = this.Players[(i - 1).Wrap(this.Players.Count)];

                if (marble.Value.IsMultipleOf(23))
                {
                    var marbleToRemove = this.CurrentMarble.Previous().Previous().Previous().Previous().Previous().Previous().Previous();
                    this.CurrentPlayer.Take(marble.Value);
                    this.CurrentPlayer.Take(marbleToRemove.Value);

                    this.CurrentMarble = marbleToRemove.Next();
                    this.Marbles.Remove(marbleToRemove);
                }
                else
                {
                    this.Marbles.AddAfter(this.CurrentMarble.Next(), marble);
                    this.CurrentMarble = marble;
                }

                this.DebugTurn();
            }
        }

        protected override void DebugTurn()
        {
            if (!this.Debug) return;

            Console.Write(this.CurrentPlayer is NullPlayer ? "[-] " : $"[{this.CurrentPlayer.Id}] ");
            
            foreach (var m in this.Marbles)
            {
                Console.Write(m.Value == this.CurrentMarble.Value.Value ? $"({m.Value})" : $" {m.Value} ");
            }
            
            Console.Write(Environment.NewLine);
        }
    }

    internal class MarbleGame
    {
        public MarbleGame()
        {
            this.Marbles = new List<Marble>() { new Marble(0) };
            this.Players = new List<Player>();
            this.CurrentMarble = this.Marbles.First();
            this.CurrentPlayer = new NullPlayer();
            this.DebugTurn();
        }

        public IList<Player> Players { get; }

        public Player CurrentPlayer { get; protected set; }

        public long HighScore => this.Players.Max(_ => _.Score);

        public bool Debug { get; set; }

        private IList<Marble> Marbles { get; }

        private Marble CurrentMarble { get; set; }

        public void WithPlayerCount(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                this.Players.Add(new Player(i));
            }
        }

        public virtual void PlayTo(int marbleMax)
        {
            for (int i = 1; i <= marbleMax; i++)
            {
                var marble = new Marble(i);
                this.CurrentPlayer = this.Players[(i - 1).Wrap(this.Players.Count)];

                if (marble.IsMultipleOf(23))
                {
                    int indexToRemove = (this.Marbles.IndexOf(this.CurrentMarble) - 7).Wrap(this.Marbles.Count);
                    var marbleToRemove = this.Marbles[indexToRemove];
                    this.Marbles.Remove(marbleToRemove);

                    this.CurrentPlayer.Take(marble);
                    this.CurrentPlayer.Take(marbleToRemove);
         
                    this.CurrentMarble = this.Marbles[indexToRemove];
                }
                else
                {
                    var index = (this.Marbles.IndexOf(this.CurrentMarble) + 1) % (this.Marbles.Count) + 1;
                    this.Marbles.Insert(index, marble);
                    
                    this.CurrentMarble = marble;
                }

                this.DebugTurn();
            }
        }

        protected virtual void DebugTurn()
        {
            if (!this.Debug) return;

            Console.Write(this.CurrentPlayer is NullPlayer ? "[-] " : $"[{this.CurrentPlayer.Id}] ");
            
            foreach (var m in this.Marbles)
            {
                Console.Write(m.Value == this.CurrentMarble.Value ? $"({m.Value})" : $" {m.Value} ");
            }
            
            Console.Write(Environment.NewLine);
        }
    }

    internal class Player
    {
        public int Id { get; }

        public long Score { get; set; } = 0;

        public Player(int id)
        {
            this.Id = id;
        }

        public virtual void Take(Marble marble)
        {
            this.Score += marble.Value;
        }
    }

    internal class NullPlayer : Player
    {
        public NullPlayer() : base(-1)
        {
        }

        public override void Take(Marble marble)
        {
            throw new NotImplementedException();
        }
    }

    internal class Marble
    {
        public long Value { get; set; }

        public Marble(long value)
        {
            this.Value = value;
        }

        public bool IsMultipleOf(int factor)
        {
            return this.Value % factor == 0 && this.Value != 0;
        }
    }
}
