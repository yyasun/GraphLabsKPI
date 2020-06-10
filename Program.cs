using System;

namespace GraphLabs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("graph.txt",true);
            Console.WriteLine(graph.DepthFirstSearch(1,4));
        }
    }
}
