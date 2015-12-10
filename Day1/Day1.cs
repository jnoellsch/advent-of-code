namespace AoC
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Calculates the building floor traversal of Santa by going up ( '(' ) or down ( ')' ).
    /// http://adventofcode.com/day/1
    /// </summary>
    public class Day1 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string floorMovement = File.ReadAllText("Day1/input.txt");

            int up = floorMovement.Count(x => x.Equals('('));
            int down = floorMovement.Count(x => x.Equals(')'));

            return up - down;
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
