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
    public class Graph
    {
        /// <summary>
        /// Vertices of the graph
        /// </summary>
        public Dictionary<string, Vertex> Vertices { get; } = new Dictionary<string, Vertex>();

        /// <summary>
        /// Edges of the graph
        /// </summary>
        private HashSet<Edge> Edges { get; } = new HashSet<Edge>();

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
        /// The font for the strings
        /// </summary>
        private static readonly Font Font = new Font("Arial", 14);

        /// <summary>
        /// Size of the graph image
        /// </summary>
        public static Size Size { get; } = new Size(1500, 800);

        /// <summary>
        /// Radius of one vertex
        /// </summary>
        private const int Radius = 20;

        /// <summary>
        /// Constructs a new graph object
        /// </summary>
        /// <param name="name">name of the graph</param>
        public Graph(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Calculates total grade (number of edges) of the graph 
        /// </summary>
        /// <returns>grade of the graph</returns>
        public int Grade() => Edges.Sum(edge => edge.Bidirectional ? 2 : 1);

        /// <summary>
        /// Checks if the graph is oriented or not
        /// </summary>
        /// <returns>true if the graph is oriented</returns>
        public bool IsOriented() => Edges.Any(edge => !edge.Bidirectional);

        /// <summary>
        /// Adds the a vertex to the graph
        /// </summary>
        /// <param name="vertex">the vertex to add</param>
        public virtual void AddVertex(Vertex vertex) => Vertices.Add(vertex.Name, vertex);
        

        /// <summary>
        /// Adds an edge to the graph
        /// </summary>
        /// <param name="from">Vertex to start from</param>
        /// <param name="to">Vertex to connect to</param>
        /// <param name="weight">weight of the edge</param>
        /// <param name="bidirectional">true if the edge is bidirectional</param>
        /// <exception cref="Exception">from or to doesn't exist in this graph</exception>
        public void AddEdge(Vertex from, Vertex to, int weight, bool bidirectional = true)
        {
            if (!Vertices.Values.Contains(from) || !Vertices.Values.Contains(to))
                throw new VertexNotInGraphException();
            Edges.Add(new Edge(from, to, weight, bidirectional));
        }

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="edge">The edge to remove</param>
        public virtual void RemoveEdge(Edge edge)
        {
            if (Edges.Contains(edge))
            {
                Edges.Remove(edge);
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
            if (Vertices.ContainsKey(name))
                return Vertices[name];
            var vertex = new Vertex(name);
            Vertices.Add(name, vertex);
            return vertex;
        }

        /// <summary>
        /// Iterators that iterates on the VerticesPosition of the graph
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => Vertices.Values.GetEnumerator();

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
            result += $"; number of vertices = {Vertices.Count}\r\n";
            result += $"; number of edges = {Grade()}\r\n";
            result += "; Edges: \r\n";
            result = Edges.Aggregate(result, (current, edge) => $"{current}    {edge}\r\n");
            result += "%Vertices\r\n";
            result = Vertices.Values.Aggregate(result, (current, vertex) => $"{current}{vertex}\r\n");
            result += "%EndFile\r\n";
            return result;
        }

        /// <summary>
        /// Remove a vertex from the graph, and the associated edges
        /// </summary>
        /// <param name="v">The vertex to remove</param>
        public void DeleteVertex(Vertex v)
        {
           if (!Vertices.ContainsKey(v.Name))
              return;
           
           var toRemove = new List<Edge>();
           foreach (var edge in Edges)
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
                Edges.Remove(edge);
            }
            Vertices.Remove(v.Name);
        }

        /// <summary>
        /// Resets all the weights with random values in range 1..20
        /// </summary>
        public void RandomizeWeights()
        {
            var rand = new Random();
            foreach (var edge in Edges)
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
                return graph;
            }
        }

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="g">The graphics object to draw on</param>
        public void Draw(Graphics g)
        {   // TODO: sistemare sto schifo
            var arrow = new AdjustableArrowCap(5, 5);
            var pen1 = new Pen(Color.Black, 2);
            pen1.CustomEndCap = arrow;
            var pen2 = new Pen(Color.Black, 2);
            pen2.CustomStartCap = arrow;
            pen2.CustomEndCap = arrow;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, Size.Width, Size.Height));
            foreach (var v in Vertices.Values)
            { 
                var r = new Rectangle(v.X - 20, v.Y - 20, 40, 40);
                g.FillEllipse(v.Color ? Brushes.LightCoral : Brushes.LightGreen, r);
                pen1.Color = v.Color ? Color.Red : Color.Black;
                g.DrawEllipse(pen1, r);
                r.X -= 10;
                r.Width += 20;
                g.DrawString(v.Name, Font, Brushes.Black, r, Format);

            }
            foreach (var e in Edges)
            {
                var pen = e.Bidirectional ? pen2 : pen1;
                pen.Color = e.Color ? Color.Red : Color.Black;
                var middleX = (e.From.X + e.To.X) / 2;
                var middleY = (e.From.Y + e.To.Y) / 2;
                var m = Math.Atan2(e.From.Y - e.To.Y, e.From.X - e.To.X);
                g.DrawLine(pen, (float)-Math.Cos(m) * Radius + e.From.X, (float)-Math.Sin(m) * Radius + e.From.Y,
                    (float)Math.Cos(m) * Radius + e.To.X, (float)Math.Sin(m) * Radius + e.To.Y);
                g.DrawString(e.Weight.ToString(), Font, Brushes.OrangeRed, middleX - 10, middleY - 20);
            }
        }

        /// <summary>
        /// Resets the color of the graph
        /// </summary>
        public void ResetColors()
        {
            foreach (var vertex in Vertices.Values)
                vertex.Color = false;
            foreach (var edge in Edges)
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
            return Vertices.Values.FirstOrDefault(
                v => x > v.X - Radius && x < v.X + Radius && v.Y - Radius < y && y < v.Y + Radius);
        }

        /// <summary>
        /// Returns an edge near the specified position
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>the edge on the specified position if it exists, else null</returns>
        public Edge EdgeOnPosition(int x, int y) => (
            from edge in Edges
            let x1 = edge.From.X
            let x2 = edge.To.X
            let y1 = edge.From.Y
            let y2 = edge.To.Y
            let r = Math.Abs((y2 - y1) * (x - x1) - (x2 - x1) * (y - y1))
            let d = (x2 - x1) * (x - x1) + (y2 - y1) * (y - y1)
            let di = (x - x1) * (x - x1) + (y - y1) * (y - y1)
            where r < 1000 && di < d
            select edge).FirstOrDefault();

        public void ColorListOfVertices(List<Vertex> vertices)
        {
            Vertex previous = null;
            foreach (var v in vertices)
            {
                v.Color = true;
                if (previous != null)
                {
                    foreach (var edge in Edges)
                        if (edge.From == previous && edge.To == v || edge.To == previous && edge.From == v)
                            edge.Color = true;
                }
                previous = v;
            }
        }

        /// <summary>
        /// Exception that gets thrown when you try to do something on a vertex that is not in the graph
        /// </summary>
        public class VertexNotInGraphException : Exception
        {
        }
    }
}
