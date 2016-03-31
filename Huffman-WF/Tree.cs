using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_WF
{
    public class Tree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; } = null;
        public FreqTable _freqs { get; private set; } = new FreqTable();
        public int BitCountForTree { get; private set; } = default(int);

        public void BuildTree(string source)
        {
            nodes.Clear(); //As we build a new tree, first make sure it's clean :)

            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException("source");
            else
            {
                _freqs.Build(source);

                foreach (KeyValuePair<char, int> symbol in _freqs.Freqs)
                {
                    nodes.Add(new Node() { Char = symbol.Key, Freq = symbol.Value });
                }

                while (nodes.Count > 1)
                {
                    List<Node> orderedNodes = nodes.OrderBy(node => node.Freq).ToList();

                    if (orderedNodes.Count >= 2)
                    {
                        List<Node> takenNodes = orderedNodes.Take(2).ToList();

                        Node parent = new Node()
                        {
                            Char = null,
                            Freq = takenNodes[0].Freq + takenNodes[1].Freq,
                            Left = takenNodes[0],
                            Right = takenNodes[1]
                        };
                        nodes.Remove(takenNodes[0]);
                        nodes.Remove(takenNodes[1]);
                        nodes.Add(parent);
                    }
                }

                Root = nodes.FirstOrDefault();
            }
        }

        public BitArray Encode(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                List<bool> encodedSource = new List<bool>();
                encodedSource.AddRange(source.SelectMany(character =>
                                            Root.Traverse(character, new List<bool>())
                                        ).ToList()
                                      );
                BitCountForTree = encodedSource.Count;
                return new BitArray(encodedSource.ToArray());
            }
            else return null;
        }

        public string Decode(BitArray bits)
        {
            Node current = Root;
            string decodedString = string.Empty;

            foreach (bool bit in bits)
            {
                current = (bit ? current.Right ?? current : current.Left ?? current);

                if (current.IsLeaf())
                {
                    decodedString += current.Char;
                    current = Root;
                }
            }

            return decodedString;
        }
    }
}
