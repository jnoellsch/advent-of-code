namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day1 : IPuzzle
    {
        public object Answer()
        {
            var digits = File.ReadAllText("Day1/input.txt").Select(c => Int32.Parse(c.ToString())).ToArray();

            var captcha = new CaptchaDoorCracker();
            captcha.Crack(digits);

            return captcha.DoorCode;
        }

        public class CaptchaDoorCracker
        {
            public int DoorCode { get; private set; } = -1;

            public List<int> NumberSets { get; } = new List<int>();

            public virtual void Crack(int[] digits)
            {
                this.FindNumberPairs(digits);
                this.SumNumberGroups();
            }

            protected virtual void SumNumberGroups()
            {
                int sum = 0;

                foreach (var set in this.NumberSets)
                {
                    sum = sum + set;
                }

                this.DoorCode = sum;
            }

            protected virtual void FindNumberPairs(int[] digits)
            {
                for (int i = 0; i < digits.Length; i++)
                {
                    int candidate = digits[i];
                    int peekIndex = i + 1 < digits.Length ? i + 1 : 0;
                    int peek = digits[peekIndex];

                    if (candidate == peek)
                    {
                        this.NumberSets.Add(candidate);
                    }
                }
            }
        }
    }
}
