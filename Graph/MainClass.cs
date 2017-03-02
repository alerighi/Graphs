using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Graph
{
    class MainClass
    {
       
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void Main()
        {
            var g = new Graph("Test");

            var A = new Vertex("A");
            var B = new Vertex("B");
            var C = new Vertex("C");
            var D = new Vertex("D");
            var E = new Vertex("E");
            var F = new Vertex("F");
            var G = new Vertex("G");

            g.AddNode(A);
            g.AddNode(B);
            g.AddNode(C);
            g.AddNode(D);
            g.AddNode(E);
            g.AddNode(F);
            g.AddNode(G);

            g.AddEdge(A, B, 7);
            g.AddEdge(A, E, 8);
            g.AddEdge(A, D, 3);
            g.AddEdge(B, C, 6);
            g.AddEdge(C, E, 1);
            g.AddEdge(C, G, 3);
            g.AddEdge(D, E, 4);
            g.AddEdge(D, F, 5);
            g.AddEdge(F, G, 2);

            Console.WriteLine(g);

            try
            {
                var res = Algorithm.Dijkstra(g, A, G);
                Console.WriteLine(Algorithm.DijkstraResultToString(res));
            }
            catch (Algorithm.NoSuchPathException)
            {
                Console.WriteLine("Path from " + A + " to " + B + "doesn't exists!");
            }

            Console.ReadKey(); // wait before closing the console
        }
    }
}
