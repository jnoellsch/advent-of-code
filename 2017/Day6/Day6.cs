namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day6 : IPuzzle, IPuzzlePart2
    {
        public int[] StartingMemory = File.ReadAllText("Day6/input.txt").Split('\t').Select(x => Convert.ToInt32(x)).ToArray();

        object IPuzzle.Answer()
        {
            var manager = new MemoryManager(this.StartingMemory);
            manager.Reallocate();

            return manager.RedistributionCycles;
        }

        object IPuzzlePart2.Answer()
        {
            var manager = new MemoryManager(this.StartingMemory);
            manager.Reallocate();

            return manager.LoopedStateDrift;
        }

        public class MemoryManager
        {
            public MemoryManager(int[] memory)
            {
                this.Memory = memory;
            }

            public int RedistributionCycles => this.History.Count;

            public int LoopedStateDrift
            {
                get
                {
                    var firstSighting = this.History.First(h => h.SequenceEqual(this.Memory));
                    return this.RedistributionCycles - this.History.IndexOf(firstSighting);
                }
            }

            public List<int[]> History { get; } = new List<int[]>();

            public int[] Memory { get; }

            public void Reallocate()
            {
                while (true)
                {
                    this.RecordState();
                    this.RedistributeStartingAt(this.FindMaxRegisterIndex());

                    if (this.IsLooped())
                    {
                        break;
                    }
                }
            }

            protected virtual bool IsLooped()
            {
                return this.History.Any(h => h.SequenceEqual(this.Memory));
            }

            protected virtual void RecordState()
            {
                this.History.Add((int[])this.Memory.Clone());
            }

            protected virtual int FindMaxRegisterIndex()
            {
                return Array.IndexOf(this.Memory, this.Memory.Max());
            }

            private void RedistributeStartingAt(int start)
            {
                // get memory. then wipe it.
                int current = start;
                int amt = this.Memory[start];
                this.Memory[start] = 0;

                // spread it around
                for (int i = 0; i < amt; i++)
                {
                    current = current.Increment().Wrap(this.Memory.Length);
                    this.Memory[current]++;
                }
            }
        }
    }
}
