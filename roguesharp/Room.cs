using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class Room
    {
        private int x { set; get; }
        private int y { set; get; }
        private int width { set; get; }
        private int height { set; get; }
        private int centX { set; get; }
        private int centY { set; get; }
        private bool hasDoor { set; get; }
        private bool hall { set; get; }
        private bool hasHall { set; get; }
        public static ArrayList rooms = new ArrayList();

        public Room(int x, int y, int width, int height, bool hall)
        {
            this.x = x;
            this.y = y;
            this.width = x + width;
            this.height = y + height;
            this.centX = x+(width/2);
            this.centY = y+(height/2);
            this.hasDoor = false;
            hasHall = false;
            this.hall = hall;
            makeWalls();
        }

        public Boolean roomsIntersect(Room otherRoom)
        {
            return (this.x <= otherRoom.width && this.width >= otherRoom.x && this.y <= otherRoom.height && this.height >= otherRoom.y);
        }

        public Boolean roomsIntersect(int x, int y, int w, int h)
        {
            int width = x + w;
            int height = y + h;
            return (this.x <= width && this.width >= x && this.y <= height && this.height >= y);
        }

        public static bool intersectsRoom(int x, int y)
        {
            foreach (Room room in Room.rooms)
            {
                if (x <= room.width && x >= room.x && y <= room.height && y >= room.y) return true;
            }
            return false;
        }


        //Makes walls around the perimeter of the room, and removes any that intersect with a room
        public void makeWalls()
        {
            for (int ydelet = this.y; ydelet <= this.height; ydelet++)
            {
                for (int xdelet = this.x; xdelet <= this.width; xdelet++)
                {
                    if (!(xdelet==this.x || xdelet == this.width) || !(ydelet == this.y || ydelet == this.height)){
                        for (int index = Wall.walls.Count - 1; index > 0; index--)
                        {

                            Wall tempE = (Wall)Wall.walls[index];
                            if (xdelet == tempE.getX() && ydelet == tempE.getY())
                            {
                                
                                Wall.walls.RemoveAt(index);
                                Entity.entities.Remove(tempE);
                            }

                        }
                    }
                }
            }
            

            for (int y = this.y; y <= this.height; y++)
                {
                    for (int x = this.x; x <= this.width; x++)
                    {

                    
                   if ((y == this.y || x == this.x || x == this.width || y == this.height) && !hall)
                     {
                            Wall.walls.Add(new Wall(x, y, '▒', "Wall", true));
                     }
                   else if ((y == this.y || x == this.x || x == this.width || y == this.height) && hall)
                    {
                        Boolean intersects = false;
                        foreach (Room room in rooms)
                        {
                            
                            if (x <= room.width-1 && x >= room.x+1 && y <= room.height-1 && y >= room.y+1)
                            {
                                intersects = true;

                                /*for (int index = Entity.entities.Count - 1; index > 0; index--)
                                {
                                    if (((x == room.x || x == room.width) && !(y == room.y || y == room.height)) || ((y == room.y || y == room.height) && !(x == room.x || x == room.width)) && x!=this.x && x!=this.width && y!=this.y && y!=this.height)
                                    { 
                                        Entity tempE = (Entity)Entity.entities[index];
                                        if (x == tempE.getX() && y == tempE.getY())
                                        {
                                            Entity.entities.RemoveAt(index);
                                        }
                                    } 
                                }*/
                            }

                            //if (!roomsIntersect(x,y,0,0))
                            //{ Entity.entities.Add(new Entity(x, y, '▒', "Wall")); }
                            
                        }
                        
                                
                        if (!intersects) Wall.walls.Add(new Wall(x, y, '▒', "Wall", true));

                        intersects = false;
                    }


                }
                }
            
        }


        public void makeDoors()
        {
            Entity.setBlocked();
            Random random = new Random();
            for (int y = this.y; y <= this.height; y++)
            {
                int oldRandoor = 0;
                for (int x = this.x; x <= this.width; x++)
                {
                    
                    if (!(x <= 0 || x >= DrawScreen.MAPWIDTH || y <= 0 || y >= DrawScreen.MAPHEIGHT))
                    {
                        if (!isHall())
                        {
                            foreach (Room room in rooms)
                            {
                                if ((x==this.x || y==this.y || x == this.width || y == this.height) && roomsIntersect(room) && room.isHall() && !Entity.blockedPlace[x,y] && !room.hasDoor 
                                    && isPassage(x,y))
                                
                                {
                                    int randoor = random.Next(1, 7);
                                    if (randoor <= 2 && !(randoor == oldRandoor))
                                    {
                                        oldRandoor = randoor;
                                        Door.doors.Add(new Door(x, y, '+', "A Door", true));
                                        hasDoor = true;
                                        room.hasDoor = true;
                                    }
                                    
                                }
                            }
                        }
                    } 
                }
            }
        }

        public bool isPassage(int x, int y)
        {
            return (
                (!Entity.blockedPlace[x + 1, y] && !Entity.blockedPlace[x - 1, y] && Entity.blockedPlace[x, y + 1]
                && Entity.blockedPlace[x, y - 1])
                                    ||
               (!Entity.blockedPlace[x, y + 1] && !Entity.blockedPlace[x, y - 1] && Entity.blockedPlace[x + 1, y]
               && Entity.blockedPlace[x - 1, y])
               );
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getCenterX()
        {
            return centX;
        }

        public int getCenterY()
        {
            return centY;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public bool isHall()
        {
            return hall;
        }
    }

    
}
