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
    public partial class GraphNameDialog : Form
    {
        public string Name { get; private set; }

        public GraphNameDialog(string name)
        {
            InitializeComponent();
            textBox1.Text = name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Name = textBox1.Text.Trim();
            if (Name.Length > 4 && !Name.Contains(' '))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Insert a name longer than 4 characters with no spaces", "Name not valid",
                    MessageBoxButtons.OK);
            }
        }
    }
}
