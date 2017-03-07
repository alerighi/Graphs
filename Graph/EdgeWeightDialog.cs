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
    public partial class EdgeWeightDialog : Form
    {
        private Edge edge;
        private static readonly Random rand = new Random();

        public int Weight { get; private set; }
        public bool Bidirectional { get; private set; }

        public EdgeWeightDialog()
        {
            InitializeComponent();
            numericUpDown1.Value = rand.Next(1, 20);
        }

        public EdgeWeightDialog(Edge e)
        {
            edge = e;
            InitializeComponent();
            numericUpDown1.Value = edge.Weight;
            checkBox1.Checked = edge.Bidirectional;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (edge != null)
            {
                edge.Weight = (int) numericUpDown1.Value;
                edge.Bidirectional = checkBox1.Checked;
            }
            Bidirectional = checkBox1.Checked;
            Weight = (int) numericUpDown1.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
