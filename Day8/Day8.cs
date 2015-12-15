namespace AoC
{
    using System.IO;
    using System.Text.RegularExpressions;

    public class Day8 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            string[] codeLines = File.ReadAllLines("Day8/input.txt");
            var codeCounter = new CodeCounter();

            foreach (var line in codeLines)
            {
                codeCounter.Count(line);
            }

            return codeCounter.CodeVsMemoryChars;
        }

        object IPuzzlePart2.Answer()
        {
            return string.Empty;
        }

        public class CodeCounter
        {
            public CodeCounter()
            {
                this.CodeChars = 0;
                this.MemoryChars = 0;
            }

            public int CodeChars { get; private set; }

            public int MemoryChars { get; private set; }

            public int CodeVsMemoryChars
            {
                get
                {
                    return this.CodeChars - this.MemoryChars;
                }
            }

            public void Count(string codeLine)
            {
                this.IncreaseCode(codeLine.Length);
                this.IncreaseMemory(this.EncodeAsMemory(codeLine).Length);
            }

            private void IncreaseMemory(int amountToAdd)
            {
                this.MemoryChars += amountToAdd;
            }

            private void IncreaseCode(int amountToAdd)
            {
                this.CodeChars += amountToAdd;
            }

            private string EncodeAsMemory(string codeLine)
            {
                // normalize/kill code ends
                codeLine = codeLine.TrimEnd('"').TrimStart('"'); 

                // replace escape sequences
                Regex regex = new Regex(@"\\x[a-f0-9][a-f0-9]|\\{2}|\\"""); 
                return regex.Replace(codeLine, "_");
            }
        }
    }
}
