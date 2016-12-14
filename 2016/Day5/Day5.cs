namespace AoC
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class Day5 : IPuzzle
    {
        object IPuzzle.Answer()
        {
            var doorUnlocker = new DoorHashUnlocker();
            return doorUnlocker.Unlock("ojvtpuvg");
        }

        public class DoorHashUnlocker
        {
            public string Unlock(string doorId)
            {
                var md5 = MD5.Create();
                string pwd = string.Empty;

                for (int i = 0; true; i++)
                {
                    // hash and convert to hex
                    var encodedDoorId = new UTF8Encoding().GetBytes(string.Format("{0}{1}", doorId, i));
                    var hash = md5.ComputeHash(encodedDoorId);
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
            private string ConvertHashToHexString(byte[] hash)
            {
                return hash.Aggregate(string.Empty, (hexString, b) => hexString + string.Format("{0:X2}", b));
            }
        }
    }
}
