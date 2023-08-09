using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace RogueSharp
{
    //The superclass for every object in the game
    //Every object needs a location, whether it is solid, and what character it uses for its image
    class Entity : IComparable
    {
        protected int x { get; set; }
        protected int y { get; set; }
        protected char image { get; set; }
        protected String name { get; set; }
        protected bool isSolid { get; set; }
        public static ArrayList entities = new ArrayList();
        public static Boolean[,] blockedPlace = new Boolean[DrawScreen.MAPWIDTH,DrawScreen.MAPHEIGHT];
        public Entity() { }
        public Entity(int x, int y, char image, String name, bool isSolid = true, string ai = "none")
        {
                this.x = x;
                this.y = y;
                this.image = image;
                this.name = name;
                this.isSolid = isSolid;
                if (isSolid) { setBlocked(x, y); }
        }

            public int getX()
            {
                return x;
            }
            public int getY()
            {
                return y;
            }
            public char getImage()
            {
                return image;
            }
            public String getName()
            {
                return name;
            }
            
            public void setX(int changeX)
            {
                x = changeX;
            }
            public void setY(int changeY)
            {
                y = changeY;
            }
            public void setImage(char changeC)
            {
                image = changeC;
            }
            public bool getSolid()
            {
                return isSolid;
            }


            public virtual Type getType()
            {
                return getType();
            }

        public ArrayList entityList()
        {
            return entities;
        }

         public static void setBlocked()
            {
            for (int y = 0; y < DrawScreen.MAPHEIGHT; y++)
            {
                for (int x = 0; x < DrawScreen.MAPWIDTH; x++)
                {
                    blockedPlace[x, y] = false;
                }
            }

             foreach(Entity thing in entities)
            {
                blockedPlace[thing.x, thing.y] = thing.isSolid;
            }
            }

        public void setBlocked(int x,int y)
        {
            blockedPlace[x, y] = isSolid;
        }


        //Makes sure the place the entity is trying to move to is not blocked
        public void Move(int x, int y)
        {
            if (!blockedPlace[x, y])
            {
                blockedPlace[x, y] = false;
                this.x = x;
                this.y = y;
                blockedPlace[x, y] = isSolid;
            }

            if (blockedPlace[x, y])
            {
                foreach (Door door in Door.doors)
                {
                    if (door.getX() == x && door.getY() == y && door.isClosed)
                    {
                        Chatlog.log.Add(this.name + " attempts to open the door...");
                        door.openDoor((Creature)this);
                    }
                }
            }
        }
        //Uses the moveAstar method to find a path to the target location, then returns that path as a list
        public void moveTo(int x2, int y2)
        {
            Node[] myPath = MoveAstar(x, y, x2, y2);
            if (myPath != null && myPath.Length>1)
            {
                for (int i = 0; i < 1; i++)
                {
                    Move(myPath[i].x, myPath[i].y);
                    DrawScreen.DrawMap(true);


                }
                
            }
        }
        public void moveAll(int x2, int y2)
        {
            Node[] myPath = MoveAstar(x, y, x2, y2);
            if (myPath != null && myPath.Length > 1)
            {
                for (int i = 0; i < myPath.Length-1; i++)
                {
                    Move(myPath[i].x, myPath[i].y);
                    DrawScreen.DrawMap(true);


                }

            }
        }
        //The method to find a path to a location
        public Node[] MoveAstar(int x1, int y1, int x2, int y2, Node initGoal = null)
        {
            Node[,] nodeMap = (Node[,])Node.nodeMap.Clone();
            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();
            Node start = nodeMap[x1, y1];
            Node current = start;
            Node goal = nodeMap[x2, y2];
            initGoal = goal;
            current.gScore = 0;
            current.hScore = start.distanceBetween(goal);
            openSet.Add(current);
            Node sameNode = null;
            //If the starting position is the closest to the goal without having to move, it doesn't want to move
            if (current == goal) { return null; }
            //The openset contains unevaluated paths to the goal
            while (openSet.Count > 0)
            {
                //If the goal position is blocked, it creates a new goal position closest to the original goal position
                if (goal.blocked) goal = Node.getUnblockedGoal(initGoal, start, nodeMap);
                if (current != goal)
                {
                    //This selects the node position in the open set closest to the goal that requires the fewest moves to get there
                    foreach (Node node in openSet)
                    {
                        if ((node.gScore + node.hScore < current.gScore + current.hScore))
                        {
                            current = node;
                        }
                    }
                    //if there are multiple nodes that are roughly equal, it chooses the most recently added on to evaluate
                    if (current == sameNode)
                    {
                        current = openSet[openSet.Count - 1];
                    }
                }
                //If a path is found, it returns the list of moves it takes to get to the goal position
                if (current.equals(goal))
                {
                    Node[] path = new Node[current.gScore + 1];
                    int counter = current.gScore;
                    while (counter > 0)
                    {
                        path[counter - 1] = current;
                        current = current.parent;
                        counter--;
                    }
                    return path;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                //Gets the node positions that are next to the current node
                List<Node> neighbors = Node.getNeighbors(current, nodeMap);
                
                bool changeGoal = false;
                foreach (Node neighbor in neighbors)
                {
                    
                    //keeps the old g score temporarily
                    //checks if the updated gscore is better than the old one
                    if (neighbor.visited)
                    {
                        int oldG = neighbor.getG();
                        neighbor.setG(current);
                        if (oldG + neighbor.hScore > neighbor.gScore + neighbor.hScore)
                        {
                            neighbor.parent = current;
                        }
                        else
                        { neighbor.gScore = oldG; }

                    }
                    //If the neighbor hasn't been visited yet and it's not blocked, then the node is added to the open set
                    if (!closedSet.Contains(neighbor) && (!neighbor.blocked)
                        )
                    {

                        openSet.Add(neighbor);
                        neighbor.setG(current);
                        neighbor.setH(goal);
                        neighbor.parent = current;
                    }

                    neighbor.visited = true;
                }
                if (!changeGoal) sameNode = current;
            }
            return null;
        }

        public double distanceTo(int x2, int y2)
        {
            return Math.Abs(x2 - x) + Math.Abs(y2 - y);
        }

        public double distanceTo(Entity ent)
        {
            return Math.Abs(ent.x - x) + Math.Abs(ent.y - y);
        }

        //Compares two objects by determining the distance between them
        public int CompareTo(object b)
        {
            Entity e2 = (Entity) b;

            if (this.distanceTo(DrawScreen.viewX,DrawScreen.viewY) > e2.distanceTo(DrawScreen.viewX,DrawScreen.viewY)) return 1;
            if (this.distanceTo(DrawScreen.viewX, DrawScreen.viewY) < e2.distanceTo(DrawScreen.viewX, DrawScreen.viewY)) return -1;
            else return 0;
        }

        public int CompareTo(Entity b)
        {
            Entity e2 = b;

            if (this.distanceTo(DrawScreen.viewX,DrawScreen.viewY) > e2.distanceTo(DrawScreen.viewX,DrawScreen.viewY)) return 1;
            if (this.distanceTo(DrawScreen.viewX, DrawScreen.viewY) < e2.distanceTo(DrawScreen.viewX, DrawScreen.viewY)) return -1;
            else return 0;
        }
        

        
    }
    }

