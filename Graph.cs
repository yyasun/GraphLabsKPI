using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphLabs
{
    public class Graph
    {
        private bool KComplete;
        public Vertex[] Vertexes {get;set;}
        public Graph()
        {
            Vertexes = new Vertex[10];
        }
        public void LoadGraph(string fileName)
        {
            var list=File.ReadAllLines(fileName);

            var info = list[0].Split(' ')[0];
            int count = Convert.ToInt32(info);
            Vertexes = new Vertex[count];
            
            foreach (var line in list.Skip(1))
            {
                var edge = line.Split(' ');
                int value = Convert.ToInt32(edge[0]);
                int endPoint = -1;
                if (edge.Length>1)
                 endPoint= Convert.ToInt32(edge[1]);
                if (Vertexes[value] == null)
                        Vertexes[value] = new Vertex() { Value =value};
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
        public void IncidentsMatrix(bool toFile=false)
        {
            
        }
        public void AdjacencyMatrix(bool toFile = false)
        { 

        }
        public void DegreeEachVertex(bool toFile = false)
        {
            //if graph is some k complete, only output this k.
        }
        public List<string> IsolatedVertexes()
        { 

        }
        public List<string> FloatingVertexes()
        { 
            
        }
    }
}
