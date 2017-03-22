using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;


namespace Graph
{

    /// <summary>
    /// Main Window for the application
    /// </summary>
    public partial class MainWindow : Form
    {
      
        private const string DefaultStatus = "Ready";
        private const string ProgramTitle = "Graphs";

        private readonly Graphics graphics;
        private Graph graph;
        private Vertex selected;
        private Point mousePosition;
        private Point tmp;
        private readonly object selectionMonitor = new object();
        private string fileName = "";

        /// <summary>
        /// Main for the applications
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainWindow());
        }

        /// <summary>
        /// Construct a new MainWindow
        /// </summary>
        public MainWindow()
        {
  
            InitializeComponent();

            // set mouse event listeners
            pictureBox1.MouseDown += OnPictureBoxMouseDown;
            pictureBox1.MouseMove += OnPictureBoxMouseMove;
            pictureBox1.MouseUp += OnPictureBoxMouseUp;
            pictureBox1.MouseDoubleClick += OnPictureBoxMouseDubleClick;

            // create image
            Image image = new Bitmap(Graph.Size.Width, Graph.Size.Height, PixelFormat.Format24bppRgb);
            pictureBox1.Image = image;

            // create graphics
            graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // create new graph
            NewGraph("NewGraph");

            // initialize status text
            status.Text = DefaultStatus;
        }

        // +------------------+
        // |   Mouse Events   |
        // +------------------+

        /// <summary>
        /// Left mouse button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPictureBoxMouseDown(object sender, MouseEventArgs args)
        {
            graph.ClearVertexText();

            selected = graph.VertexOnPoint(args.X, args.Y);
            Monitor.Enter(selectionMonitor);
            mousePosition.X = args.X;
            mousePosition.Y = args.Y;
            Monitor.Pulse(selectionMonitor);
            Monitor.Exit(selectionMonitor);

            UpdateGraphics();


            if (selected == null) return;
            tmp.X = args.X - selected.X;
            tmp.Y = args.Y - selected.Y;

        }

        /// <summary>
        /// Left mouse button relese event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnPictureBoxMouseUp(object sender, MouseEventArgs mouseEventArgs) => selected = null;

        /// <summary>
        /// Left mouse button double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnPictureBoxMouseDubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            var vertex = graph.VertexOnPoint(mouseEventArgs.X, mouseEventArgs.Y);
            if (vertex != null)
            {
                using (var dialog = new NewVertexDialog(vertex.Name))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!graph.Contains(dialog.Name))
                            graph.ChangeVertexName(vertex, dialog.Name);
                        else
                            MessageBox.Show(this, "Name already used in graph", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                var edge = graph.EdgeOnPosition(mouseEventArgs.X, mouseEventArgs.Y);
                if (edge == null) return;
                using (var dialog = new EdgeWeightDialog(edge))
                    dialog.ShowDialog(this);
            }
            UpdateGraphics();
        }

        /// <summary>
        /// Mouse move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnPictureBoxMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (selected != null && mouseEventArgs.X > 0 && mouseEventArgs.Y > 0
                && mouseEventArgs.X < Graph.Size.Width && mouseEventArgs.Y < Graph.Size.Height)
            {
                selected.X = mouseEventArgs.X - tmp.X;
                selected.Y = mouseEventArgs.Y - tmp.Y;
                UpdateGraphics();
            }
        }

        // +---------------------+
        // |   Selection Tasks   |
        // +---------------------+

        /// <summary>
        /// Task that waits for a left mouse click
        /// </summary>
        /// <returns></returns>
        private Task<Point> WaitMouseClick() => Task.Factory.StartNew(() =>
        {
            Monitor.Enter(selectionMonitor);
            Monitor.Wait(selectionMonitor);
            Monitor.Exit(selectionMonitor);
            selected = null;
            return mousePosition;
        });

        /// <summary>
        /// Async task to select a vertex
        /// </summary>
        /// <returns></returns>
        private async Task<Vertex> VertexSelectionTask()
        {
            var point = await WaitMouseClick();
            var vertex = graph.VertexOnPoint(point.X, point.Y);
            if (vertex != null)
                vertex.Color = true;
            UpdateGraphics();
            status.Text = DefaultStatus;
            return vertex;
        }

        /// <summary>
        /// Async task to select one vertex
        /// </summary>
        /// <returns></returns>
        private async Task<Vertex> SelectOneVertex()
        {
            ResetColor();
            status.Text = "Select one vertex";
            return await VertexSelectionTask();
        }

        /// <summary>
        /// Async task to select two vertices
        /// </summary>
        /// <returns></returns>
        private async Task<Tuple<Vertex, Vertex>> SelectTwoVertices()
        {
            ResetColor();
            status.Text = "Select first vertex";
            var first = await VertexSelectionTask();
            if (first == null)
                return null;

            status.Text = "Select second vertex";
            var second = await VertexSelectionTask();
            if (second == null)
                return null;

            return new Tuple<Vertex, Vertex>(first, second);
        }

        // +-------------------------------------+
        // |   Menu Items and Button Listeners   |
        // +-------------------------------------+

        /// <summary>
        /// AddVertex and Edit->Add Vertex click listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddVertexButtonClick(object sender, EventArgs e)
        {
            CancelPendingSelections();
            using (var dialog = new NewVertexDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                if (graph.Contains(dialog.Name))
                {
                    MessageBox.Show("Vertex already in graph!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var v = new Vertex(dialog.Name, 300, 300);
                graph.AddVertex(v);
                selected = v;
                UpdateGraphics();
            }
        }

        /// <summary>
        /// DeleteVertex button and Edit->Delete Vertex listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDeleteVertexButtonClick(object sender, EventArgs e)
        {
            CancelPendingSelections();
            var vertex = await SelectOneVertex();
            if (vertex != null &&
                MessageBox.Show("Really delete vertex ?", "Delete Vertex", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                graph.DeleteVertex(vertex);
            }
            selected = null;
            ResetColor();
            UpdateGraphics();
        }

        /// <summary>
        /// AddEdge and Edit->Add Edge click listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddEdgeButtonClick(object sender, EventArgs e)
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

        /// <summary>
        /// DeleteEdge and Edit->Delete Edge listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnRemoveEdgeButtonClick(object sender, EventArgs e)
        {
            CancelPendingSelections();
            status.Text = "Select the edge to remove";
            var position = await WaitMouseClick();
            var edge = graph.EdgeOnPosition(position.X, position.Y);
            if (edge != null)
            {
                graph.RemoveEdge(edge);
                UpdateGraphics();
            }
            status.Text = DefaultStatus;
        }

        /// <summary>
        /// File->New menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewMenuItemClick(object sender, EventArgs e)
        {
            CancelPendingSelections();
            NewGraph();
        }

        /// <summary>
        /// File->Load menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadMenuItemClick(object sender, EventArgs e)
        {
            CancelPendingSelections();
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                graph = Graph.FromFile(openFileDialog1.FileName);
                fileName = openFileDialog1.FileName;
                UpdateGraphics();
            }
        }

        /// <summary>
        /// File->Save menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveMenuItemClick(object sender, EventArgs e)
        {
            if (fileName.Length < 2)
                OnSaveWithNameMenuItemClick(null, null);
            else
                SaveFile(fileName);
        }

        /// <summary>
        /// File->Save With Name menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveWithNameMenuItemClick(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                SaveFile(saveFileDialog1.FileName);
        }

        /// <summary>
        /// File->Export Image menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExportImageMenuItemClick(object sender, EventArgs e)
        {
            if (saveImageDialog.ShowDialog(this) == DialogResult.OK)
            {
                ImageFormat format;

                switch (saveImageDialog.FileName.Split('.')[1])
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

        /// <summary>
        /// File->Exit menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitMenuItemClick(object sender, EventArgs e) => Close();

        /// <summary>
        /// Edit-.Change Graph Name menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeGraphNameMenuItemClick(object sender, EventArgs e)
        {
            using (var dialog = new GraphNameDialog(graph.Name))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    graph.Name = dialog.Name;
                    UpdateGraphics();
                }
            }
        }

        /// <summary>
        /// Edit->Randomize Weights menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomizeWeightsMenuItemClick(object sender, EventArgs e)
        {
            graph.RandomizeWeights();
            ResetColor();
        }

        /// <summary>
        /// Edit->Reset Colors menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResetColorsMenuItemClick(object sender, EventArgs e) => ResetColor();

        /// <summary>
        /// Algorithm->Dijkstra menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDijkstraMenuItemClick(object sender, EventArgs e)
        {
            CancelPendingSelections();
            var elements = await SelectTwoVertices();
            if (elements != null)
            {
                try
                {
                    var result = Algorithm.Dijkstra(graph, elements.Item1, elements.Item2);
                    graph.ColorListOfVertices(result.Item1);
                    status.Text =
                        $"Dijkstra: path from {elements.Item1.Name} to {elements.Item2.Name} costs {result.Item2}";
                }
                catch (Algorithm.NoSuchPathException)
                {
                    MessageBox.Show($"No path exists from {elements.Item1.Name} to {elements.Item2.Name}!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetColor();
                }
            }
            UpdateGraphics();
        }

        /// <summary>
        /// Algorithm->Distance Vector menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDistanceVectorMenuItemClick(object sender, EventArgs e)
        {
            var result = Algorithm.DistanceVector(graph, await SelectOneVertex());
            var distance = result.Item1;
            var s = "";
            var vertices = new List<Vertex>(distance.Keys);
            vertices.Sort((v1, v2) => (int) distance[v1] - (int) distance[v2]);
            foreach (var v in vertices)
                if (distance[v] != uint.MaxValue)
                    s += $"distance[{v.Name}] = {distance[v]}\r\n";
            foreach (var v in vertices)
                if (distance[v] == uint.MaxValue)
                    s += $"distance[{v.Name}] = Inf\r\n";
            MessageBox.Show(s, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = Algorithm.DFS(graph);
           
            foreach (Vertex v in graph)
            {
                v.Text = $"{res.Item1[v]}/{res.Item2[v]}";
            }

            UpdateGraphics();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopSortMenuItemClick(object sender, EventArgs e)
        {
            var res = Algorithm.DFS(graph);
            var result = res.Item3.Pop().Name;
            result = res.Item3.Aggregate(result, (current, v) => current + $" >> {v.Name}");
            MessageBox.Show(this, result, "Output", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Help->About menu listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAboutMenuItemClick(object sender, EventArgs e)
        {
            using (var about = new AboutBox1())
                about.ShowDialog(this);
        }


        /// <summary>
        /// Event that gets fired when the form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            if (graph.ChangedSinceLastSave)
            {
                var result = MessageBox.Show("Save current graph ?", "Save", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
                if (result == DialogResult.Yes)
                    OnSaveMenuItemClick(sender, e);
            }
        }

        // +-------------------------------+
        // |   Various Utility Functions   |
        // +-------------------------------+

        /// <summary>
        /// Saves graph to file
        /// </summary>
        /// <param name="filename"></param>
        private void SaveFile(string filename)
        {
            graph.SaveGraphToFile(filename);
            fileName = filename;
        }

        /// <summary>
        /// Resets graph colors
        /// </summary>
        private void ResetColor()
        {
            graph.ResetColors();
            UpdateGraphics();
        }

        /// <summary>
        /// Updates the graph view
        /// </summary>
        public void UpdateGraphics()
        {
            graph.Draw(graphics);
            Text = $"{ProgramTitle} - {(graph.ChangedSinceLastSave ? "*" : "")}{graph.Name} - {fileName}";
            pictureBox1.Refresh();
        }

        /// <summary>
        /// Create new graph, a dialog is shown to choose the name
        /// </summary>
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

        /// <summary>
        /// Create new graph, with the given name
        /// </summary>
        /// <param name="name">name of the graph</param>
        private void NewGraph(string name)
        {
            graph = new Graph(name);
            UpdateGraphics();
        }

        /// <summary>
        /// Cancels pending waiting for the user to click in the graph area
        /// Simply signal the monitor with a nonsense mouse position
        /// </summary>
        private void CancelPendingSelections()
        {
            Monitor.Enter(selectionMonitor);
            mousePosition.X = 0;
            mousePosition.Y = 0;
            Monitor.Pulse(selectionMonitor);
            Monitor.Exit(selectionMonitor);
            ResetColor();
            UpdateGraphics();
        }

        private void transposeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph = graph.Transpose();
            UpdateGraphics();
        }

        private void sCCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = Algorithm.SCC(graph);
            var color = 0;
            var result = "";
            foreach (var c in res)
            {
                foreach (var v in c)
                {
                    v.Color = true;
                    v.ExtendedColor = Color.FromArgb(255, color%255, (color+70)%255, (color+170)%255);
                    result += v.Name + ",";
                }
                result += "\n";
                color += 100;
            }
            UpdateGraphics();
            MessageBox.Show(result);
        }
    }
}
