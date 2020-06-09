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
        public Vertex[] Vertecies { get; set; }
        public Graph()
        {
            Vertecies = new Vertex[10];
        }
        public Graph(string fileName, bool isDirected)
        {
            LoadGraph(fileName,isDirected);
        }
        public void LoadUndirGraph(string fileName)
        {
            LoadGraph(fileName, false);
        }
        public void LoadDirectGraph(string fileName)
        {
            LoadGraph(fileName, true);
        }
        void LoadGraph(string fileName, bool isDir)
        {
            var list = File.ReadAllLines(fileName);

            var info = list[0].Split(' ');
            int count = Convert.ToInt32(info[0]);
            edgesCount = Convert.ToInt32(info[1]);

            Vertecies = new Vertex[count];

            foreach (var line in list.Skip(1))
            {
                var edge = line.Split(' ');
                int value = Convert.ToInt32(edge[0]);
                int endPoint = -1;
                if (edge.Length > 1)
                    endPoint = Convert.ToInt32(edge[1]);
                if (Vertecies[value - 1] == null)
                    Vertecies[value - 1] = new Vertex() { Value = value };
                if (endPoint > 0)
                {
                    var end = Vertecies[endPoint-1];
                    if (end == null)
                    {
                        end = new Vertex();
                        end.Value = endPoint;
                        Vertecies[endPoint - 1] = end;
                    }
                    Vertecies[value - 1].Adjacent.Add(end);
                    if(!isDir)
                    Vertecies[endPoint - 1].Adjacent.Add(Vertecies[value - 1]);
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

           
            for (int i = 0,c=1; i < Vertecies.Length; i++)
            {
                foreach (var adj in Vertecies[i].Adjacent)
                {

                    if (Vertecies[i].Value < adj.Value)
                    {
                        write("e" + (c++) + " ", tw);
                        write($"{Vertecies[i].Value} {adj.Value}{endL}", tw);
                    }                                        
                }
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
            for (int i = 1; i <= Vertecies.Length; i++)
                write("v" + i + " ", tw);
            for (int i = 0; i < Vertecies.Length; i++)
            {
                write(endL, tw);
                write("v" + (i + 1) + " ", tw);
                for (int j = 0; j < Vertecies.Length; j++)
                {
                    int exists = Convert.ToInt32(Vertecies[i].Adjacent.Find(x => x.Value == j + 1) != null);
                    write(exists.ToString()+"  ", tw);
                }
            }
            if (tw != null)
            {
                tw.Flush();
                tw.Dispose();
            }
        }
        public int IsKComplete()
        {
            int k = Vertecies[0].Value;
            bool isKComplete = true;
            foreach (var i in Vertecies)
            {
                isKComplete = k == i.Value;
            }
            return isKComplete ? k : -1 ;
        }
        public void PrintDegreeEachVertex(bool toFile = false)
        {
            if (IsKComplete()!=-1)
                Console.WriteLine($"graph is k-complete: {Vertecies[0].Adjacent.Count}");
            foreach (var i in Vertecies)
            {
                Console.WriteLine($"v{i.Value} has degree: {i.Degree}");
            }
        }
        public void PrintIsolatedVertexes()
        {
            foreach (var v in Vertecies.Where(x => x.Degree == 0))
                Console.WriteLine(v.Value);
        }
        public void PrintFloatingVertexes()
        {
            foreach (var v in Vertecies.Where(x => x.Degree == 1))
                Console.WriteLine(v.Value);
        }

        public void DepthFirstSearch(int start,int sValue)
        { 
            //out current vertex
            //output dfs number
            //output stack  
        }
        public int BreadthFirstSearch(int start,int sValue)
        {          
            int[] lvl = new int[Vertecies.Length];
            for (int j = 0; j < lvl.Length; j++)
            {
                lvl[j] = -1;
            }
            Queue<Vertex> frontier = new Queue<Vertex>();
            // invariant: queue items are always a frontier
            frontier.Enqueue(Vertecies[start-1]);
            int i = 1;
            lvl[Vertecies[start-1].Value-1] = 0;
            OutputSearch(lvl, frontier, Vertecies[start-1]);
            while (frontier.Count > 0)
            {                
                List<Vertex> next = new List<Vertex>();
                int len = frontier.Count;
                for(int j=0;j<len;  j++)
                    foreach (var n in frontier.Dequeue().Adjacent)
                    {
                        if (lvl[n.Value-1] == -1)
                        {
                            lvl[n.Value-1] = i;
                            frontier.Enqueue(n);
                            OutputSearch(lvl, frontier,n);
                        }
                    }                
                i++;
            }
            return lvl[sValue-1];
        }        
        void OutputSearch(int[] lvl,Queue<Vertex> frontier , Vertex cur)
        {
            Console.Write($"Vertex {cur.Value} No: {lvl[cur.Value-1]} Queue: ");
            foreach(var i in frontier)
                Console.Write(i.Value+" ");
            Console.WriteLine();
        }


    }
}

