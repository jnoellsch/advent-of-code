namespace AoC
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Finds the lowest coin number that has a hash that starts with five zeros.
    /// http://adventofcode.com/day/4
    /// </summary>
    public class Day4 : IPuzzle
    {
        public object Answer()
        {
            string secretKey = "ckczppom";
            var miner = new CoinMiner(secretKey);

            while (!miner.IsIdealCoin())
            {
                miner.Dig();
            }

            return miner.CoinNumber;
        }

        public class CoinMiner
        {
            private Md5HexHasher _hasher = new Md5HexHasher();

            public CoinMiner(string secretKey)
            {
                this.SecretKey = secretKey;
                this.CoinNumber = 1;
                this.Hash = string.Empty;
            }

            public int CoinNumber { get; set; }

            public string SecretKey { get; private set; }

            public string Hash { get; private set; }

            public void Dig()
            {
                this.NextCoin();
                this.Hash = this._hasher.Calculate(string.Format("{0}{1}", this.SecretKey, this.CoinNumber));
            }

            public bool IsIdealCoin()
            {
                return this.Hash.StartsWith("00000");
            }

            private void NextCoin()
            {
                this.CoinNumber++;
            }
        }

        /// <summary>
        /// Creates a hexidecimal representation of an MD5 hash.
        /// </summary>
        public class Md5HexHasher
        {
            public string Calculate(string input)
            {
                // calculate hash
                byte[] hash;
                using (var md5 = MD5.Create())
                {
                    hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                }

                // convert to hex string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
