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
        private static Random rand = new Random(); 
        public EdgeWeightDialog(Edge e)
        {
            edge = e;
            InitializeComponent();
            if (edge.Weight != 0)
            {
                numericUpDown1.Value = edge.Weight;
                checkBox1.Checked = edge.Bidirectional;
            }
            else
            {
                numericUpDown1.Value = rand.Next(1, 20);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edge.Weight = (int) numericUpDown1.Value;
            edge.Bidirectional = checkBox1.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
