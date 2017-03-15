using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace Graph
{ 
    /// <summary>
    /// A class that rappresents a graph
    /// </summary>
    public sealed class Graph
    {
        /// <summary>
        /// Vertices of the graph
        /// </summary>
        private readonly Dictionary<string, Vertex> vertices  = new Dictionary<string, Vertex>();

        /// <summary>
        /// Edges of the graph
        /// </summary>
        private readonly HashSet<Edge> edges = new HashSet<Edge>();

        /// <summary>
        /// Name of the graph
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Static initializer for text format
        /// </summary>
        private static readonly StringFormat Format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        /// <summary>
        /// The old hash code of the ToString() of the object, to see if the object is changed
        /// </summary>
        private int oldCode;

        /// <summary>
        /// Checks if the graph has changed since last save (works 99.9% of the times!)
        /// </summary>
        public bool ChangedSinceLastSave => oldCode != ToString().GetHashCode();

        /// <summary>
        /// Size of the graph image
        /// </summary>
        public static Size Size { get; } = new Size(1500, 800);

        /// <summary>
        /// Radius of one vertex
        /// </summary>
        private const int Radius = 22;

        /// <summary>
        /// Check if a vertex with the given name is present in the graph
        /// </summary>
        /// <param name="name">name of the vertex</param>
        /// <returns>true if vertex is in the graph, else false</returns>
        public bool Contains(string name) => vertices.ContainsKey(name);

        /// <summary>
        /// Constructs a new graph object
        /// </summary>
        /// <param name="name">name of the graph</param>
        public Graph(string name)
        {
            Name = name;
            oldCode = ToString().GetHashCode();
        }

        /// <summary>
        /// Calculates total grade (number of edges) of the graph 
        /// </summary>
        /// <returns>grade of the graph</returns>
        public int Grade() => edges.Sum(edge => edge.Bidirectional ? 2 : 1);

        /// <summary>
        /// Checks if the graph is oriented or not
        /// </summary>
        /// <returns>true if the graph is oriented</returns>
        public bool IsOriented() => edges.Any(edge => !edge.Bidirectional);

        /// <summary>
        /// Adds the a vertex to the graph
        /// </summary>
        /// <param name="vertex">the vertex to add</param>
        public void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex.Name, vertex);
        }

        /// <summary>
        /// Adds an edge to the graph
        /// </summary>
        /// <param name="from">Vertex to start from</param>
        /// <param name="to">Vertex to connect to</param>
        /// <param name="weight">weight of the edge</param>
        /// <param name="bidirectional">true if the edge is bidirectional</param>
        /// <exception cref="Exception">from or to doesn't exist in this graph</exception>
        public void AddEdge(Vertex from, Vertex to, int weight, bool bidirectional = true) => edges.Add(new Edge(from, to, weight, bidirectional));

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="edge">The edge to remove</param>
        public void RemoveEdge(Edge edge)
        {
            if (edges.Contains(edge))
            {
                edges.Remove(edge);
                edge.From.RemoveEdge(edge.To);
                if (edge.Bidirectional)
                    edge.To.RemoveEdge(edge.From);
            }
        }

        /// <summary>
        /// If the vertex is already in the graph, it returns it.
        /// If not, create a new vertex, add it to the graph and returns it
        /// </summary>
        /// <param name="name">the name of the vertex</param>
        /// <returns>the vertex</returns>
        public Vertex GetOrCreate(string name)
        {
            if (vertices.ContainsKey(name))
                return vertices[name];
            var vertex = new Vertex(name);
            vertices.Add(name, vertex);
            return vertex;
        }

        /// <summary>
        /// Iterators that iterates on the VerticesPosition of the graph
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => vertices.Values.GetEnumerator();

        /// <summary>
        /// Returns a string rappresentation of the graph.
        /// General informations such name, number of VerticesPosition, grade are shown, 
        /// with the list of VerticesPosition and edges
        /// </summary>
        /// <returns>string rappresentation of the graph</returns>
        public override string ToString()
        {
            var result = $"; Graph: {Name}\r\n";
            result += $"; the graph is {(IsOriented() ? "" : "not ")} oriented\r\n";
            result += $"; number of vertices = {vertices.Count}\r\n";
            result += $"; number of edges = {Grade()}\r\n";
            result += "; Edges: \r\n";
            result = edges.Aggregate(result, (current, edge) => $"{current}    {edge}\r\n");
            result += "%Vertices\r\n";
            result = vertices.Values.Aggregate(result, (current, vertex) => $"{current}{vertex}\r\n");
            result += "%EndFile\r\n";
            return result;
        }

        /// <summary>
        /// Remove a vertex from the graph, and the associated edges
        /// </summary>
        /// <param name="v">The vertex to remove</param>
        public void DeleteVertex(Vertex v)
        {
           if (!vertices.ContainsKey(v.Name))
              return;
           
           var toRemove = new List<Edge>();
           foreach (var edge in edges)
           {
                if (edge.From == v)
                {
                    toRemove.Add(edge);
                }
                else if (edge.To == v)
                {
                    edge.From.RemoveEdge(v);
                    toRemove.Add(edge);
                }
            }
            foreach (var edge in toRemove)
            {
                edges.Remove(edge);
            }
            vertices.Remove(v.Name);
        }

        /// <summary>
        /// Resets all the weights with random values in range 1..20
        /// </summary>
        public void RandomizeWeights()
        {
            var rand = new Random();
            foreach (var edge in edges)
            {
                edge.Weight = rand.Next(1, 21);
            }
        }

        /// <summary>
        /// Imports a graph from a file
        /// </summary>
        /// <param name="fileName">the filename</param>
        /// <exception cref="FileNotFoundException">the file cannot be found</exception>
        /// <exception cref="FormatException">the format of the graph file is not correct</exception>
        public static Graph FromFile(string fileName)
        {
            using (var fileReader = new StreamReader(fileName))
            {
                var line = fileReader.ReadLine();
                if (line == null)
                    return null;
                var name = line.Split(':')[1].Trim();
                var graph = new Graph(name);
                while ((line = fileReader.ReadLine()) != null && !line.Contains("%Vertices"))
                {
                    line = line.Split(';')[0]; // remove comments

                    if (line.Length < 1) continue;

                    var p = line.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);

                    if (p.Length < 2)
                        throw new FormatException();

                    if (!int.TryParse(p[1], out var weight))
                        throw new FormatException();

                    var s = p[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length < 3)
                        throw new FormatException();

                    var a = graph.GetOrCreate(s[0]);
                    var b = graph.GetOrCreate(s[2]);

                    bool bidirectional;
                    if (s[1] == "<->") bidirectional = true;
                    else if (s[1] == "->") bidirectional = false;
                    else throw new FormatException();

                    graph.AddEdge(a, b, weight, bidirectional);
                }
                while ((line = fileReader.ReadLine()) != null && !line.Contains("%EndFile"))
                {
                    line = line.Split(';')[0]; // remove comments
                    if (line.Length < 4) continue;
                    var parts = line.Split(',');
                    var node = parts[0].Trim();
                    var vertex = graph.GetOrCreate(node);
                    vertex.X = int.Parse(parts[1].Trim());
                    vertex.Y = int.Parse(parts[2].Trim());
                }
                graph.oldCode = graph.ToString().GetHashCode();
                return graph;
            }
        }

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="g">The graphics object to draw on</param>
        public void Draw(Graphics g)
        {
            using (var arrow = new AdjustableArrowCap(5, 5))
            using (var penOneArrow = new Pen(Color.Black, 2) { CustomEndCap = arrow })
            using (var penTwoArrows = new Pen(Color.Black, 2) { CustomStartCap = arrow, CustomEndCap = arrow })
            using (var font = new Font("Arial", 14))
            {
                g.Clear(Color.White);
                foreach (var v in vertices.Values)
                {
                    var r = new Rectangle(v.X - Radius, v.Y - Radius, Radius * 2, Radius * 2);
                    g.FillEllipse(v.Color ? Brushes.LightCoral : Brushes.LightGreen, r);
                    penOneArrow.Color = v.Color ? Color.Red : Color.Black;
                    g.DrawEllipse(penOneArrow, r);
                    r.X -= 10;
                    r.Width += 20;
                    g.DrawString(v.Name + "\n" + v.Text, font, Brushes.Black, r, Format);

                }
                foreach (var e in edges)
                {
                    var pen = e.Bidirectional ? penTwoArrows : penOneArrow;
                    pen.Color = e.Color ? Color.Red : Color.Black;
                    var middleX = (e.From.X + e.To.X) / 2;
                    var middleY = (e.From.Y + e.To.Y) / 2;
                    var m = Math.Atan2(e.From.Y - e.To.Y, e.From.X - e.To.X);
                    g.DrawLine(pen, (float)-Math.Cos(m) * Radius + e.From.X, (float)-Math.Sin(m) * Radius + e.From.Y,
                        (float)Math.Cos(m) * Radius + e.To.X, (float)Math.Sin(m) * Radius + e.To.Y);
                    g.DrawString(e.Weight.ToString(), font, Brushes.OrangeRed, middleX - 10, middleY - 20);
                }
            }
        }

        /// <summary>
        /// Resets the color of the graph
        /// </summary>
        public void ResetColors()
        {
            foreach (var vertex in vertices.Values)
                vertex.Color = false;
            foreach (var edge in edges)
                edge.Color = false;
        }

        /// <summary>
        /// Returns a vertex on the givern position, if it exists
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>the vertex on the position if it exists, else null</returns>
        public Vertex VertexOnPoint(int x, int y)
        {
            return vertices.Values.FirstOrDefault(
                v => x > v.X - Radius && x < v.X + Radius && v.Y - Radius < y && y < v.Y + Radius);
        }

        /// <summary>
        /// Returns an edge near the specified position
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>the edge on the specified position if it exists, else null</returns>
        public Edge EdgeOnPosition(int x, int y) => (
            from edge in edges
            let x1 = edge.From.X
            let x2 = edge.To.X
            let y1 = edge.From.Y
            let y2 = edge.To.Y
            let r = Math.Abs((y2 - y1) * (x - x1) - (x2 - x1) * (y - y1))
            let d = (x2 - x1) * (x - x1) + (y2 - y1) * (y - y1)
            let di = (x - x1) * (x - x1) + (y - y1) * (y - y1)
            where r < 1000 && di < d
            select edge).FirstOrDefault();

        /// <summary>
        /// Sets the color of a list of vertices and the edge between them red
        /// </summary>
        /// <param name="verticesToColor">The list of vertices to color</param>
        public void ColorListOfVertices(List<Vertex> verticesToColor)
        {
            Vertex previous = null;
            foreach (var v in verticesToColor)
            {
                v.Color = true;
                if (previous != null)
                {
                    foreach (var edge in edges)
                        if (edge.From == previous && edge.To == v || edge.To == previous && edge.From == v)
                            edge.Color = true;
                }
                previous = v;
            }
        }

        /// <summary>
        /// Clear the addictional text of the vertices
        /// </summary>
        public void ClearVertexText()
        {
            foreach (var v in vertices.Values)
            {
                v.Text = "";
            }
        }

        /// <summary>
        /// Saves the current graph to file 
        /// </summary>
        /// <param name="fileName">the file to save graph to</param>
        public void SaveGraphToFile(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(ToString());
            }
            oldCode = ToString().GetHashCode();
        }

        /// <summary>
        /// Changes the name of a vertex in the graph
        /// </summary>
        /// <param name="vertex">vertex to change name</param>
        /// <param name="name">new name</param>
        public void ChangeVertexName(Vertex vertex, string name)
        {
            vertices.Remove(name);
            vertex.Name = name;
            vertices.Add(name, vertex);
        }
    }
}
