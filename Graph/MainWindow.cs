using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Graph
{

    public partial class MainWindow : Form
    {

        private Dictionary<Vertex, VertexComponent> vertices;
        private readonly Image image;
        private Graph graph;
        private const int Radius = 20;
        private VertexComponent[] selected = new VertexComponent[2];
        private readonly Size size = new Size(1500, 800);
        private bool changed;
        private bool deleting;
        private bool selectingEdge;
        private bool doDijkstra;
        private const string DefaultStatus = "Ready";
        private string fileName;
        private Dictionary<Edge, bool> colorEdges;

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
            image = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            pictureBox1.Image = image;
            NewGraph("NewGraph");
            statusStrip1.Text = DefaultStatus;
        }

        private void PictureBox1OnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            var edge = EdgeOnPosition(mouseEventArgs.X, mouseEventArgs.Y);
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
                && mouseEventArgs.X < size.Width && mouseEventArgs.Y < size.Height)
            {
                dragging.X = mouseEventArgs.X - tmpX;
                dragging.Y = mouseEventArgs.Y - tmpY;
                UpdateGraphics();
            }
        }

        private VertexComponent VertexOnMousePosition(int x, int y)
        {
            foreach (var v in vertices.Values)
            {
                if (x > v.X - Radius && x < v.X + Radius && v.Y - Radius < y && y < v.Y + Radius)
                    return v;
            }
            return null;
        }

        private static bool NearTo(double x1, double y1, double x2, double y2, double x, double y)
        {

            var r = Math.Abs((y2 - y1) * (x - x1) - (x2 - x1) * (y - y1));
            var d = (x2 - x1) * (x - x1) + (y2 - y1) * (y - y1);
            var di = (x - x1) * (x - x1) + (y - y1) * (y - y1);
            return r < 1000 && di < d;
        }

        private Edge EdgeOnPosition(int x, int y)
        {
            foreach (var edge in graph.Edges)
            {
                var x1 = vertices[edge.From].X;
                var x2 = vertices[edge.To].X;
                var y1 = vertices[edge.From].Y;
                var y2 = vertices[edge.To].Y;
                if (NearTo(x1, y1, x2, y2, x, y) || NearTo(x2, y2, x1, y1, x, y))
                    return edge;
            }
            return null;
        }

        private void RemoveEdge(int x, int y)
        {
            var edge = EdgeOnPosition(x, y);
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
            dragging = VertexOnMousePosition(args.X, args.Y);
            if (deleting && dragging != null)
            {
                if (MessageBox.Show("Really delete vertex ?", "Delete Vertex", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    graph.DeleteVertex(dragging.V);
                    vertices.Remove(dragging.V);
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


        public void SetGraph(Graph g)
        {
            graph = g;
            Text = g.Name;
            colorEdges = new Dictionary<Edge, bool>();
            vertices = new Dictionary<Vertex, VertexComponent>();
            int x = 40;
            int y = 40;
            foreach (var v in g.Vertices.Values)
            {
                vertices.Add(v, new VertexComponent(v, x, y));
                x += 100;
                if (x / 300 > 0)
                {
                    y += 100;
                    x = 80;
                }
            }
            foreach (var edge in graph.Edges)
                colorEdges.Add(edge, false);
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

                Vertex v = new Vertex(dialog.Name);
                dialog.Dispose();
                graph.AddVertex(v);
                VertexComponent v2 = new VertexComponent(v, 300, 300);
                vertices.Add(v, v2);
                dragging = v2;
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
                    vertices[v].Color = true;
                    if (previous != null)
                    {
                        foreach (var edge in graph.Edges)
                            if (edge.From == previous && edge.To == v || edge.To == previous && edge.From == v)
                                colorEdges[edge] = true;
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
                colorEdges.Add(e, false);
                selected = new VertexComponent[2];
                selecting = false;
                status.Text = DefaultStatus;
                ResetColor();
                UpdateGraphics();
               
            }
        }

        public void UpdateGraphics()
        {
            Font font = new Font("Arial", 14);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, size.Width, size.Height));
            foreach (var e in graph.Edges)
            {
                bool color = colorEdges[e];
                g.DrawLine(color ? Pens.Red : Pens.Black, vertices[e.From].X, vertices[e.From].Y, vertices[e.To].X, vertices[e.To].Y);
                int middleX = (vertices[e.From].X + vertices[e.To].X) / 2;
                int middleY = (vertices[e.From].Y + vertices[e.To].Y) / 2;
                g.DrawString(e.Weight.ToString(), font, Brushes.Black, middleX, middleY-20);


            }
            foreach (var v in vertices.Values)
                v.Draw(g);

            changed = true;
            pictureBox1.Refresh();
        }

        public override string ToString()
        {
            string res = graph.ToString();
            res += "; EndGraph\n";
            foreach (var vertex in vertices.Values)
            {
                res += "; " + vertex + "\n";
            }
            return res;
        }

        private void LoadFile(string filename)
        {
            fileName = filename;
            graph = Graph.LoadGraphFromFile(filename);
            SetGraph(graph);
            vertices = new Dictionary<Vertex, VertexComponent>();
            using (var fileReader = new StreamReader(filename))
            {
                string line;
                do
                {
                    line = fileReader.ReadLine();
                }
                while (line != null && !line.Contains("EndGraph"));
                while ((line = fileReader.ReadLine()) != null)
                {
                    if (line.Length < 4) continue;
                    line = line.Split(';')[1];
                    var parts = line.Split(',');
                    var node = parts[0].Trim();
                    var posX = Int32.Parse(parts[1].Trim());
                    var posY = Int32.Parse(parts[2].Trim());
                    var vertex = graph.Vertices[node];
                    vertices.Add(vertex, new VertexComponent(vertex, posX, posY));
                }
            }
            UpdateGraphics();
            changed = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (fileName.Length < 2)
                saveWithNameToolStripMenuItem_Click(sender, e);
           SaveFile(fileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                LoadFile(openFileDialog1.FileName);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGraph();
        }

        private void NewGraph(string name)
        {
            SetGraph(new Graph(name));
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
            foreach (var vertex in vertices.Values)
            {
                vertex.Color = false;
            }
            foreach (var edge in colorEdges.Keys.ToList())
            {
                colorEdges[edge] = false;
            }
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
            writer.WriteLine(ToString());
            writer.Close();
            fileName = filename;
        }
    }


    public class VertexComponent
    {

        public bool Color { get; set; }

        public VertexComponent(Vertex v, int x, int y)
        {
            V = v;
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Vertex V { get; set; }

        public void Draw(Graphics g)
        {
            Font font = new Font("Arial", 14);
            Rectangle r = new Rectangle(X-20, Y-29, 40, 40);
            g.FillEllipse(Brushes.White, r);
            g.DrawEllipse(Color ? Pens.Red : Pens.Black, r);
            g.DrawString(V.Name, font, Brushes.Black, X -15 , Y - 20);
        }

        public override string ToString()
        {
            return V + "," + X + "," + Y;
        }
    }

}
