namespace AoC.Day8
{
    using System.IO;
    using AoC.Common;

    public class Day8 : IPuzzle
    {
        public string[] Instructions = File.ReadAllLines("Day8/input.txt");

        object IPuzzle.Answer()
        {
            var manager = new RegisterManager();
            manager.LoadAndRun(this.Instructions);

            return manager.LargestValue;
        }

        public class RegisterManager
        {
            public void LoadAndRun(string[] instructions)
            {
                // convert instructions
                // define unique registers
                // run instructions
                // find largest register value
            }

            public int LargestValue { get; set; }
        }
    }
}
