namespace AoC
{
    using System.IO;
    using System.Linq;
    using AoC.Common;

    /// <summary>
    /// Calculates the building floor traversal of Santa by going up ( '(' ) or down ( ')' ).
    /// http://adventofcode.com/day/1
    /// </summary>
    public class Day1 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string floorMovement = File.ReadAllText("Day1/input.txt");
            
            var tracker = new ApartmentTracker();
            tracker.RunRoute(floorMovement.ToCharArray());

            return tracker.Floor;
        }

        object IPuzzlePart2.Answer()
        {
            string floorMovement = File.ReadAllText("Day1/input.txt");
            var tracker = new ApartmentTracker();

            foreach (char upOrDown in floorMovement)
            {
                tracker.Move(upOrDown);
                if (tracker.IsBasement()) break;
            }

            return tracker.ElevatorVisits;
        }

        public class ApartmentTracker
        {
            public ApartmentTracker()
            {
                this.Floor = 0;
            }

            public int Floor { get; private set; }

            public int ElevatorVisits { get; private set; }

            public void RunRoute(char[] allDirections)
            {
                allDirections.ToList().ForEach(this.Move);
            }

            public void Move(char direction)
            {
                this.ElevatorVisits++;

                switch (direction.ToString())
                {
                    case "(":
                        this.Floor++;
                        break;
                    case ")":
                        this.Floor--;
                        break;
                }
            }

            public bool IsBasement()
            {
                return this.Floor == -1;
            }
        }
    }
}
