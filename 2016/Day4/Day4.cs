namespace AoC
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day4 : IPuzzle
    {
        object IPuzzle.Answer()
        {
            var decryptor = new RoomListDecryptor();
            decryptor.FindLegitRoomsForConductingBiznass(File.ReadAllLines("Day4/input.txt"));
            return decryptor.SumRoomSectors();
        }

        public class RoomListDecryptor
        {
            public IList<Room> RealRooms { get; set; }

            public void FindLegitRoomsForConductingBiznass(string[] roomLines)
            {
                foreach (string roomLine in roomLines)
                {
                    var r = Room.Parse(roomLine);
                    if (r.IsValid())
                    {
                        this.RealRooms.Add(r);
                    }
                }
            }

            public int SumRoomSectors()
            {
                return this.RealRooms.Sum(x => x.Sector);
            }
        }

        public class Room
        {
            private Room()
            {
            }

            public int Sector { get; set; }

            public string Checksum { get; set; }

            public string Encrypted { get; set; }

            public static Room Parse(string encryptedRoomInfo)
            {
                return new Room()
                       {
                           
                       };
            }

            public bool IsValid()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
