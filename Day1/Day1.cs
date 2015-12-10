namespace AoC
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Calculates the building floor traversal of Santa by going up ( '(' ) or down ( ')' ).
    /// http://adventofcode.com/day/1
    /// </summary>
    public class Day1 : IPuzzle
    {
        public object Answer()
        {
            string input = File.ReadAllText("Day1/input.txt");
            
            int up = input.Count(x => x.Equals('('));
            int down = input.Count(x => x.Equals(')'));

            return up - down;
        }
    }
}
