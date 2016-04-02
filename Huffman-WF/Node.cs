using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_WF
{
    public sealed class Node
    {
        /// <summary>
        /// Klasa przechowująca pojedyńczy element drzewa Huffmana.
        /// </summary>
        public Node Left { get; set; } = null; //potomek po lewej
        public Node Right { get; set; } = null; //potomek po prawej
        public char? Char { get; set; } = null; //litera: "?" oznacza typ nullable
        public int Freq { get; set; } = default(int); //częstość występowania

        /// <summary>
        /// Metoda służąca do analizy drzewa.
        /// Rekurencyjnie przeszukuje drzewo, zwracająć binarną ściężkę(listę) do poszukiwanej litery.
        /// 
        /// </summary>
        /// <param name="ch">Poszukiwana litera</param>
        /// <param name="data">Aktualna ścieżka(lista) zawierająca zero-jedynkowy kod litery</param>
        /// <returns>Zwraca kompletny kod 0-1 do litery przechowywanej w danym węźle</returns>
        public List<bool> Traverse(char? ch, List<bool> data)
        {
            //sprawdzanie, czy obecny element jest liściem
            if (Left == null && Right == null)
            {
                //jeżeli jest poszukiwaną literą zwraca obecną ściężkę, w przeciwnym wypadku zwraca 0
                return (bool)ch?.Equals(Char) ? data : null;
            }
            else
            {
                // listy pomocnicze przechowujące kody znaków dla węzłów z każdej ze stron
                List<bool> left = null;
                List<bool> right = null;
                
                //dla potomka po lewej
                if (Left != null)
                {
                    //tworzona jest lista
                    List<bool> leftPath = new List<bool>(data);
                    leftPath.Add(false); //dodawane jest 0
                    left = Left.Traverse(ch, leftPath); //rekuryncyjne przeszukiwanie kolejnych potomków z lewej strony
                }

                //analogicznie z potomkami z prawej strony, z tą różnicą, że posiadają wpisaną 1 zamiast 0.
                if (null != Right)
                {
                    List<bool> rightPath = new List<bool>(data);
                    rightPath.Add(true); //Add a '1'
                    right = Right.Traverse(ch, rightPath);
                }

                //w zależności od tego, czy liść(bez potomków) znajduje się po lewej, czy po prawej, zwraca jego kod
                return (left != null) ? left : right;
            }
        }
        /// <summary>
        /// Metoda sprawdzająca, czy dany węzeł jest liściem (nie posiada potomków)
        /// </summary>
        /// <returns>True gdy węzeł nie ma potomków, false w przeciwnym wypadku</returns>
        public bool IsLeaf()
        {
            return (null == this.Left && null == this.Right);
        }
    }
}
