using System;

namespace GraphLabs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("graph.txt",false);
            Console.WriteLine(graph.BreadthFirstSearch(1, 4));
        }
    }
}
