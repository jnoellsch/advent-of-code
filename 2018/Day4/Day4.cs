namespace Aoc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AoC.Common;

    public class Day4 : IPuzzle, IPuzzlePart2
    {
        private string[] ActivityLog { get; } = File.ReadAllLines("Day4/input.txt").OrderBy(_ => _).ToArray();

        object IPuzzle.Answer()
        {
            var tracker = new GuardTracker();
            tracker.DebugInput(this.ActivityLog);
            tracker.Track(this.ActivityLog);
            tracker.DebugTotals();
            tracker.DebugActivity(tracker.SleepestGuard);

             return tracker.SleepestGuard.Id * tracker.SleepestGuard.SleepiestMinute();
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }
    }

    internal class GuardTracker
    {
        public IList<Guard> Guards { get; set; } = new List<Guard>();

        public Guard SleepestGuard { get; set; } = new Guard();

        public void Track(string[] activityLog)
        {
            // load data
            var guardIndexes = this.DetectGuardActivityIndexes(activityLog);
            foreach (int gi in guardIndexes)
            {
                var guard = this.GetOrAddGuard(activityLog[gi]);

                // pump in activity (until next guard detected)
                int nextGi = guardIndexes.ElementAt(Math.Min(guardIndexes.IndexOf(gi) + 1, guardIndexes.Count - 1));
                
                // skip guard with no activity (hacky...)
                bool noGuardActivity = (nextGi - gi) == 1;
                if (noGuardActivity)
                {
                    continue;
                }

                // bump up final one so for will actually run? (hacky...)
                bool isLastRun = nextGi == gi;
                if (isLastRun)
                {
                    nextGi += 2;
                }

                for (int i = gi; i < nextGi - 1;)
                {
                    guard.Activity.Add(GuardActivity.Parse(activityLog[i+1], activityLog[i+2]));
                    i += 2;
                }
            }

            // find sleepiest
            this.SleepestGuard = this.Guards.OrderByDescending(_ => _.TotalNapTime()).First();
        }

        public void DebugInput(string[] activityLog)
        {
            foreach (var a in activityLog)
            {
                Console.WriteLine($"{a}");
            }

            Console.WriteLine();
        }

        private Guard GetOrAddGuard(string guardActivity)
        {
            var candidate = Guard.Parse(guardActivity);
            var existing = this.Guards.FirstOrDefault(_ => _.Id == candidate.Id);

            if (existing == null)
            {
                this.Guards.Add(candidate);
                return candidate;
            }

            return existing;
        }

        private IList<int> DetectGuardActivityIndexes(string[] activityLog)
        {
            return activityLog
                .Select((a, i) => new
                                  {
                                      Activity = a, 
                                      Index = i
                                  })
                .Where(_ => _.Activity.Contains("Guard"))
                .Select(_ => _.Index)
                .ToList();
        }

        public void DebugActivity(Guard guard)
        {
            Console.WriteLine();
            Console.WriteLine($"Guard #{guard.Id}");
            Console.WriteLine("000000000011111111112222222222333333333344444444445555555555");
            Console.WriteLine("012345678901234567890123456789012345678901234567890123456789");

            foreach (var activity in guard.Activity)
            {
                for (int i = 0; i < 60; i++)
                {
                    if (activity.SleepySleepy <= i && i < activity.WakeyWakey)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            for (int i = 0; i <= 59; i++)
            {
                int total = 0;

                foreach (var activity in guard.Activity)
                {
                    if (activity.SleepySleepy <= i && i < activity.WakeyWakey)
                    {
                        total += 1;
                    }
                }

                Console.WriteLine($"Minute {i:00} = {total}");
            }
        }

        public void DebugTotals()
        {
            foreach (var guard in this.Guards.OrderByDescending(_ => _.TotalNapTime()))
            {
                Console.WriteLine($"Guard #{guard.Id} = {guard.TotalNapTime()}");
            }
        }
    }

    internal class Guard
    {
        public int Id { get; set; }

        public IList<GuardActivity> Activity { get; set; } = new List<GuardActivity>();

        public int TotalNapTime() => this.Activity.Sum(_ => _.NapTimeMinutes);

        public int SleepiestMinute()
        {
            return Enumerable.Range(0, 59)
                .Select(i => new
                             {
                                 Count = this.Activity.Count(_ => _.SleepySleepy <= i && _.WakeyWakey >= i), 
                                 Minute = i
                             })
                .OrderByDescending(_ => _.Count)
                .First()
                .Minute;
        }

        public static Guard Parse(string guard)
        {
            var regex = new Regex(@"Guard #(?<id>\d+)");
            Match match = regex.Match(guard);

            return new Guard() { Id = Convert.ToInt32(match.Groups["id"].Value) };
        }
    }

    internal class GuardActivity
    {
        public int SleepySleepy { get; set; }

        public int WakeyWakey { get; set; }

        public int NapTimeMinutes => this.WakeyWakey - this.SleepySleepy - 1;

        public static GuardActivity Parse(string sleepIntel, string awakeIntel)
        {
            var regex = new Regex(@"\[(?<date>\d{4}-\d{2}-\d{2}) \d{2}:(?<min>\d{2})\]");
            Match sleepMatch = regex.Match(sleepIntel);
            Match awakeMatch = regex.Match(awakeIntel);

            return new GuardActivity()
                   {
                       SleepySleepy = Convert.ToInt32(sleepMatch.Groups["min"].Value),
                       WakeyWakey = Convert.ToInt32(awakeMatch.Groups["min"].Value)
                   };
        }
    }
}
