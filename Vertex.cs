using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLabs
{
    public class Vertex
    {
        public int Value { get; set; }
        public int Degree => Adjacent.Count;
        private List<Vertex> _adjacent;
        public List<Vertex> Adjacent { get
            {
                if (_adjacent == null)
                {
                    _adjacent = new List<Vertex>();
                }
                return _adjacent;
            } 
            set { _adjacent = value; }
        }
        public int Weight { get; set; }
    }
}
