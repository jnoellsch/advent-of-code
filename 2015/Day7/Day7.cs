namespace AoC.Day7
{
    using System.IO;

    public class Day7 : IPuzzle
    {
        object IPuzzle.Answer()
        {
            string[] instructions = File.ReadAllLines("Day7/input.txt");
            return string.Empty;
        }

        public class BitShifter
        {
        }

        public class BitShifterCommand
        {
            internal BitShifterCommand()
            {
            }

            public Variable StoreVariable { get; set; }

            public Variable LhVariable { get; set; }

            public Variable RhVariable { get; set; }

            public BitShifterCommand Parse(string command)
            {
                return null;
            }

            public void Run(BitShifter shifter)
            {
            }
        }

        public class Variable
        {
            public string Key { get; set; }

            public int Value { get; set; } 
        }
    }
}
