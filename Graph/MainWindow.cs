using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{

    public partial class MainWindow : Form
    {
        private readonly Graphics graphics;
        private Graph graph;

        private bool changed;
        private bool selectingEdge;

        private readonly object selection = new object();

        private string fileName = "";

        private const string DefaultStatus = "Ready";
        private const string ProgramTitle = "Graphs";

        public override string Text
        {
            get { return base.Text; }
            set { base.Text = ProgramTitle + " - " + value; }
        }

        private Vertex dragging;
        private int tmpX, tmpY;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainWindow());
        }


        public MainWindow()
        {
            InitializeComponent();
            pictureBox1.MouseDown += OnPictureBox1OnMouseDown;
            pictureBox1.MouseMove += PictureBox1OnMouseMove;
            pictureBox1.MouseUp += PictureBox1OnMouseUp;
            pictureBox1.MouseDoubleClick += PictureBox1OnMouseDoubleClick;
            Image image = new Bitmap(Graph.Size.Width, Graph.Size.Height, PixelFormat.Format24bppRgb);
            pictureBox1.Image = image;
            graphics = Graphics.FromImage(image);
            NewGraph("NewGraph");
            statusStrip1.Text = DefaultStatus;
        }

        private void PictureBox1OnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            var edge = graph.EdgeOnPosition(mouseEventArgs.X, mouseEventArgs.Y);
            if (edge == null) return;
            using (var dialog = new EdgeWeightDialog(edge))
                dialog.ShowDialog(this);
            UpdateGraphics();
        }

        private void PictureBox1OnMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            dragging = null;
        }

        private void PictureBox1OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (dragging != null && mouseEventArgs.X > 0 && mouseEventArgs.Y > 0
                && mouseEventArgs.X < Graph.Size.Width && mouseEventArgs.Y < Graph.Size.Height)
            {
                dragging.X = mouseEventArgs.X - tmpX;
                dragging.Y = mouseEventArgs.Y - tmpY;
                UpdateGraphics();
            }
        }

        private void RemoveEdge(int x, int y)
        {
            var edge = graph.EdgeOnPosition(x, y);
            if (edge != null)
            {
                graph.RemoveEdge(edge);
                UpdateGraphics();
            }
            selectingEdge = false;
            status.Text = DefaultStatus;
        }

        private void OnPictureBox1OnMouseDown(object sender, MouseEventArgs args)
        {
            if (selectingEdge)
            {
                RemoveEdge(args.X, args.Y);
                return;
            }
            dragging = graph.VertexOnPoint(args.X, args.Y);
            Monitor.Enter(selection);
            Monitor.Pulse(selection);
            Monitor.Exit(selection);

            UpdateGraphics();
            if (dragging == null) return;
            tmpX = args.X - dragging.X;
            tmpY = args.Y - dragging.Y;

        }

        private void CancelPendingSelections()
        {
            Monitor.Enter(selection);
            dragging = null;
            Monitor.Pulse(selection);
            Monitor.Exit(selection);
            ResetColor();
            UpdateGraphics();   
        }

        private  Task<Vertex> SelectOneVertex() => Task.Factory.StartNew(() =>
            {
                Monitor.Enter(selection);
                status.Text = "Select one vertex";
                Monitor.Wait(selection);
                var result = dragging;
                status.Text = DefaultStatus;
                Monitor.Exit(selection);
                if (result != null)
                    result.Color = true;
                return result;
            });
        

        private Task<Tuple<Vertex, Vertex>> SelectTwoVertices() => Task.Factory.StartNew(() =>
            {
                Monitor.Enter(selection);
                status.Text = "Select the first vertex";
                Monitor.Wait(selection);
                var first = dragging;
                dragging = null;
                if (first == null)
                {
                    Monitor.Exit(selection);
                    return null;
                }
                first.Color = true;
                status.Text = "select the second vertex";
                Monitor.Wait(selection);
                var second = dragging;
                if (dragging != null)
                    dragging.Color = true;
                dragging = null;
                status.Text = DefaultStatus;
                Monitor.Exit(selection);
                if (second == null)
                    return null;
                return new Tuple<Vertex, Vertex>(first, second);
            });

        private void addVertex_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            using (var dialog = new NewVertexDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                if (graph.Vertices.ContainsKey(dialog.Name))
                {
                    MessageBox.Show("Vertex already in graph!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var v = new Vertex(dialog.Name, 300, 300);
                dialog.Dispose();
                graph.AddVertex(v);
                dragging = v;
                UpdateGraphics();
            }
        }

        private async void deleteVertex_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            var vertex = await SelectOneVertex();
            if ( vertex != null &&
                MessageBox.Show("Really delete vertex ?", "Delete Vertex", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                graph.DeleteVertex(vertex);
            }
            dragging = null;
            ResetColor();
            UpdateGraphics();
        }

        private async void edgeButton_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            var vertices = await SelectTwoVertices();
            if (vertices == null) return;
            using (var dialog = new EdgeWeightDialog())
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    graph.AddEdge(vertices.Item1, vertices.Item2, dialog.Weight, dialog.Bidirectional);
            ResetColor();
            UpdateGraphics();
        }

        public void UpdateGraphics()
        {
            graph.Draw(graphics);
            changed = true;
            pictureBox1.Refresh();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName.Length < 2)
                saveWithNameToolStripMenuItem_Click(null, null);
            else
                SaveFile(fileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                graph = Graph.FromFile(openFileDialog1.FileName);
                fileName = openFileDialog1.FileName;
                UpdateGraphics();
                changed = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            NewGraph();
        }

        private void NewGraph(string name)
        {
            graph = new Graph(name);
            Text = name;
            UpdateGraphics();
        }

        private void NewGraph()
        {
            using (var dialog = new GraphNameDialog("NewGraph"))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    NewGraph(dialog.Name);
                }
            }
        }

        private void ResetColor()
        {
            graph.ResetColors();
            UpdateGraphics();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox1 about = new AboutBox1())
                about.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                var result = MessageBox.Show("Save current graph ?", "Save", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
            }
            Application.Exit();
        }

        private void changeGraphNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new GraphNameDialog(graph.Name))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    graph.Name = dialog.Name;
                    Text = dialog.Name;
                    changed = true;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            selectingEdge = true;
            status.Text = "Select the edge to remove";
        }

        private void randomizeWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.RandomizeWeights();
            ResetColor();
        }

        private async void dijkstraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelPendingSelections();
            var elements = await SelectTwoVertices();
            if (elements != null)
            {
                try
                {
                    var result = Algorithm.Dijkstra(graph, elements.Item1, elements.Item2);
                    graph.ColorListOfVertices(result.Item1);
                    status.Text = $"Dijkstra: path from {elements.Item1} to {elements.Item2} costs {result.Item2}";
                }
                catch (Algorithm.NoSuchPathException)
                {
                    MessageBox.Show($"No path exists from {elements.Item1} to {elements.Item2}!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetColor();
                }
            }
            UpdateGraphics();
            changed = false;
        }

        private void resetColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetColor();
        }

        private void exportImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveImageDialog.ShowDialog(this) == DialogResult.OK)
            {
                ImageFormat format;
                string extension = saveImageDialog.FileName.Split('.')[1];
                switch (extension)
                {
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "bpm":
                        format = ImageFormat.Bmp;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    default:
                        format = ImageFormat.Jpeg;
                        break;
                }
                pictureBox1.Image.Save(saveImageDialog.FileName, format);
            }
        }

        private void saveWithNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                SaveFile(saveFileDialog1.FileName);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            if (changed)
            {
                var result = MessageBox.Show("Save current graph ?", "Save", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
            }
        }

        private void SaveFile(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(graph.ToString());
            writer.Close();
            fileName = filename;
            changed = false;
        }
    }
}
