namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day1 : IPuzzle, IPuzzlePart2
    {
        public int[] Changes { get; set; } = File.ReadAllLines("Day01/input.txt").Select(f => Int32.Parse(f, NumberStyles.AllowLeadingSign)).ToArray();

        object IPuzzle.Answer()
        {
            var calibrator = new DeviceCalibrator();
            calibrator.Fix(this.Changes);

            return calibrator.Frequency;
        }

        object IPuzzlePart2.Answer()
        {
            var calibrator = new DoubleFrequencyDeviceCalibrator();
            calibrator.Fix(this.Changes);

            return calibrator.Frequency;
        }
    }

    public class DeviceCalibrator
    {
        public int Frequency { get; set; } = 0;

        public virtual void Fix(int[] changes)
        {
            this.Frequency = changes.Sum();
        }
    }

    public class DoubleFrequencyDeviceCalibrator : DeviceCalibrator
    {
        private readonly List<int> _frequencyHistory = new List<int>();

        public override void Fix(int[] changes)
        {
            int rollingFrequency = 0;
            int i = 0;

            while (true)
            {
                rollingFrequency += changes[i];
                
                // search for dupes
                if(this.NoDupesDetected(rollingFrequency))
                {
                    this.StoreHistory(rollingFrequency);
                }
                else
                {
                    this.Frequency = rollingFrequency;
                    break;
                }

                // bump counter (for infinite looping)
                i = i + 1;
                if (i > changes.Length - 1)
                {
                    i = 0;
                }
            }
        }

        private void StoreHistory(int frequency) => this._frequencyHistory.Add(frequency);

        private bool NoDupesDetected(int currentFrequency) => !this._frequencyHistory.Contains(currentFrequency);
    }
}
