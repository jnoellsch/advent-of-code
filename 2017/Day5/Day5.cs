namespace AoC
{
    using System;
    using System.IO;
    using System.Linq;
    using AoC.Common;
    public class Day5 : IPuzzle
    {
        public int[] JumpOffsets = File.ReadAllLines("Day5/input.txt").Select(c => Int32.Parse(c.ToString())).ToArray(); 

        public object Answer()
        {
            var jumper = new InstructionJumper();
            jumper.FindExit(this.JumpOffsets);

            return jumper.StepsTaken;
        }

        public class InstructionJumper
        {
            public int StepsTaken { get; private set; } = 0;

            public void FindExit(int[] jumpOffsets)
            {
                int listSize = jumpOffsets.Length;
                int i = 0;

                while (true)
                {
                    bool outsideListBoundaries = i < 0 || listSize - 1 < i;
                    if (outsideListBoundaries)
                    {
                        break;
                    }

                    // pop instruction. increment old instruction. move by instruction. track.
                    var instruction = jumpOffsets[i];  
                    jumpOffsets[i]++;                  
                    i = i + instruction;               
                    this.StepsTaken++;                 
                }
            }
        }
    }
}
