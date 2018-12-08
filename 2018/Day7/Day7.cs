namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AoC.Common;

    public class Day7 : IPuzzle, IPuzzlePart2
    {
        private IList<StepInstruction> Instructions = File.ReadAllLines("Day7/sampleinput.txt").Select(StepInstruction.Parse).ToList();

        object IPuzzle.Answer()
        {
            var orderer = new StepOrderer();
            orderer.Order(this.Instructions);

            return orderer.Result;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class StepOrderer
    {
        public string Result { get; set; }

        public void Order(IList<StepInstruction> instructions)
        {
            var steps = this.SeedSteps();
            var startInst = this.FindStart(instructions);

            // create tree/linked list/tracking
            foreach (var inst in instructions)
            {
                var stepToModify = steps.Single(_ => _.Key == inst.First);
                stepToModify.AddNextStep(steps.Single(_ => _.Key == inst.Second));
            }

            // print out
            this.FollowInstructions(steps.Single(_ => _.Key == startInst));
        }

        private void FollowInstructions(Step step)
        {
            if (step.HasNextSteps())
            {
                foreach (var next in step.NextSteps)
                {
                    this.FollowInstructions(next);
                }
            }

            Console.Write(step.Key);
        }

        private string FindStart(IList<StepInstruction> instructions)
        {
            var firsts = instructions.Select(_ => _.First);
            var seconds = instructions.Select(_ => _.Second);

            return firsts.Except(seconds).Single();
        }

        private IList<Step> SeedSteps()
        {
            return Enumerable.Range(0, 26).Select(i => new Step() { Key = Convert.ToChar(i + 65).ToString()}).ToList();
        }
    }

    internal class Step
    {
        private IList<Step> _nextSteps = new List<Step>();

        public string Key { get; set; }

        public IList<Step> NextSteps => this._nextSteps.OrderBy(_ => _.Key).ToList();

        public bool HasNextSteps() => this._nextSteps.Any();

        public void AddNextStep(Step step) => this._nextSteps.Add(step);
    }

    internal class StepInstruction 
    {
        public string First { get; private set; }

        public string Second { get; private set; }

        public static StepInstruction Parse(string input)
        {
            var regex = new Regex(@"Step (?<first>.) must be finished before step (?<second>.) can begin.");
            var match = regex.Match(input);

            return new StepInstruction()
                   {
                       First = match.Groups["first"].Value, 
                       Second = match.Groups["second"].Value
                   };
        }
    }
}
