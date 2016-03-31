using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_WF
{
    public sealed class Node
    {
        public Node Left { get; set; } = null;
        public Node Right { get; set; } = null;
        public char? Char { get; set; } = null;
        public int Freq { get; set; } = default(int); //ale fajne znalazłem <3

        public List<bool> Traverse(char? ch, List<bool> data)
        {
            if (Left == null && Right == null)
            {
                return (bool)ch?.Equals(Char) ? data : null;
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>(data);
                    leftPath.Add(false);
                    left = Left.Traverse(ch, leftPath);
                }

                if (null != Right)
                {
                    List<bool> rightPath = new List<bool>(data);
                    rightPath.Add(true); //Add a '1'
                    right = Right.Traverse(ch, rightPath);
                }

                return (null != left) ? left : right;
            }
        }

        public bool IsLeaf()
        {
            return (null == this.Left && null == this.Right);
        }
    }
}
