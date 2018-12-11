namespace AoC.Common
{
    using System;

    public static class IntExtensions
    {
        public static int Increment(this int number)
        {
            return number + 1;
        }

        public static int Wrap(this int index, int n)
        {
            return ((index % n) + n) % n;
        }

        public static string ToLetter(this int number)
        {
            if (number > 26)
            {
                throw new ArgumentException("Number is too large to convert to an alphabetic letter", nameof(number));
            }

            return Convert.ToChar(number + 65).ToString();
        }

        public static bool IsOdd(this int number) => !IsEven(number);

        public static bool IsEven(this int number) => number % 2 == 0;
    }
}
