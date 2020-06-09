using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphLabs
{
    public class Graph
    {
        private int edgesCount;
        public Vertex[] Vertexes { get; set; }
        public Graph()
        {
            Vertexes = new Vertex[10];
        }
        public void LoadGraph(string fileName)
        {
            var list = File.ReadAllLines(fileName);

            var info = list[0].Split(' ');
            int count = Convert.ToInt32(info[0]);
            edgesCount = Convert.ToInt32(info[1]);

            Vertexes = new Vertex[count];

            foreach (var line in list.Skip(1))
            {
                var edge = line.Split(' ');
                int value = Convert.ToInt32(edge[0]);
                int endPoint = -1;
                if (edge.Length > 1)
                    endPoint = Convert.ToInt32(edge[1]);
                if (Vertexes[value] == null)
                    Vertexes[value] = new Vertex() { Value = value };
                if (endPoint > 0)
                {
                    var end = new Vertex();
                    if (Vertexes[endPoint] == null)
                    {
                        end.Value = endPoint;
                        Vertexes[endPoint] = end;
                    }
                    Vertexes[value].Adjacent.Add(end);
                }
            }
        }
        public void IncidentsMatrix(string filename)
        {
            Action<string, TextWriter> write;
            using (TextWriter tw = new StreamWriter(filename))
            {
                if (!String.IsNullOrEmpty(filename))
                    write = new Action<string, TextWriter>(WriteToFile);
                else write = new Action<string, TextWriter>(WriteToConsole);

                for (int i = 1; i <= edgesCount; i++)
                    Console.Write("e" + i + " ");
                for (int i = 0; i < Vertexes.Length; i++)
                {
                    Console.WriteLine();
                    Console.Write("v" + i + 1 + " ");
                    for (int j = 0; j < Vertexes.Length; j++)
                    {
                        Console.Write(Vertexes[i].Adjacent.Find(x => x.Value == j + 1));
                    }
                }
            }
        }
        public void WriteToFile(string str, TextWriter tw)
        {
            tw.Write(str);
        }
        public void WriteToConsole(string str, TextWriter tw = null)
        {
            Console.Write(str);
        }
        public void AdjacencyMatrix(string filename = null)
        {
            Action<string, TextWriter> write;

            using (TextWriter tw = new StreamWriter(filename))
            {
                if (!String.IsNullOrEmpty(filename))
                    write = new Action<string, TextWriter>(WriteToFile);
                else write = new Action<string, TextWriter>(WriteToConsole);
                string endL = "\r\n";

                for (int i = 1; i <= Vertexes.Length; i++)
                    write("v" + i + " ", tw);
                for (int i = 0; i < Vertexes.Length; i++)
                {
                    write(endL, tw);
                    Console.Write("v" + i + 1 + " ");
                    for (int j = 0; j < Vertexes.Length; j++)
                    {
                        int exists = Convert.ToInt32(Vertexes[i].Adjacent.Find(x => x.Value == j + 1) != null);
                        write(exists.ToString(), tw);
                    }
                }
            }
        }
        public bool IsKComplete()
        {
            int k = Vertexes[0].Value;
            bool isKComplete = true;
            foreach (var i in Vertexes)
            {
                isKComplete = k == i.Value;
            }
            return isKComplete;
        }
        public void DegreeEachVertex(bool toFile = false)
        {

            if (IsKComplete())
                Console.WriteLine($"graph is k-complete: {Vertexes[0].Adjacent.Count}");
            foreach (var i in Vertexes)
            {
                Console.WriteLine($"v{i.Value} has degree: {i.Degree}");
            }
        }
        //public List<string> IsolatedVertexes()
        //{ 

        //}
        //public List<string> FloatingVertexes()
        //{ 

        //}
    }
}
