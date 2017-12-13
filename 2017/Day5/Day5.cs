namespace AoC
{
    using System;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day5 : IPuzzle, IPuzzlePart2
    {
        public int[] JumpOffsets = File.ReadAllLines("Day5/input.txt").Select(c => Int32.Parse(c.ToString())).ToArray();

        object IPuzzle.Answer()
        {
            var jumper = new InstructionJumper();
            jumper.FindExit(this.JumpOffsets);

            return jumper.StepsTaken;
        }

        object IPuzzlePart2.Answer()
        {
            var jumper = new WeirdInstructionJumper();
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

                    // track.
                    i = this.CalculateOffset(jumpOffsets, i);
                    this.StepsTaken++;                 
                }
            }

            /// <summary>
            /// Pop instruction. Increment old instruction by 1. Move by instruction.
            /// </summary>
            protected virtual int CalculateOffset(int[] jumpOffsets, int i)
            {
                var instruction = jumpOffsets[i];
                jumpOffsets[i]++;
                i = i + instruction;
                return i;
            }
        }

        public class WeirdInstructionJumper : InstructionJumper
        {
            /// <summary>
            /// Pop instruction. If the offset was three or more, instead decrease it by 1. Otherwise, increase it by 1. Move by instruction
            /// </summary>
            protected override int CalculateOffset(int[] jumpOffsets, int i)
            {
                var instruction = jumpOffsets[i];
                if (instruction >= 3)
                {
                    jumpOffsets[i]--;

                }
                else
                {
                    jumpOffsets[i]++;
                }

                i = i + instruction;
                return i;
            }
        }
    }
}
