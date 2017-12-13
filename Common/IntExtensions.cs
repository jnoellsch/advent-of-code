namespace AoC.Common
{
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
    }
}
