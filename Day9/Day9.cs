namespace AoC
{
    using System;

    public class Day9 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var lookAndSay = new LookAndSay("1321131112");
            lookAndSay.Go(2);
            return lookAndSay.Result;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    public class LookAndSay
    {
        public LookAndSay(string start)
        {
            this.Start = start;
            this.Result = start;
        }

        public string Start { get; private set; }

        public string Result { get; private set; }

        public void Go(int times)
        {
            for (int i = 0; i < times; i++)
            {
                this.Result = this.DoIt(this.Result, string.Empty);
            }
        }

        public string DoIt(string input, string seekChars)
        {
            if (input.Length == 1)
            {
                return String.Concat(1, input);
            }

            string peek = input[0].ToString();
            if (seekChars != string.Empty && 
                seekChars.LastIndexOf(peek, StringComparison.Ordinal) == -1)
            {
                return string.Concat(seekChars.Length, seekChars[0], this.DoIt(input.Remove(0, 1), peek));
            }
            else
            {
                return this.DoIt(input.Remove(0, 1), seekChars + peek);
            }
        }
    }
}
