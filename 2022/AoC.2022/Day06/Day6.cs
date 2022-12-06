namespace AoC
{
    using AoC.Common;
    using System;
    using System.Diagnostics;
    using static AoC.Day5;

    internal class Day6 : IPuzzle, IPuzzlePart2
    {
        private string Datastream = File.ReadAllText("Day06/input.txt");

        object IPuzzle.Answer()
        {
            var system = new CommunicationsSystem();
            Debug.WriteLine("{0} = 7", system.FindMarker("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4));
            Debug.WriteLine("{0} = 5", system.FindMarker("bvwbjplbgvbhsrlpgdmjqwftvncz", 4));
            Debug.WriteLine("{0} = 6", system.FindMarker("nppdvjthqldpwncqszvftbrmjlhg", 4));
            Debug.WriteLine("{0} = 10", system.FindMarker("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4));
            Debug.WriteLine("{0} = 11", system.FindMarker("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4));
            return system.FindMarker(this.Datastream, 4);
        }

        object IPuzzlePart2.Answer()
        {
            var system = new CommunicationsSystem();
            return system.FindMarker(this.Datastream, 14);
        }
    }

    internal class CommunicationsSystem
    {
        internal int FindMarker(string datastream, int length)
        {
            for (int i = 0; i < datastream.Length - length; i++)
            {
                if(datastream.Skip(i).Take(length).Distinct().Count() == length)
                {
                    return i + length;
                }
            }

            return -1;
        }
    }
}
