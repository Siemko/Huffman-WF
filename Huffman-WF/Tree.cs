using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_WF
{
    public class Tree
    {
        /// <summary>
        /// Klasa przechowująca drzewo Huffmana w postaci listy obiektów klasy Node, z zapisanym korzeniem - Root, tabelą częstości występowania oraz liczbą bitów zapisanych
        /// </summary>
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; } = null;
        public FreqTable _freqs { get; private set; } = new FreqTable();
        public int BitCountForTree { get; private set; } = default(int);

        /// <summary>
        /// Budowanie drzewa
        /// </summary>
        /// <param name="source">Ciąg znaków, z którego budowane jest drzewo</param>
        public void BuildTree(string source)
        {
            //usuwamy ewentualne stare drzewo
            nodes.Clear();

            //jeżeli brak ciągów wejściowego, rzuć wyjątkiem
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException("source");
            else
            {
                //budowanie tabeli częstotliwości
                _freqs.Build(source);

                //dla każdej kolumny w tabeli, czyli pary <znak, częstość występowania>, tworzy się nowy liść w drzewie
                //jest to pierwszy etap budowy drzewa
                foreach (KeyValuePair<char, int> symbol in _freqs.Freqs)
                {
                    nodes.Add(new Node() { Char = symbol.Key, Freq = symbol.Value });
                }
                //dopóki w drzewie pozostaje więcej niż jeden element
                while (nodes.Count > 1)
                {
                    //tworzymy listę pomocniczą przechowująca elementy uporządkowane rosnąco
                    List<Node> orderedNodes = nodes.OrderBy(node => node.Freq).ToList();

                    //jeżeli w liście pomocnicznej znajduje się więcej niż 2 elementy
                    if (orderedNodes.Count >= 2)
                    {
                        //bierzemy dwa najmniejsze elementy
                        List<Node> takenNodes = orderedNodes.Take(2).ToList();

                        //łączymy w nowy węzeł, tym razem nie będący liściem, a zawierający potomków po prawej i po lewej - węzeły z których został utworzony
                        Node parent = new Node()
                        {
                            Char = null,
                            Freq = takenNodes[0].Freq + takenNodes[1].Freq,
                            Left = takenNodes[0],
                            Right = takenNodes[1]
                        };
                        //usuwane są liście
                        nodes.Remove(takenNodes[0]);
                        nodes.Remove(takenNodes[1]);
                        //a dodawany węzeł
                        nodes.Add(parent);
                    }
                }
                //biorąc pod uwagę, że drzewo jest posortowane, możemy przyjąć za korzeń pierwszy element.
                Root = nodes.FirstOrDefault();
            }
        }
        /// <summary>
        /// Metoda kodująca podany ciąg znaków do tablicy bitowej zawierającej kody znaków z źródła.
        /// 
        /// </summary>
        /// <param name="source">źródłowy ciąg znaków</param>
        /// <returns>Binarny zapis źródła</returns>
        public BitArray Encode(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                //lista pomocnicza
                List<bool> encodedSource = new List<bool>();
                //Drzewo analizowane jest dla każdej litery źródła w poszukiwaniu kodu znaków do każdej z liter ze źródła, a do tablicy binarnej dodaje się tyle elementów, jak długi jest kod litery
                encodedSource.AddRange(source.SelectMany(character =>
                                            Root.Traverse(character, new List<bool>())
                                        ).ToList()
                                      );
                BitCountForTree = encodedSource.Count; //zapisujemy liczbę bitów, które zostały zapisane przy kodowaniu ciągu znaków, aby móc je łatwo zdekodować i usunąć nadmiarowe bity
                return new BitArray(encodedSource.ToArray());
            }
            else return null;
        }

        /// <summary>
        /// Metoda działająca odwrotnie do powyższej, dokodująca tablicę bitową do ciągu znaków
        /// </summary>
        /// <param name="bits">tablica bitowa z zakodowanymi literami</param>
        /// <returns>Ciąg znaków, który był zakodowany w postaci wejściowej tablicy bitów</returns>
        public string Decode(BitArray bits)
        {
            //zaczynamy od korzenia
            Node current = Root;
            string decodedString = string.Empty;
            //dla każdego bitu
            foreach (bool bit in bits)
            {
                //poszukujemy węzła, w którym zapisana jest litera ukryta pod ciągiem bitów
                current = (bit ? current.Right ?? current : current.Left ?? current);
                //jeżeli ten węzeł jest liściem, co oznacza, że zapisana jest w nim litera
                if (current.IsLeaf())
                {
                    //dodawana jest ta litera do ciągu znaków, który zwrócimy
                    decodedString += current.Char;
                    current = Root;
                }
            }

            return decodedString;
        }

        public void ToFile(string file)
        {
            using (FileStream fs = File.OpenWrite(file))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(_freqs.Freqs.Count);
                // Write pairs.
                foreach (var pair in _freqs.Freqs)
                {
                    writer.Write(pair.Key);
                    writer.Write(pair.Value);
                }
                writer.Write(BitCountForTree);

            }
        }
    }
}
