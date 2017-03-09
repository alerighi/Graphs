using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class NewVertexDialog : Form
    {

        public new string Name { get; private set; }
        private static char lastLetter = 'A';

        public NewVertexDialog()
        {
            InitializeComponent();
            textBox1.Text = lastLetter.ToString();
            lastLetter++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Name = textBox1.Text;
            Name.Trim(' ');
            if (Name.Length > 0 && !Name.Contains(' '))
                DialogResult = DialogResult.OK;
            Close();
        }
    }
}
