namespace AoC
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AoC.Common;

    /// <summary>
    /// Decrypts a set of rooms and finds rooms by names.
    /// http://adventofcode.com/2016/day/4
    /// </summary>
    public class Day4 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var decryptor = new RoomListDecryptor();
            decryptor.FindLegitRoomsForConductingBiznass(File.ReadAllLines("Day4/input.txt"));
            return decryptor.SumRoomSectors();
        }

        object IPuzzlePart2.Answer()
        {
            var decryptor = new RoomListDecryptor();
            decryptor.FindLegitRoomsForConductingBiznass(File.ReadAllLines("Day4/input.txt"));

            var finder = new RoomFinder(decryptor);
            var room = finder.NameStartsWith("north");

            return room?.Sector ?? -1;
        }

        public class RoomListDecryptor
        {
            public IList<Room> RealRooms => this.Rooms.Where(this.IsValidRoom).ToList();

            public IList<Room> Rooms { get; set; } = new List<Room>();

            public void FindLegitRoomsForConductingBiznass(string[] roomLines)
            {
                foreach (string roomLine in roomLines)
                {
                    this.Rooms.Add(Room.Parse(roomLine));
                }
            }

            public int SumRoomSectors()
            {
                return this.RealRooms.Sum(x => x.Sector);
            }

            public bool IsValidRoom(Room r)
            {
                var cipherOrderdCount =
                    r.CipherDashesRemoved.ToCharArray()
                        .GroupBy(x => x)
                        .Select(grp => new { Character = grp.Key, Count = grp.Count() })
                        .OrderByDescending(x => x.Count)
                        .ThenBy(x => x.Character)
                        .Take(5);

                bool checksumMatches = cipherOrderdCount.Select(x => x.Character).Intersect(r.Checksum.ToCharArray()).Count() == 5;
                return checksumMatches;
            }
        }

        public class RoomFinder
        {
            public RoomListDecryptor Decryptor { get; }

            public RoomFinder(RoomListDecryptor decryptor)
            {
                if (decryptor == null) throw new ArgumentNullException(nameof(decryptor));
                this.Decryptor = decryptor;
            }

            public Room NameStartsWith(string prefix)
            {
                return this.Decryptor.RealRooms.FirstOrDefault(x => x.Name.StartsWith(prefix));
            }
        }

        public class Room
        {
            private Room()
            {
            }

            public string Checksum { get; private set; }

            public string Cipher { get; private set; }

            public string CipherDashesRemoved => this.Cipher.Replace("-", "");

            public int Sector { get; private set; }

            public string Name => this.DecryptName();

            public static Room Parse(string encryptedRoomInfo)
            {
                var regex = new Regex(@"(?<cipher>[a-z\-]*?)-(?<sector>\d*?)\[(?<checksum>.*?)\]");
                var match = regex.Match(encryptedRoomInfo);

                return new Room()
                       {
                           Cipher = match.Groups["cipher"].Value,
                           Sector = Convert.ToInt32(match.Groups["sector"].Value),
                           Checksum = match.Groups["checksum"].Value
                       };
            }

            public string DecryptName()
            {
                int shift = this.Sector % 26;
                string decryptedName = string.Empty;

                foreach (char c in this.Cipher)
                {
                    if (c == '-')
                    {
                        decryptedName += ' ';
                        continue;
                    }

                     decryptedName += (char)((((c + shift) % 97) % 26) + 97);
                }

                return decryptedName;
            }
        }
    }
}
