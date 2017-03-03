using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;

namespace Graph
{
    class MainClass
    {

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void Main()
        {

            try
            {
                var g = Graph.LoadGraphFromFile("..\\..\\samplegraph.txt");
                Console.WriteLine(g);
                var res = Algorithm.Dijkstra(g, g.Vertices["A"], g.Vertices["G"]);
                Console.WriteLine(Algorithm.DijkstraResultToString(res));
            }
            catch (Algorithm.NoSuchPathException)
            {
                Console.WriteLine("Path doesn't exists!");
            }

            

            Console.ReadKey(); // wait before closing the console
        }
    }
}
