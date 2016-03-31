using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_WF
{
    public sealed class FreqTable
    {
        public Dictionary<char, int> Freqs { get; set; } = new Dictionary<char, int>();

        public void Build(string line)
        {
            foreach (char c in line)
            {
                if (!Freqs.ContainsKey(c))
                {
                    Freqs.Add(c, 1);
                }
                else
                {
                    Freqs[c] += 1;
                }
            }
        }
    }
}
