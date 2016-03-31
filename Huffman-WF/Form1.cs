using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Huffman_WF
{
    public partial class Form1 : Form
    {
        public string original = null;
        public Tree HTree = new Tree();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = default(int);
            
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    original = File.ReadAllText(file);
                    size = original.Length;
                    textBox4.Text = original;
                    textBox5.Text = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                }
                catch (IOException exeption)
                {
                    MessageBox.Show("Error" + exeption.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HTree.BuildTree(original);
            BitArray encoded = HTree.Encode(original);
            string zero = string.Join(string.Empty, encoded.Cast<bool>().Select(bit => bit ? "1" : "0"));
            textBox3.Text = zero;
            byte[] bytes = new byte[(encoded.Length / 8) + 1];
            encoded.CopyTo(bytes, 0);
            string output = Encoding.Default.GetString(bytes);
            textBox2.Text = output;

            bool[] boolAr = new BitArray(Encoding.Default.GetBytes(output)).Cast<bool>().Take(HTree.BitCountForTree).ToArray();
            BitArray encoded2 = new BitArray(boolAr);

            string decoded = HTree.Decode(encoded2);
            textBox1.Text = decoded;
        }
    }
}
