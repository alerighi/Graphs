using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Graph
{
    /// <summary>
    /// A class that represents a vertex in a graph
    /// </summary>
    class Vertex
    {
        /// <summary>
        /// Edge dictionary, every Vertex that the edge connects to has associated its weight
        /// </summary>
        private readonly Dictionary<Vertex, int> edges = new Dictionary<Vertex, int>();
      
        /// <summary>
        /// Name of the vertex
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the exit grade of the node
        /// </summary>
        /// <returns>the exit grade of the node</returns>
        public int ExitGrade => edges.Count;

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
            edges.Add(to, weight);
        }

        /// <summary>
        /// Removes an edge
        /// </summary>
        /// <param name="to">destination vertex to remove</param>
        public void RemoveEdge(Vertex to)
        {
            edges.Remove(to);
        }

        /// <summary>
        /// Iterates on node neighbors
        /// </summary>
        /// <returns>Neighbors iterator</returns>
        public IEnumerator GetEnumerator() => edges.Keys.GetEnumerator();

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
        public int GetWeightTo(Vertex to) => edges[to];
    }

    /// <summary>
    /// A class that rappresents an edge in a graph
    /// </summary>
    class Edge
    {
        /// <summary>
        /// Vertex where the edge starts
        /// </summary>
        public Vertex From { get; }

        /// <summary>
        /// Vertex that the edge connects to
        /// </summary>
        public Vertex To { get; }

        /// <summary>
        /// Weight of the edge
        /// </summary>
        public int Weight { get; }

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
            Weight = weight;
            Bidirectional = bidirectional;
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
            return obj is Edge other && (other.From == From && other.To == To);
        }

        /// <summary>
        /// Returns string rappresentation of the object
        /// </summary>
        /// <returns>string rappresentation of the object</returns>
        public override string ToString() => From + (Bidirectional ? " <-> " : " -> ") + To + " : " + Weight;
    }

    /// <summary>
    /// A class that rappresents a graph
    /// </summary>
    class Graph
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
        public string Name { get; }

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
        /// Constructs a new graph object with the given name
        /// </summary>
        /// <param name="name">name of the graph</param>
        public Graph(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds the a vertex to the graph
        /// </summary>
        /// <param name="vertex">the vertex to add</param>
        public void AddNode(Vertex vertex)
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
            Edges.Add(new Edge(from, to, weight, bidirectional));
            from.AddEdge(to, weight);
            if (bidirectional)
                to.AddEdge(from, weight);
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
            string result = "Graph: " + Name + "\n";
            result += "number of vertices = " + Vertices.Count + "\n";
            result += "number of edges = " + Grade() + "\n";
            result += "Vertices: ";
            foreach (var node in Vertices.Values)
                result += node + ", ";
            result += "\b\b;\nEdges: {\n";
            foreach (var edge in Edges)
                result += "    " + edge + "\n";
            result += "}";
            return result;
        }

        /// <summary>
        /// Exception that gets thrown when you try to do something on a vertex that is not in the graph
        /// </summary>
        public class VertexNotInGraphException : Exception
        {
        }
    }
}
