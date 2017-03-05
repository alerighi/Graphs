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

        public static Size Size { get; } = new Size(1500, 800);

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
                    var vertex = GetOrCreate(node);
                    VerticesPosition.Add(vertex, new VertexComponent(vertex, posX, posY));
                }
            }
            foreach (var edge in Edges)
                ColorEdges.Add(edge, false);
        }

        public override string ToString()
        {
            string res = base.ToString();
            res += "; EndGraph\r\n";
            foreach (var vertex in VerticesPosition.Values)
            {
                res += "; " + vertex + "\r\n";
            }
            return res;
        }

        public void UpdateGraphics(Graphics g)
        {
            var arrow = new AdjustableArrowCap(5, 5);
            var pen1 = new Pen(Color.Black, 2);
            pen1.CustomEndCap = arrow;
            var pen2 = new Pen(Color.Black, 2);
            pen2.CustomStartCap = arrow;
            pen2.CustomEndCap = arrow;
            using (Font font = new Font("Arial", 14))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, Size.Width, Size.Height));
                foreach (var e in Edges)
                {
                    var pen = e.Bidirectional ? pen2 : pen1;
                    pen.Color = ColorEdges[e] ? Color.Red : Color.Black;
                    var x1 = VerticesPosition[e.From].X;
                    var y1 = VerticesPosition[e.From].Y;
                    var x2 = VerticesPosition[e.To].X;
                    var y2 = VerticesPosition[e.To].Y;
                    var middleX = (x1 + x2) / 2;
                    var middleY = (y1 + y2) / 2;
                    var m = Math.Atan2(y1 - y2, x1 - x2);
                    var y4 = (float) Math.Sin(m) * Radius + y2;
                    var x4 = (float) Math.Cos(m) * Radius + x2;
                    var y3 = (float) - Math.Sin(m) * Radius + y1;
                    var x3 = (float) - Math.Cos(m) * Radius + x1;
                    g.DrawLine(pen, x3, y3, x4, y4);        
                    g.DrawString(e.Weight.ToString(), font, Brushes.OrangeRed, middleX - 10, middleY - 20);
                }
                foreach (var v in VerticesPosition.Values)
                    v.Draw(g);

                
            }
        }

        public void ResetColors()
        {
            foreach (var vertex in VerticesPosition.Values)
                vertex.Color = false;
            foreach (var edge in ColorEdges.Keys.ToList())
                ColorEdges[edge] = false;
        }

        public VertexComponent VertexOnMousePosition(int x, int y)
        {
            foreach (var v in VerticesPosition.Values)
                if (x > v.X - Radius && x < v.X + Radius && v.Y - Radius < y && y < v.Y + Radius)
                    return v;
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
            AddVertex(vertex.V);
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

        private static readonly StringFormat Format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        private static readonly Pen pen = new Pen(System.Drawing.Color.Black, 2);
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
            using (Font font = new Font("Arial", 14))
            {
                Rectangle r = new Rectangle(X - 20, Y - 20, 40, 40);
                
                g.FillEllipse(Brushes.White, r);
                pen.Color = Color ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                g.DrawEllipse(pen, r);
                r.X -= 10;
                r.Width += 20;
                g.DrawString(V.Name, font, Brushes.Black, r, Format);
            }
        }

        public override string ToString() => V + "," + X + "," + Y;
    }
}
