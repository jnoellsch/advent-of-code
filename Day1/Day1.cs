namespace AoC
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// http://adventofcode.com/day/1
    /// </summary>
    public class Day1 : IDay
    {
        public string Answer()
        {
            string input = File.ReadAllText("Day1/input.txt");
            
            int upFloors = input.Count(x => x.Equals('('));
            int downFloors = input.Count(x => x.Equals(')'));

            return (upFloors - downFloors).ToString();
        }
    }
}
