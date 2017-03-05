using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Graph
{

    public partial class MainWindow : Form
    {
        private readonly Graphics graphics;
        private GraphRappresentation graph;

        private VertexComponent[] selected = new VertexComponent[2];

        private bool changed;
        private bool deleting;
        private bool selectingEdge;
        private bool doDijkstra;
        
        private string fileName = "";

        private const string DefaultStatus = "Ready";
        private const string ProgramTitle = "Graphs";

        public override string Text {
            get { return base.Text; }
            set { base.Text = ProgramTitle + " - " + value; }
        }


        private VertexComponent dragging;
        private int tmpX, tmpY;

        private bool selecting;
        public static AutoResetEvent SelectEvent = new AutoResetEvent(false);

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
            Image image = new Bitmap(GraphRappresentation.Size.Width, GraphRappresentation.Size.Height, PixelFormat.Format24bppRgb);
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
                && mouseEventArgs.X < GraphRappresentation.Size.Width && mouseEventArgs.Y < GraphRappresentation.Size.Height)
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
            dragging = graph.VertexOnMousePosition(args.X, args.Y);
            if (deleting && dragging != null)
            {
                if (MessageBox.Show("Really delete vertex ?", "Delete Vertex", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    graph.DeleteVertex(dragging.V);
                    graph.VerticesPosition.Remove(dragging.V);
                    UpdateGraphics();
                }
                status.Text = DefaultStatus;
                deleting = false;
                dragging = null;
                return;
            }
            deleting = false;
            if (selected[0] == null && selecting && dragging != null)
            {
                selected[0] = dragging;
                dragging.Color = true;
                UpdateGraphics();
                DoSelection();
                dragging = null;
            }
            else if (selected[1] == null && selecting && dragging != null && dragging != selected[0])
            {
                selected[1] = dragging;
                dragging.Color = true;
                UpdateGraphics();
                DoSelection();
                dragging = null;
            }
            else
            {
                selecting = false;
                ResetColor();
                UpdateGraphics();
                status.Text = DefaultStatus;
            }
            if (dragging == null) return;
            tmpX = args.X - dragging.X;
            tmpY = args.Y - dragging.Y;
        }

        private void addVertex_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewVertexDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) 
                    return;
                if (graph.Vertices.ContainsKey(dialog.Name))
                {
                    MessageBox.Show("Vertex already in graph!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                VertexComponent v = new VertexComponent(dialog.Name, 300, 300);
                dialog.Dispose();
                graph.AddVertex(v);
                dragging = v;
                UpdateGraphics();
            }
        }

        private void deleteVertex_Click(object sender, EventArgs e)
        {
            status.Text = "Select the vertex to delete";
            deleting = true;
        }

        private void edgeButton_Click(object sender, EventArgs e)
        {
            selected = new VertexComponent[2];
            selecting = true;
            DoSelection();
        }

        [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
        private void ExecDijkstra()
        {
            try
            {
                var result = Algorithm.Dijkstra(graph, selected[0].V, selected[1].V);
                Vertex previous = null;
                foreach (var v in result.Item1)
                {
                    graph.VerticesPosition[v].Color = true;
                    if (previous != null)
                    {
                        foreach (var edge in graph.Edges)
                            if (edge.From == previous && edge.To == v || edge.To == previous && edge.From == v)
                                graph.ColorEdges[edge] = true;
                    }
                    previous = v;
                }
                status.Text = "Dijkstra: path from " + selected[0].V + " to " + selected[1].V + " costs " + result.Item2;
                UpdateGraphics();
            }
            catch (Algorithm.NoSuchPathException)
            {
                MessageBox.Show("No path exists from " + selected[0].V + " to " + selected[1].V + "!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void DoSelection()
        {
            if (selected[0] == null)
                status.Text = "Select first Vertex";
            else if (selected[1] == null)
                status.Text = "Select second Vertex";
            else
            {
                if (doDijkstra)
                {
                    doDijkstra = false;
                    ExecDijkstra();
                    return;
                }
                Edge e = new Edge(selected[0].V, selected[1].V, 0, true);
                using (var dialog = new EdgeWeightDialog(e)) 
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;
                graph.AddEdge(e);
                graph.ColorEdges.Add(e, false);
                selected = new VertexComponent[2];
                selecting = false;
                status.Text = DefaultStatus;
                ResetColor();
                UpdateGraphics();
               
            }
        }

        public void UpdateGraphics()
        {
            graph.UpdateGraphics(graphics);
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
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                graph = new GraphRappresentation(openFileDialog1.FileName);
                fileName = openFileDialog1.FileName;
                UpdateGraphics();
                changed = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGraph();
        }

        private void NewGraph(string name)
        {
            graph = new GraphRappresentation();
            graph.Name = name;
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
                var result = MessageBox.Show("Save current graph ?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
            selectingEdge = true;
            status.Text = "Select the edge to remove";
        }

        private void randomizeWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.RandomizeWeights();
            ResetColor();
        }

        private void dijkstraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected = new VertexComponent[2];
            selecting = true;
            doDijkstra = true;
            ResetColor();
            DoSelection();
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
                var result = MessageBox.Show("Save current graph ?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
