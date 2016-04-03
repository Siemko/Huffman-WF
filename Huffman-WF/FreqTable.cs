using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_WF
{
    public sealed class FreqTable
    {
        /// <summary>
        /// Klasa przechowująca tabelę liter i częstości ich występowania w formie Słownika, w którym kluczem jest litera, a wartością częstość występowania
        /// </summary>
        public Dictionary<char, int> Freqs { get; set; } = new Dictionary<char, int>();
        /// <summary>
        /// Metoda budująca tabelę wystąpień:
        /// dla każdej litery z źródła sprawdzana jest jego obecność w słowniku,
        /// jeżeli nie istnieje taki klucz, dodawany jest z wartością 1
        /// w przeciwnym wypadku częstość wystąpień jest inkrementowana
        /// </summary>
        /// <param name="line">ciąg znaków, z którego budowany jest słownik</param>
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

        public void ToFile(string file)
        {
            var items = from pair in Freqs orderby pair.Value descending select pair;
            using (FileStream fs = File.OpenWrite(file))
            using (var writer = new StreamWriter(fs))
            {
                writer.WriteLine(Freqs.Count);
                // Write pairs.
                foreach (var pair in items)
                {
                    writer.WriteLine(pair.Key + ":[" + pair.Value + "]");
                }
            }
        }
    }
}
