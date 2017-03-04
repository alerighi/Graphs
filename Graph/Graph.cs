using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Graph
{
    /// <summary>
    /// A class that represents a vertex in a graph
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// Edge dictionary, every Vertex that the edge connects to has associated its weight
        /// </summary>
        public Dictionary<Vertex, int> Edges { get; } = new Dictionary<Vertex, int>();

        /// <summary>
        /// Name of the vertex
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the exit grade of the vertex
        /// </summary>
        public int ExitGrade => Edges.Count;

        /// <summary>
        /// Enter grade of the vertex
        /// </summary>
        public int EnterGrade { get; private set; }

        /// <summary>
        /// Totale grade of the vertex
        /// </summary>
        public int Grade => ExitGrade + EnterGrade;

        /// <summary>
        /// Constructor of a vertex
        /// </summary>
        /// <param name="name">The name of the vertex</param>
        public Vertex(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds an edge from this vertex
        /// </summary>
        /// <param name="to">vertex to connect</param>
        /// <param name="weight">weight of the edge</param>
        public void AddEdge(Vertex to, int weight)
        {
            Edges.Add(to, weight);
            to.EnterGrade += 1;
        }

        /// <summary>
        /// Removes an edge
        /// </summary>
        /// <param name="to">destination vertex to remove</param>
        public void RemoveEdge(Vertex to)
        {
            Edges.Remove(to);
        }

        /// <summary>
        /// Iterates on node neighbors
        /// </summary>
        /// <returns>Neighbors iterator</returns>
        public IEnumerator GetEnumerator() => Edges.Keys.GetEnumerator();

        /// <summary>
        /// Returns string rappresentation of the vertex, in practice its name
        /// </summary>
        /// <returns>string rappresentation of the vertex</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Gets the edge weight from this vertex to the one specified
        /// </summary>
        /// <param name="to">vertex to go to</param>
        /// <returns>the weight of the edge between this vertex and the one specified</returns>
        public int GetWeightTo(Vertex to) => Edges[to];

        /// <summary>
        /// Get the hash code of the vertex
        /// </summary>
        /// <returns>the hash code of the vertex</returns>
        public override int GetHashCode() => Edges.GetHashCode() ^ Name.GetHashCode();

        /// <summary>
        /// Check if 2 vertex are equal
        /// </summary>
        /// <param name="obj">second vertex to compare</param>
        /// <returns>true if equals</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vertex))
                return false;
            Vertex other = (Vertex) obj;
            return Edges.Equals(other.Edges) && other.Name == Name;
        }
    }

    /// <summary>
    /// A class that rappresents an edge in a graph
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Vertex where the edge starts
        /// </summary>
        public Vertex From { get; }

        /// <summary>
        /// Vertex that the edge connects to
        /// </summary>
        public Vertex To { get; }

        private int weight;

        /// <summary>
        /// Weight of the edge
        /// </summary>
        public int Weight {
            get { return weight ; }
            set
            {
                weight = value;
                if (From.Edges.ContainsKey(To)) From.Edges[To] = value;
                if (To.Edges.ContainsKey(From)) To.Edges[From] = value;
            }
        }

        /// <summary>
        /// true if the edge is bidirectional
        /// </summary>
        public bool Bidirectional { get; }

        /// <summary>
        /// Constructor of an edge
        /// </summary>
        /// <param name="from">vertex to connect from</param>
        /// <param name="to">vertex to connect to</param>
        /// <param name="weight">weight of the edge</param>
        /// <param name="bidirectional">true if bidirectional</param>
        public Edge(Vertex from, Vertex to, int weight, bool bidirectional)
        {
            From = from;
            To = to;
            Bidirectional = bidirectional;
            Weight = weight;
        }

        /// <summary>
        /// Calculate hash code for the edge.
        /// </summary>
        /// <returns>Hash code of the edge</returns>
        public override int GetHashCode() => From.GetHashCode() << 16 + To.GetHashCode();

        /// <summary>
        /// Check if two edges are equal
        /// </summary>
        /// <param name="obj">the other object</param>
        /// <returns>true if the other object is equal to this</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Edge))
                return false;
            Edge other = (Edge) obj;
            return other.From == From && other.To == To;
        }

        /// <summary>
        /// Returns string rappresentation of the object
        /// </summary>
        /// <returns>string rappresentation of the object</returns>
        public override string ToString() => From + (Bidirectional ? " <-> " : "  -> ") + To + " : " + Weight;
    }

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
        public HashSet<Edge> Edges { get; } = new HashSet<Edge>();

        /// <summary>
        /// Name of the graph
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructs a new graph object with the given name
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
        public int Grade()
        {
            int grade = 0;
            foreach (var edge in Edges)
            {
                grade += edge.Bidirectional ? 2 : 1;
            }
            return grade;
        }

        /// <summary>
        /// Checks if the graph is oriented or not
        /// </summary>
        /// <returns>true if the graph is oriented</returns>
        public bool IsOriented()
        {
            foreach (var edge in Edges)
                if (!edge.Bidirectional)
                    return true;
            return false;
        }

        /// <summary>
        /// Adds the a vertex to the graph
        /// </summary>
        /// <param name="vertex">the vertex to add</param>
        public void AddVertex(Vertex vertex)
        {
            Vertices.Add(vertex.Name, vertex);
        }

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
            Edge e = new Edge(from, to, weight, bidirectional);
            Edges.Add(e);
            from.AddEdge(to, weight);
            if (bidirectional)
                to.AddEdge(from, weight);
        }

        /// <summary>
        /// Adds an edge to the graph
        /// </summary>
        /// <param name="edge">The edge to add</param>
        public void AddEdge(Edge edge)
        {
            AddEdge(edge.From, edge.To, edge.Weight, edge.Bidirectional);
        }

        public void RemoveEdge(Edge edge)
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
        /// Iterators that iterates on the vertices of the graph
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => Vertices.Values.GetEnumerator();

        /// <summary>
        /// Returns a string rappresentation of the graph.
        /// General informations such name, number of vertices, grade are shown, 
        /// with the list of vertices and edges
        /// </summary>
        /// <returns>string rappresentation of the graph</returns>
        public override string ToString()
        {
            string result = "; Graph: " + Name + "\n";
            result += "; the graph is " + (IsOriented() ? "" : "not ") + "oriented\n";
            result += "; number of vertices = " + Vertices.Count + "\n";
            result += "; number of edges = " + Grade() + "\n";
            result += "; Vertices: ";
            foreach (var node in Vertices.Values)
                result += node + ", ";
            result += "\b\b;\n; Edges: \n";
            foreach (var edge in Edges)
                result += "    " + edge + "\n";
            return result;
        }

        /// <summary>
        /// Remove a vertex from the graph, and the associated edges
        /// </summary>
        /// <param name="v">The vertex to remove</param>
        [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
        public void DeleteVertex(Vertex v)
        {
           if (!Vertices.ContainsKey(v.Name))
              return;
           
           List<Edge> toRemove = new List<Edge>();
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
            Random rand = new Random();
            foreach (var edge in Edges)
            {
                edge.Weight = rand.Next(1, 21);
            }
        }

        /// <summary>
        /// Imports a graph from a file
        /// </summary>
        /// <param name="fileName">the filename</param>
        /// <returns>the graph loaded from file</returns>
        /// <exception cref="FileNotFoundException">the file cannot be found</exception>
        /// <exception cref="FormatException">the format of the graph file is not correct</exception>
        public static Graph LoadGraphFromFile(string fileName)
        {
            using (var fileReader = new StreamReader(fileName))
            {
                var line = fileReader.ReadLine();
                if (line == null)
                    return null;
                var name = line.Split(':')[1].Trim();
                var graph = new Graph(name);

                while ((line = fileReader.ReadLine()) != null)
                {
                    line = line.Split(';')[0]; // remove comments

                    if (line.Length < 1) continue;

                    var p = line.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);

                    if (p.Length < 2)
                        throw new FormatException();

                    int weight;
                    if (!Int32.TryParse(p[1], out weight))
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

                    graph.AddEdge(a, b, weight, bidirectional: bidirectional);

                }
                return graph;
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
