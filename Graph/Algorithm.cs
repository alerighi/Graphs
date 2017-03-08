using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace Graph
{

    // type for Dijkstra algorithm result
    using DijkstraResult = Tuple<List<Vertex>, int>;

    /// <summary>
    /// Static class that contains common algorithms on graphs
    /// </summary>
    internal static class Algorithm
    {
        /// <summary>
        /// Get the distance vector of the distances of every node in graph from vertex s
        /// </summary>
        /// <param name="g">the graph to use</param>
        /// <param name="s">the source vertex</param>
        /// <returns>cector of the distances of every vertex from s</returns>
        public static Dictionary<Vertex, uint> DistanceVector(Graph g, Vertex s)
        {

            var distance = new Dictionary<Vertex, uint>(); // distance of v from s
            var Q = new Queue<Vertex>(); // FIFO queue

            foreach (Vertex v in g)
            {
                distance[v] = uint.MaxValue; // initial distance = infinite
            }

            distance[s] = 0; // distance of source with himself = 0
            Q.Enqueue(s);    // add s to queue

            while (Q.Count > 0) // while Q not empty
            {
                var min = Q.Dequeue(); // remove from queue
                foreach (Vertex v in min)
                {
                    if (distance[v] == uint.MaxValue) // if not already visited 
                    {
                        distance[v] = distance[min] + 1; // update distance
                        Q.Enqueue(v); // add to queue
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Find shortest path path from a to b with the classic Dijkstra algorithm 
        /// </summary>
        /// <param name="graph">Graph to use</param>
        /// <param name="a">starting vertex</param>
        /// <param name="b">destination vertex</param>
        /// <returns>A tuple containing a list of the nodes of the minimum path and the minimum cost</returns>
        /// <exception cref="NoSuchPathException">No path from a to b found</exception>
        public static DijkstraResult Dijkstra(Graph graph, Vertex a, Vertex b)
        {
            var distance = new Dictionary<Vertex, int>(); // distance from the origin
            var previous = new Dictionary<Vertex, Vertex>(); // precedent vertex
            var work = new List<Vertex>(); // list of working (not yet processed) vertex

            foreach (Vertex node in graph) // algorithm initialization
            {
                work.Add(node);
                distance[node] = int.MaxValue; // initial distance = infinite (int max)
            }

            distance[a] = 0; // distance from source vertex to itself equals 0

            while (work.Count > 0) // while there are elements b process
            {
                // find the element in Q with the smallest distance a origin
                //work.Sort((x, y) => distance[x] - distance[y]); // sort the queue
                var smallest = work[0];
                foreach (var node in work)
                    if (distance[node] < distance[smallest])
                        smallest = node;

                work.Remove(smallest); // remove smallest element from work set

                if (distance[smallest] == int.MaxValue) // no path a A b B
                    throw new NoSuchPathException();

                if (smallest == b) // found shortest path, build result
                {
                    var path = new List<Vertex>();
                    var precedent = smallest;
                    while (precedent != a)
                    {
                        path.Add(precedent); // add in reverse nodes to the list, using previous array
                        precedent = previous[precedent];
                    }
                    path.Add(a);
                    path.Reverse(); // reverse the list
                    return new DijkstraResult(path, distance[b]); // finsh!
                }

                foreach (Vertex neighbor in smallest) // for each neighbor of smallest 
                {
                    var alt = distance[smallest] + smallest.GetWeightTo(neighbor);
                        // new distance = distance from source to smallest + distance from smallest to neighbor
                    if (alt < distance[neighbor])
                        // if this distance is less than the distance from souce to neighbor then  
                    {
                        distance[neighbor] = alt; // update distance
                        previous[neighbor] = smallest; // path from neighbor to start passes trough smallest
                    }
                }
            }
            return null; // never reached
        }

        /// <summary>
        /// An exception that is thrown when the path from A to B doesn't exist
        /// </summary>
        public class NoSuchPathException : Exception
        {
        }
    }
}
