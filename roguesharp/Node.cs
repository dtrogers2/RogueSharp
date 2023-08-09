using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class Node : IComparer
    {
        public int x { get; }
        public int y { get; }
        public int gScore { set; get; }
        public int hScore { set; get; }
        public bool blocked { get; set; }
        public bool visited { set; get; }
        public Node parent { set; get; }
        public static Node[,] nodeMap = new Node[DrawScreen.MAPWIDTH, DrawScreen.MAPHEIGHT];

        //Creates the map, then determines if a position is blocked or not
         public static void NodeMap() {
            for (int y = 0; y < DrawScreen.MAPHEIGHT; y++)
            {
                for (int x = 0; x < DrawScreen.MAPWIDTH; x++)
                {
                    //previously blockedmapped
                    nodeMap[x, y] = new Node(x, y, false,false);
                    
                }
            }
            foreach (Wall wall in Wall.walls)
            {
                nodeMap[wall.getX(), wall.getY()].blocked = wall.getSolid();
            }
        }
        public static void UpdateMap()
        {
            foreach (Wall wall in Wall.walls)
            {
                nodeMap[wall.getX(), wall.getY()].blocked = wall.getSolid();
            }
        }

        public Node(int x, int y, bool blocked, bool visited)
        {
            this.x = x;
            this.y = y;
            this.blocked = blocked;
            this.visited = visited;
            parent = this;
            gScore = hScore = int.MaxValue;
        }

        public bool equals(Node other)
        {
            if ((x == other.x) && (y == other.y)) return true;
            return false;
               
        }

        public int distanceBetween(Node goal)
        {
            return Math.Abs(goal.x - x) + Math.Abs(goal.y - y) ;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public void setG(Node previous)
        {
            gScore = previous.getG()+1;
        }

        public int getG()
        {
            return gScore;
        }

        public void setH(Node goal)
        {
            hScore = distanceBetween(goal);
        }

        public int getH()
        {
            return hScore;
        }
        public static List<Node> getNeighbors(Node center, Node[,] map)
        {
            List<Node> retList = new List<Node>();
            if (center.y - 1 > 0) retList.Add(map[center.x, center.y - 1]);
            if (center.y + 1 < DrawScreen.MAPHEIGHT) retList.Add(map[center.x, center.y + 1]);
            if (center.x - 1 > 0) retList.Add(map[center.x - 1, center.y]);
            if (center.x + 1 < DrawScreen.MAPWIDTH) retList.Add(map[center.x + 1, center.y]);

            return retList;
        }

        /*public static List<Node> getExtendedNeighbors(Node center, Node[,] map)
        {
            List<Node> retList = new List<Node>();
            if (center.y - 1 > 0) retList.Add(map[center.x, center.y - 1]);
            if (center.y + 1 < DrawScreen.MAPHEIGHT) retList.Add(map[center.x, center.y + 1]);
            if (center.x - 1 > 0) retList.Add(map[center.x - 1, center.y]);
            if (center.x + 1 < DrawScreen.MAPWIDTH) retList.Add(map[center.x + 1, center.y]);

            if (center.y - 1 > 0) retList.Add(map[center.x-1, center.y - 1]);
            if (center.y + 1 < DrawScreen.MAPHEIGHT) retList.Add(map[center.x+1, center.y -1]);
            if (center.x - 1 > 0) retList.Add(map[center.x - 1, center.y+1]);
            if (center.x + 1 < DrawScreen.MAPWIDTH) retList.Add(map[center.x + 1, center.y+1]);

            return retList;
        }*/

        public static Node getUnblockedGoal(Node center, Node start, Node[,] map)
        {

            bool found = false;
            int xmin = center.x;
            int xmax = center.x;
            int ymin = center.y;
            int ymax = center.y;
            Node newGoal = center;

            do
            {
                for (int y = ymin; y <= ymax; y++)
                {
                    for (int x = xmin; x <= xmax; x++)
                    {

                        if (!map[x, y].blocked && Room.intersectsRoom(x, y))
                        {

                            if (!found)
                            {
                                newGoal = map[x, y];
                                found = true;
                            }
                            if (found == true)
                            {
                                if ((map[x, y].distanceBetween(center) <= newGoal.distanceBetween(center)) && map[x,y].distanceBetween(start) <= newGoal.distanceBetween(start))
                                {
                                    newGoal = map[x, y];
                                }
                            }
                        }
                    }
                }
                if (xmin - 1 > 0) xmin--;
                if (xmax + 1 < DrawScreen.MAPWIDTH - 1) xmax++;
                if (ymin - 1 > 0) ymin--;
                if (ymax + 1 < DrawScreen.MAPHEIGHT - 1) ymax++;
            } while (found == false);
            return newGoal;
        }
        int IComparer.Compare(object a, object b)
        {
            Node center = (Node)b;
            Node n2 = (Node)a;
            double dist1 = distanceBetween(center);
            double dist2 = n2.distanceBetween(center);
            if (dist1 > dist2) return 1;
            if (dist1 < dist2) return -1;
            else return 0;
        }
    }
}
