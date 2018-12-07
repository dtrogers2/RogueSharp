using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class NodeComparer
    {
        Node n;
        public NodeComparer(Node n)
        {
            this.n = n;
        }


        public int distanceCompare(Node n1, Node n2)
        {
            double dist1 = (Math.Abs(n.x - n1.x) + Math.Abs(n.y - n1.y));
            double dist2 = (Math.Abs(n.x - n2.x) + Math.Abs(n.y - n2.y));
            return dist1.CompareTo(dist2);
        }
        public int Compare(Node a, Node b)
        {
            double dist1 = (Math.Abs(n.x - a.x) + Math.Abs(n.y - a.y));
            double dist2 = (Math.Abs(n.x - b.x) + Math.Abs(n.y - b.y));
            return dist1.CompareTo(dist2);
        }
    }
}

