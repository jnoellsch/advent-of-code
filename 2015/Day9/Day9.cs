namespace AoC
{
    using System;
    using AoC.Common;

    public class Day9 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var lookAndSay = new LookAndSay("1321131112");
            lookAndSay.Go(5);
            return lookAndSay.Result;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    public class LookAndSay
    {
        public LookAndSay(string first)
        {
            this.First = first;
            this.Result = first;
        }

        public string First { get; private set; }

        public string Result { get; private set; }

        public void Go(int times)
        {
            for (int i = 0; i < times; i++)
            {
                string result = this.DoIt(this.Result, string.Empty);
                this.Result = result;
            }
        }

        public string DoIt(string input, string seekChars)
        {
            if (input.Length == 1)
            {
                return string.Concat(1, input);
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
