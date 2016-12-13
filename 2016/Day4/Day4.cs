namespace AoC
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

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
            public IList<Room> RealRooms { get; set; } = new List<Room>();

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

            public string Checksum { get; private set; }

            public string Cipher { get; private set; }

            public int Sector { get; private set; }

            public static Room Parse(string encryptedRoomInfo)
            {
                var regex = new Regex(@"(?<cipher>[a-z\-]*?)-(?<sector>\d*?)\[(?<checksum>.*?)\]");
                var match = regex.Match(encryptedRoomInfo);

                return new Room()
                       {
                           Cipher = match.Groups["cipher"].Value.Replace("-", ""),
                           Sector = Convert.ToInt32(match.Groups["sector"].Value),
                           Checksum = match.Groups["checksum"].Value
                       };
            }

            public bool IsValid()
            {
                var cipherOrderdCount =
                    this.Cipher.ToCharArray()
                        .GroupBy(x => x)
                        .Select(grp => new { Character = grp.Key, Count = grp.Count() })
                        .OrderByDescending(x => x.Count)
                        .ThenBy(x => x.Character)
                        .Take(5);

                bool checksumMatches = cipherOrderdCount.Select(x => x.Character).Intersect(this.Checksum.ToCharArray()).Count() == 5;
                return checksumMatches;
            }
        }
    }
}
