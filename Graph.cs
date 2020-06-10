using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Formatters;
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
            LoadGraph(fileName, isDirected);
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
                    var end = Vertecies[endPoint - 1];
                    if (end == null)
                    {
                        end = new Vertex();
                        end.Value = endPoint;
                        Vertecies[endPoint - 1] = end;
                    }
                    Vertecies[value - 1].Adjacent.Add(end);
                    if (!isDir)
                        Vertecies[endPoint - 1].Adjacent.Add(Vertecies[value - 1]);
                }
            }
        }
        public void ShowGraph()
        {
            foreach (var i in this.Vertecies)
            {
                foreach (var j in i.Adjacent)
                    Console.WriteLine(i.Value + " " + j.Value);
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

            for (int i = 0, c = 1; i < Vertecies.Length; i++)
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
                    write(exists.ToString() + "  ", tw);
                }
            }
            if (tw != null)
            {
                tw.Flush();
                tw.Dispose();
            }
        }
        public void PrintDegreeEachVertex(bool toFile = false)
        {
            if (IsKComplete() != -1)
                Console.WriteLine($"graph is k-complete: {Vertecies[0].Adjacent.Count}");
            foreach (var i in Vertecies)
            {
                Console.WriteLine($"v{i.Value} has degree: {i.Degree}");
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
            return isKComplete ? k : -1;
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

        public Stack<Vertex> finished;
        public int DepthFirstSearch(int start, int sValue)
        {
            finished = new Stack<Vertex>();
            int[] lvl = InitSearchLvl(Vertecies.Length);
            Stack<Vertex> queue = new Stack<Vertex>();
            foreach (var i in Vertecies)
            {
                if (lvl[i.Value - 1] == -1)
                {
                    lvl[i.Value - 1] = 0;
                    DFSVisit(queue, i, lvl, 1);
                }
            }
            return lvl[sValue - 1] - lvl[start - 1];
        }
        private void DFSVisit(Stack<Vertex> stack, Vertex start, int[] lvl, int curLvl = 1)
        {
            OutputSearch(lvl, stack, start);
            stack.Push(start);
            foreach (var desc in start.Adjacent)
            {
                if (lvl[desc.Value - 1] == -1)
                {
                    lvl[desc.Value - 1] = curLvl;
                    DFSVisit(stack, desc, lvl, curLvl + 1);
                }
            }
            finished.Push(stack.Pop());
        }
        public int BreadthFirstSearch(int start, int sValue)
        {
            int[] lvl = InitSearchLvl(Vertecies.Length);
            Queue<Vertex> frontier = new Queue<Vertex>();
            // invariant: queue items are always a frontier
            frontier.Enqueue(Vertecies[start - 1]);
            int i = 1;
            lvl[Vertecies[start - 1].Value - 1] = 0;
            OutputSearch(lvl, frontier, Vertecies[start - 1]);
            while (frontier.Count > 0)
            {
                List<Vertex> next = new List<Vertex>();
                int len = frontier.Count;
                for (int j = 0; j < len; j++)
                    foreach (var n in frontier.Dequeue().Adjacent)
                    {
                        if (lvl[n.Value - 1] == -1)
                        {
                            lvl[n.Value - 1] = i;
                            frontier.Enqueue(n);
                            OutputSearch(lvl, frontier, n);
                        }
                    }
                i++;
            }
            return lvl[sValue - 1];
        }
        private int[] InitSearchLvl(int len)
        {
            int[] lvl = new int[len];
            for (int j = 0; j < lvl.Length; j++)
            {
                lvl[j] = -1;
            }
            return lvl;
        }
        void OutputSearch(int[] lvl, IEnumerable<Vertex> frontier, Vertex cur)
        {
            Console.Write($"Vertex {cur.Value} No: {lvl[cur.Value-1]} Queue: ");
            foreach (var i in frontier.Reverse())
                Console.Write(i.Value + " ");
            Console.WriteLine();
        }

        public List<Vertex> TopologicalSort()
        {
            DepthFirstSearch(1, 2);
            var lst = finished.ToList();
            return lst;
        }

        public Vertex[] Transpose()
        {
            Vertex[] transposed = new Vertex[Vertecies.Length];
            foreach (var v in Vertecies)
                transposed[v.Value - 1] = new Vertex() { Value = v.Value };
            foreach (var v in Vertecies)
            {
                foreach (var child in v.Adjacent)
                {
                    if (!transposed[child.Value - 1].Adjacent.Contains(transposed[v.Value-1]))
                    {                      
                        transposed[child.Value - 1].Adjacent.Add(transposed[v.Value - 1]);
                    }
                }
            }
            return transposed;
        }

        public int FindStronglyConnectedComponents()
        {
            var finishTimes = TopologicalSort();
            Graph tg = new Graph();
            tg.Vertecies = this.Transpose();

            return tg.DepthFirstSearchSCC(finishTimes);
        }
        private int DepthFirstSearchSCC(List<Vertex> finishTimes)
        {
            Stack<Vertex> queue = new Stack<Vertex>();
            int[] lvl = InitSearchLvl(Vertecies.Length);
            List<Vertex>[] owner = new List<Vertex>[Vertecies.Length];

            foreach (var v in finishTimes)
            {
                if (lvl[v.Value - 1] == -1)
                {
                    lvl[v.Value - 1] = 0;
                    owner[v.Value - 1] = new List<Vertex>();
                    owner[v.Value - 1].Add(Vertecies[v.Value-1]);
                    DFSvisitSCC(owner[v.Value - 1], queue, Vertecies[v.Value-1], lvl);
                }
            }

            Console.WriteLine();
            foreach (var master in owner.Where(x => x != null && x.Count > 1))
            {
                foreach (var v in master)
                {
                    Console.Write(v.Value + " ");
                }
                if (master != null)
                    Console.WriteLine();
            }
            return owner.Where(x => x != null && x.Count > 1).Count();
        }
        private void DFSvisitSCC(List<Vertex> owner, Stack<Vertex> stack, Vertex start, int[] lvl, int curLvl = 1)
        {
            OutputSearch(lvl, stack, start);
            stack.Push(start);
            foreach (var desc in start.Adjacent)
            {
                if (lvl[desc.Value - 1] == -1)
                {                    
                    owner.Add(desc);
                    lvl[desc.Value - 1] = curLvl;
                    DFSvisitSCC(owner, stack, desc, lvl, curLvl + 1);
                }
            }
        }



        public void Dijkstra()
        {

        }
        public void FloydWarshall()
        {

        }

    }
}

