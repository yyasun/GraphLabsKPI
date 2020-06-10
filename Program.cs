using System;
using System.Xml;

namespace GraphLabs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("graph.txt",true);
            //graph.DepthFirstSearch(1,2);
            //graph.Vertecies = graph.Transpose();
            //graph.ShowGraph();
            Console.WriteLine("Count of strongly connected components: "+ graph.FindStronglyConnectedComponents()); 
            //Console.WriteLine(graph.DepthFirstSearch(1,6));
           //foreach(var i in graph.TopologicalSort())
           //     Console.Write(i.Value+" ");
           // graph.Vertecies = graph.Transpose();
           // graph.ShowGraph();
           // //graph.FindStronglyConnectedComponents();
        }
    }
}
