namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day1 : IPuzzle, IPuzzlePart2
    {
        public int[] Input { get; set; } = File.ReadAllText("Day1/input.txt").Select(c => Int32.Parse(c.ToString())).ToArray();

        object IPuzzle.Answer()
        {
            var captcha = new CaptchaDoorCracker();
            captcha.Crack(this.Input);

            return captcha.DoorCode;
        }

        object IPuzzlePart2.Answer()
        {
            var captcha = new HalfsiesCaptchaDoorCracker();
            captcha.Crack(this.Input);

            return captcha.DoorCode;
        }

        /// <summary>
        /// Finds the sum of all digits that match the next digit in the input.
        /// </summary>
        public class CaptchaDoorCracker
        {
            public int DoorCode { get; private set; } = -1;

            public List<int> NumberSets { get; } = new List<int>();

            public virtual void Crack(int[] digits)
            {
                this.FindNumberPairs(digits);
                this.SumNumberSets();
            }

            protected virtual void SumNumberSets()
            {
                this.DoorCode = this.NumberSets.Aggregate(0, (current, set) => current + set);
            }

            protected virtual void FindNumberPairs(int[] digits)
            {
                for (int i = 0; i < digits.Length; i++)
                {
                    int candidate = digits[i];
                    int peekIndex = this.CalculatePeekIndex(digits.Length, i);
                    int peek = digits[peekIndex];

                    if (candidate == peek)
                    {
                        this.NumberSets.Add(candidate);
                    }
                }
            }

            protected virtual int CalculatePeekIndex(int arraySize, int currentIndex)
            {
                return currentIndex + 1 < arraySize ? currentIndex + 1 : 0;
            }
        }

        /// <summary>
        /// find the sum of all digits that match the digit halfway around the circular list.
        /// </summary>
        public class HalfsiesCaptchaDoorCracker : CaptchaDoorCracker
        {
            protected override int CalculatePeekIndex(int arraySize, int currentIndex)
            {
                int offset = arraySize / 2 + currentIndex;
                if (offset >= arraySize)
                {
                    offset = Math.Abs(arraySize - offset);
                }

                return offset;
            }
        }
    }
}
