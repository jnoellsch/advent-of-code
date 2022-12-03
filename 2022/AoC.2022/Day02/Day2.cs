namespace AoC
{
    using AoC.Common;

    public class Day2 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var rounds = File.ReadAllLines("Day02/input.txt").Select(GameRound.Parse);
            var rpc = new RockPaperScissorsScoring();
            return rpc.CalculateScore(rounds);
        }

        object IPuzzlePart2.Answer()
        {
            var rounds = File.ReadAllLines("Day02/input.txt").Select(ForcedGameRound.Parse);
            var rpc = new RockPaperScissorsScoring();
            return rpc.CalculateScore(rounds);
        }
    }

    public class RockPaperScissorsScoring
    {
        internal int CalculateScore(IEnumerable<GameRound> strategyGuideItems)
        {
            return strategyGuideItems.Select(sgi => sgi.GetScore()).Sum();
        }
    }

    /// <summary>
    /// Modified game round of rock paper scissors where the need or outcome is predetermined.
    /// </summary>
    internal class ForcedGameRound : GameRound
    {
        private const char NEED_LOSS = 'X';
        private const char NEED_DRAW = 'Y';
        private const char NEED_WIN = 'Z';

        public char Need { get; private set; }

        public static ForcedGameRound Parse(string input)
        {
            return new ForcedGameRound()
            {
                Op = input[0],
                Need = input[2]
            };
        }

        public override int GetScore()
        {
            // draw
            if (this.Need == NEED_DRAW)
            {
                if (this.Op == OP_ROCK) { this.Me = ME_ROCK; }
                if (this.Op == OP_PAPER) { this.Me = ME_PAPER; }
                if (this.Op == OP_SCISSORS) { this.Me = ME_SCISSORS; }
            }

            // loss
            if (this.Need == NEED_LOSS)
            {
                if (this.Op == OP_ROCK) { this.Me = ME_SCISSORS; }
                if (this.Op == OP_PAPER) { this.Me = ME_ROCK; }
                if (this.Op == OP_SCISSORS) { this.Me = ME_PAPER; }
            }

            // win
            if (this.Need == NEED_WIN)
            {
                if (this.Op == OP_ROCK) { this.Me = ME_PAPER; }
                if (this.Op == OP_PAPER) { this.Me = ME_SCISSORS; }
                if (this.Op == OP_SCISSORS) { this.Me = ME_ROCK; }
            }

            return base.GetScore();
        }
    }

    /// <summary>
    /// Standard game round of rock paper scissors
    /// </summary>
    internal class GameRound
    {
        protected const char OP_ROCK = 'A';
        protected const char OP_PAPER = 'B';
        protected const char OP_SCISSORS = 'C';

        protected const char ME_ROCK = 'X';
        protected const char ME_PAPER = 'Y';
        protected const char ME_SCISSORS = 'Z';

        protected const int WIN = 6;
        protected const int DRAW = 3;
        protected const int LOSS = 0;

        public char Op { get; protected set; }

        public char Me { get; protected set; }

        public static GameRound Parse(string input)
        {
            return new GameRound()
            {
                Op = input[0],
                Me = input[2]
            };
        }

        public virtual int GetScore()
        {
            // draw
            if ((this.Op == OP_ROCK && ME_ROCK == this.Me) ||
                (this.Op == OP_PAPER && ME_PAPER == this.Me) ||
                (this.Op == OP_SCISSORS && ME_SCISSORS == this.Me))
            {
                return DRAW + this.MyPoints();
            }

            // loss
            if ((this.Op == OP_ROCK && ME_SCISSORS == this.Me) ||
                (this.Op == OP_PAPER && ME_ROCK == this.Me) ||
                (this.Op == OP_SCISSORS && ME_PAPER == this.Me))
            {
                return LOSS + this.MyPoints();
            }

            // win
            if ((this.Me == ME_ROCK && OP_SCISSORS == this.Op) ||
                (this.Me == ME_PAPER && OP_ROCK == this.Op) ||
                (this.Me == ME_SCISSORS && OP_PAPER == this.Op))
            {
                return WIN + this.MyPoints();
            }

            throw new Exception("Nope");
        }

        private int MyPoints()
        {
            switch (this.Me)
            {
                case ME_ROCK: return 1;
                case ME_PAPER: return 2;
                case ME_SCISSORS: return 3;
            }

            throw new Exception("Nope");
        }
    }
}
