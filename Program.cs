using System;

namespace GraphLabs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("graph.txt");
            graph.PrintIncidentsMatrix("inc.txt");
            graph.PrintAdjacencyMatrix("adj.txt");
        }
    }
}
