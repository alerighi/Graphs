using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace Graph
{

    public class GraphRappresentation : Graph
    {

        public Dictionary<Vertex, VertexComponent> VerticesPosition { get; }
        public Dictionary<Edge, bool> ColorEdges { get; }

        private readonly Size size = new Size(1500, 800);

        private const int Radius = 20;

        public GraphRappresentation()
        {
            ColorEdges = new Dictionary<Edge, bool>();
            VerticesPosition = new Dictionary<Vertex, VertexComponent>();
        }

        public GraphRappresentation(string fileName) : base(fileName)
        {
            VerticesPosition = new Dictionary<Vertex, VertexComponent>();
            ColorEdges = new Dictionary<Edge, bool>();
            using (var fileReader = new StreamReader(fileName))
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
                    var vertex = Vertices[node];
                    VerticesPosition.Add(vertex, new VertexComponent(vertex, posX, posY));
                }
            }
            foreach (var edge in Edges)
                ColorEdges.Add(edge, false);
        }

        public override string ToString()
        {
            string res = base.ToString();
            res += "; EndGraph\n";
            foreach (var vertex in VerticesPosition.Values)
            {
                res += "; " + vertex + "\n";
            }
            return res;
        }

        public void UpdateGraphics(Graphics g)
        {
            Font font = new Font("Arial", 14);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, size.Width, size.Height));
            foreach (var e in Edges)
            {
                bool color = ColorEdges[e];
                g.DrawLine(color ? Pens.Red : Pens.Black, VerticesPosition[e.From].X, VerticesPosition[e.From].Y, VerticesPosition[e.To].X, VerticesPosition[e.To].Y);
                int middleX = (VerticesPosition[e.From].X + VerticesPosition[e.To].X) / 2;
                int middleY = (VerticesPosition[e.From].Y + VerticesPosition[e.To].Y) / 2;
                g.DrawString(e.Weight.ToString(), font, Brushes.Black, middleX, middleY - 20);


            }
            foreach (var v in VerticesPosition.Values)
                v.Draw(g);
        }

        public void ResetColors()
        {
            foreach (var vertex in VerticesPosition.Values)
            {
                vertex.Color = false;
            }
            foreach (var edge in ColorEdges.Keys.ToList())
            {
                ColorEdges[edge] = false;
            }
        }

        public VertexComponent VertexOnMousePosition(int x, int y)
        {
            foreach (var v in VerticesPosition.Values)
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

        public Edge EdgeOnPosition(int x, int y)
        {
            foreach (var edge in Edges)
            {
                var x1 = VerticesPosition[edge.From].X;
                var x2 = VerticesPosition[edge.To].X;
                var y1 = VerticesPosition[edge.From].Y;
                var y2 = VerticesPosition[edge.To].Y;
                if (NearTo(x1, y1, x2, y2, x, y) || NearTo(x2, y2, x1, y1, x, y))
                    return edge;
            }
            return null;
        }

        public void AddVertex(VertexComponent vertex)
        {
            base.AddVertex(vertex.V);
            VerticesPosition.Add(vertex.V, vertex);
        }

        public override void RemoveEdge(Edge edge)
        {
            base.RemoveEdge(edge);
            ColorEdges.Remove(edge);
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

        public VertexComponent(string name, int x, int y) : this(new Vertex(name), x, y)
        {
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Vertex V { get; set; }

        public void Draw(Graphics g)
        {
            Font font = new Font("Arial", 14);
            Rectangle r = new Rectangle(X - 20, Y - 29, 40, 40);
            g.FillEllipse(Brushes.White, r);
            g.DrawEllipse(Color ? Pens.Red : Pens.Black, r);
            g.DrawString(V.Name, font, Brushes.Black, X - 15, Y - 20);
        }

        public override string ToString()
        {
            return V + "," + X + "," + Y;
        }
    }
}
