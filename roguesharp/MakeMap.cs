using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    //This class generates the map
    class MakeMap
    {
        
        private static int MAPWIDTH = DrawScreen.MAPWIDTH;
        private static int MAPHEIGHT = DrawScreen.MAPHEIGHT;
        private static int MINSIZE = 5;
        private static int MAXSIZE = 20;
        private static int ROOMMAX = 50;
        private static int ROOMMIN = 10;


        public static void GenerateMap()
        {
            Node.NodeMap();
            Random rng = new Random();
            int roomCount = 0;
            Boolean keepMakingRooms = true;
            while (keepMakingRooms) { 
            int randW = rng.Next(MINSIZE, MAXSIZE);
            int randX = rng.Next(0, MAPWIDTH - randW);
            int randH = rng.Next(MINSIZE, MAXSIZE);
            int randY = rng.Next(0, MAPHEIGHT - randH);
                //This makes sure at least 1 room is always made
                if (roomCount == 0)
                {
                    roomCount++;
                    Room.rooms.Add(new Room(randX, randY, randW, randH, false));
                    Room startroom = (Room)Room.rooms[0];
                    DrawScreen.viewX = startroom.getCenterX();
                    DrawScreen.viewY = startroom.getCenterY();
                }
                else
                {
                    //Checks to see if the generated room intersects with previously made rooms
                    Boolean doesIntersect = false;
                    foreach (Room newrooms in Room.rooms)
                    {
                        if (newrooms.roomsIntersect(randX, randY, randW, randH))
                        {
                            doesIntersect = true;
                        }
                    }
                    //If the new room doesn't intersect with previous rooms, it is created
                    if (!doesIntersect)
                    {
                        roomCount++;
                        Room.rooms.Add(new Room(randX, randY, randW, randH, false));
                    }
                }
                //Random number to determine if it will keep creating rooms
                //If there are too few rooms, it keeps trying to make rooms anyways
                //If there are too many rooms, it stops trying to make rooms
                int random = rng.Next(50);
                if ((random < 1 && roomCount >= ROOMMIN) || roomCount >=ROOMMAX) { keepMakingRooms = false; }
            }

            GenerateHalls();
            //Test code, inserts a player and ally objects into the starting room, and enemy zombies into the other rooms
            foreach(Room room in Room.rooms)
            {
                room.makeDoors();
                if (!room.isHall())
                {  
                    if (Room.rooms.IndexOf(room) == 0)
                    {
                        foreach(Ally ally in Ally.allies)
                        {
                            ally.setX(room.getCenterX()+ally.getX());
                            ally.setY(room.getCenterY()+ally.getY());
                        }
                    }
                    else Enemy.enemies.Add(new Enemy(room.getCenterX(), room.getCenterY(), 'Z', "Zombie", 1));

                }
            }
            Entity.setBlocked();
            Node.NodeMap();
            
        }
        
        //This creates passageways the connect the rooms together
        public static void GenerateHalls()
        {
            int hallMax = Room.rooms.Count - 1;
            Random rng = new Random();
            for (int i = 0; i < hallMax; i++)
            {


                Room tempRoom1 = (Room)Room.rooms[i];
                Room tempRoom2 = (Room)Room.rooms[i+1];
                if (i==hallMax-1)
                {
                    tempRoom1 = (Room)Room.rooms[i+1];
                    tempRoom2 = (Room)Room.rooms[0];
                }
                
                int x1 = rng.Next(tempRoom1.getX()+1, tempRoom1.getWidth()-1);
                int y1 = rng.Next(tempRoom1.getY()+1, tempRoom1.getHeight()-1);
                int x2 = rng.Next(tempRoom2.getX()+1, tempRoom2.getWidth()-1);
                int y2 = rng.Next(tempRoom2.getY()+1, tempRoom2.getHeight()-1);

                if (x1 <= x2)
                {
                    int w = x2 - x1;
                    Room.rooms.Add(new Room(x1, y1, w+2, 2, true));

                    if (y1 <= y2)
                    {
                        int h = y2 - y1;
                        Room.rooms.Add(new Room(x2, y1, 2, h+2, true));
                    }

                    if (y1 > y2)
                    {
                        int h = y1 - y2;
                        Room.rooms.Add(new Room(x2, y2, 2, h+2, true));
                    }
                    
                }
                 if (x1 > x2)
                {
                    int w = x1 - x2;
                    Room.rooms.Add(new Room(x2, y1, w+2, 2, true));
                    if (y1 <= y2)
                    {
                        int h = y2 - y1;
                        Room.rooms.Add(new Room(x2, y1, 2, h+2, true));
                    }

                    if (y1 > y2)
                    {
                        int h = y1 - y2;
                       Room.rooms.Add(new Room(x2, y2, 2, h+2, true));
                    }
                   
                }


                
            }

            }
            
    }
}
