namespace Aoc
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using AoC.Common;

    public class Day1 : IPuzzle
    {
        public string[] Frequencies { get; set; } = File.ReadAllLines("Day1/input.txt");

        public object Answer()
        {
            var calibrator = new DeviceCalibrator();
            calibrator.Fix(this.Frequencies);

            return calibrator.Frequency;
        }
    }

    public class DeviceCalibrator
    {
        public int Frequency { get; set; } = 0;

        public void Fix(string[] frequencies)
        {
            this.Frequency = frequencies.Select(f => Int32.Parse(f, NumberStyles.AllowLeadingSign)).Sum();
        }
    }
}
