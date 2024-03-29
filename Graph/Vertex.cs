﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Graph
{
    /// <summary>
    /// A class that represents a vertex in a graph
    /// </summary>
    public sealed class Vertex
    {
        /// <summary>
        /// Edge dictionary, every Vertex that the edge connects to has associated its weight
        /// </summary>
        public Dictionary<Vertex, int> Edges { get; } = new Dictionary<Vertex, int>();

        /// <summary>
        /// Name of the vertex
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Additional text to show under the vertex name
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// X position of the vertex
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y position of the vertex
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Color of the vertex - true = red, false = black
        /// </summary>
        public bool Color { get; set; }

        /// <summary>
        /// Extended color attribute
        /// </summary>
        public Color ExtendedColor { get; set; } = System.Drawing.Color.LightCoral;

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
        /// Construct a new vertex on the given position
        /// </summary>
        /// <param name="name">name of the vertex</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public Vertex(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
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
            to.EnterGrade -= 1;
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
        public override string ToString() => Name + "," + X + "," + Y;

        /// <summary>
        /// Gets the edge weight from this vertex to the one specified
        /// </summary>
        /// <param name="to">vertex to go to</param>
        /// <returns>the weight of the edge between this vertex and the one specified</returns>
        public int GetWeightTo(Vertex to) => Edges[to];
    }
}
