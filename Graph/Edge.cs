using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
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

        /// <summary>
        /// Color of the edge - true = red, false = black 
        /// </summary>
        public bool Color { get; set; } // TODO: oggetto color 

        private int weight;
        private bool bidirectional;

        /// <summary>
        /// Weight of the edge
        /// </summary>
        public int Weight
        {
            get { return weight; }
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
        public bool Bidirectional
        {
            get { return bidirectional; }
            set
            {
                if (bidirectional != value)
                {
                    bidirectional = value;
                    if (bidirectional)
                        To.AddEdge(From, Weight);
                    else
                        To.RemoveEdge(From);
                }
            }
        }

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
            from.AddEdge(to, weight);
            Bidirectional = bidirectional;
        }

        /// <summary>
        /// Returns string rappresentation of the object
        /// </summary>
        /// <returns>string rappresentation of the object</returns>
        public override string ToString() => From.Name + (Bidirectional ? " <-> " : "  -> ") + To.Name + " : " + Weight;
    }
}
