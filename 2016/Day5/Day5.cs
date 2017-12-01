namespace AoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using AoC.Common;

    /// <summary>
    /// Finds the password for a door by MD5 hashing a door id and looking for patterns.
    /// http://adventofcode.com/2016/day/5
    /// </summary>
    public class Day5 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var doorUnlocker = new DoorHashUnlocker("ojvtpuvg");
            return doorUnlocker.Unlock();
        }

        object IPuzzlePart2.Answer()
        {
            var doorUnlocker = new DoorFancyHashUnlocker("ojvtpuvg");
            return doorUnlocker.Unlock();
        }

        public class DoorFancyHashUnlocker : DoorHashUnlocker
        {
            public DoorFancyHashUnlocker(string doorId)
                : base(doorId)
            {
            }

            public override string Unlock()
            {
                var pwd = new Dictionary<int, string>();

                for (int i = 0; true; i++)
                {
                    // hash and convert to hex
                    var encodedDoorId = new UTF8Encoding().GetBytes(string.Format("{0}{1}", this.DoorId, i));
                    var hash = this.Md5.ComputeHash(encodedDoorId);
                    var hashAsHexString = this.ConvertHashToHexString(hash);

                    // look for pattern. if found, nab 7th ordinal for password 
                    // then use 6th ordinal for password slot
                    if (hashAsHexString.Substring(0, 5) == "00000")
                    {
                        int slot;
                        string slotAsString = hashAsHexString.Substring(5, 1);
                        string pwdChar = hashAsHexString.Substring(6, 1);

                        bool isSlotRealNumber = int.TryParse(slotAsString, out slot);
                        bool withinRange = slot < 8;
                        bool notAlreadyAdded = !pwd.ContainsKey(slot);
                        if (isSlotRealNumber && withinRange && notAlreadyAdded)
                        {
                            pwd.Add(slot, pwdChar);
                        } 
                    }

                    // ...but only abort when we've found 8 digits
                    if (pwd.Count == 8)
                    {
                        return pwd.OrderBy(x => x.Key).Aggregate(string.Empty, (current, p) => current + p.Value);
                    }
                }
            }
        }

        public class DoorHashUnlocker
        {
            public DoorHashUnlocker(string doorId)
            {
                this.DoorId = doorId;
            }

            public MD5 Md5 { get; } = MD5.Create();

            public string DoorId { get; }

            public virtual string Unlock()
            {
                string pwd = string.Empty;

                for (int i = 0; true; i++)
                {
                    // hash and convert to hex
                    var encodedDoorId = new UTF8Encoding().GetBytes(string.Format("{0}{1}", this.DoorId, i));
                    var hash = this.Md5.ComputeHash(encodedDoorId);
                    var hashAsHexString = this.ConvertHashToHexString(hash);

                    // look for pattern. if found, nab 6th digit
                    if (hashAsHexString.Substring(0, 5) == "00000")
                    {
                        pwd += hashAsHexString.Substring(5, 1);
                    }

                    // ...but only abort when we've found 8 digits
                    if (pwd.Length == 8)
                    {
                        return pwd;
                    }
                }
            }

            public string UnlockTest()
            {
                var md5 = MD5.Create();
                var encodedDoorId = new UTF8Encoding().GetBytes("abc" + "3231929");
                var hash = md5.ComputeHash(encodedDoorId);

                return this.ConvertHashToHexString(hash);
            }
            public string ConvertHashToHexString(byte[] hash)
            {
                return hash.Aggregate(string.Empty, (hexString, b) => hexString + string.Format("{0:X2}", b));
            }
        }
    }
}
