namespace AoC.Common
{
    using System;
    using System.Security.Cryptography;

    public static class StringExtensions
    {
        public static int ToInt(this string s) => Convert.ToInt32(s);

        public static (string Left, string Right) SplitInHalf(this string s)
        {
            return (s.Substring(0, (s.Length / 2)), s.Substring(s.Length / 2));
        }

        public static int ToInt(this char c) => Convert.ToInt32(c);
    }
}
