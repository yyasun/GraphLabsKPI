using Microsoft.VisualBasic;
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
        public Graph(string fileName)
        {
            LoadGraph(fileName);
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
                if (Vertexes[value - 1] == null)
                    Vertexes[value - 1] = new Vertex() { Value = value };
                if (endPoint > 0)
                {
                    var end = new Vertex();
                    if (Vertexes[endPoint - 1] == null)
                    {
                        end.Value = endPoint;
                        Vertexes[endPoint - 1] = end;
                    }
                    Vertexes[value - 1].Adjacent.Add(end);
                    Vertexes[endPoint - 1].Adjacent.Add(Vertexes[value - 1]);
                }
            }
        }

        const string endL = "\r\n";
        void WriteToFile(string str, TextWriter tw)
        {
            tw.Write(str);
        }
        void WriteToConsole(string str, TextWriter tw = null)
        {
            Console.Write(str);
        }
        
        public void PrintIncidentsMatrix(string filename = null)
        {
            Action<string, TextWriter> write;
            TextWriter tw = null;
            if (!String.IsNullOrEmpty(filename))
            {
                tw = new StreamWriter(filename);
                write = new Action<string, TextWriter>(WriteToFile);
            }
            else write = new Action<string, TextWriter>(WriteToConsole);


           
            for (int i = 0,c=1; i < Vertexes.Length; i++)
            {
                foreach (var adj in Vertexes[i].Adjacent)
                {

                    if (Vertexes[i].Value < adj.Value)
                    {
                        write("e" + (c++) + " ", tw);
                        write($"{Vertexes[i].Value} {adj.Value}{endL}", tw);
                    }                                        
                }
                //write(endL, tw);
            }

            if (tw != null)
            {
                tw.Flush();
                tw.Dispose();
            }

        }


        public void PrintAdjacencyMatrix(string filename = null)
        {
            Action<string, TextWriter> write;
            TextWriter tw = null;
            if (!String.IsNullOrEmpty(filename))
            {
                tw = new StreamWriter(filename);
                write = new Action<string, TextWriter>(WriteToFile);
            }
            else write = new Action<string, TextWriter>(WriteToConsole);

            write("  ", tw);
            for (int i = 1; i <= Vertexes.Length; i++)
                write("v" + i + " ", tw);
            for (int i = 0; i < Vertexes.Length; i++)
            {
                write(endL, tw);
                write("v" + (i + 1) + " ", tw);
                for (int j = 0; j < Vertexes.Length; j++)
                {
                    int exists = Convert.ToInt32(Vertexes[i].Adjacent.Find(x => x.Value == j + 1) != null);
                    write(exists.ToString()+"  ", tw);
                }
            }
            if (tw != null)
            {
                tw.Flush();
                tw.Dispose();
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
        public void PrintDegreeEachVertex(bool toFile = false)
        {

            if (IsKComplete())
                Console.WriteLine($"graph is k-complete: {Vertexes[0].Adjacent.Count}");
            foreach (var i in Vertexes)
            {
                Console.WriteLine($"v{i.Value} has degree: {i.Degree}");
            }
        }
        public void PrintIsolatedVertexes()
        {
            foreach (var v in Vertexes.Where(x => x.Degree == 0))
                Console.WriteLine(v.Value);
        }
        public void PrintFloatingVertexes()
        {
            foreach (var v in Vertexes.Where(x => x.Degree == 1))
                Console.WriteLine(v.Value);
        }
    }
}

