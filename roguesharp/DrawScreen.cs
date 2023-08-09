using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleLib.ConsoleListener;
using ConsoleLib;

namespace RogueSharp
{
    
    class DrawScreen
    {
        //This sets the parameters for the map to be created as well as how far the user can see in the game
        public static int MAPWIDTH =  100;
        public static int MAPHEIGHT = 50;
        public static int PORTWIDTH = 30;
        public static int PORTHEIGHT = 8;
        public static char[,] mapBuffer = new char[MAPWIDTH, MAPHEIGHT];
        public static char[,] portBuffer = new char[PORTWIDTH*2, PORTHEIGHT*2];
        public static bool[,] blockedMap = new bool[MAPWIDTH, MAPHEIGHT];
        public static int viewX;
        public static int viewY;

        //Draws the walls
        public static void BufferWalls()
        {
            foreach (Wall entity in Wall.walls)
            {
                mapBuffer[entity.getX(), entity.getY()] = entity.getImage();
                if (entity.getSolid()) { blockedMap[entity.getX(), entity.getY()] = true; }
            }
        }

        //Redraws the location of everything on the map
        static void BufferMap()
        {
            Entity.setBlocked();
            for (int y = 0; y < MAPHEIGHT; y++)
            {
                for (int x = 0; x < MAPWIDTH; x++)
                {
                    int intersects = 0;
                    foreach(Room room in Room.rooms)
                    {
                        if (room.roomsIntersect(x, y, 0, 0))
                        {
                            if (room.isHall())
                                intersects = 2;
                            else intersects = 1;
                        }
                       
                    }
                    if (intersects == 1) mapBuffer[x, y] = '.';
                    else if (intersects == 2) mapBuffer[x, y] = '.';
                    else mapBuffer[x, y] = ' ';

                    intersects = 0;

                    blockedMap[x, y] = Entity.blockedPlace[x,y];
                }
            }

            foreach (Wall entity in Wall.walls)
            {
                mapBuffer[entity.getX(), entity.getY()] = entity.getImage();
                if (entity.getSolid()) { blockedMap[entity.getX(), entity.getY()] = true; }
            }
            foreach (Door entity in Door.doors)
            {
                mapBuffer[entity.getX(), entity.getY()] = entity.getImage();
                if (entity.getSolid()) { blockedMap[entity.getX(), entity.getY()] = true; }
            }
            foreach (Creature entity in Creature.creatures)
            {
                mapBuffer[entity.getX(), entity.getY()] = entity.getImage();
                if (entity.getSolid()) { blockedMap[entity.getX(), entity.getY()] = true; }
            }
            foreach (Enemy entity in Enemy.enemies)
            {
                mapBuffer[entity.getX(), entity.getY()] = entity.getImage();
                if (entity.getSolid()) { blockedMap[entity.getX(), entity.getY()] = true; }
            }
            foreach (Ally entity in Ally.allies)
            {
                mapBuffer[entity.getX(), entity.getY()] = entity.getImage();
                if (entity.getSolid()) { blockedMap[entity.getX(), entity.getY()] = true; }
            }
            Ally player = Ally.allies[0];
            mapBuffer[player.getX(), player.getY()] = player.getImage();
            blockedMap[player.getX(), player.getY()] = player.getSolid();


            Node.UpdateMap();
        }

        static void BufferPort(Boolean buffer)
        {
            if (buffer) BufferMap();

            int minX = viewX - PORTWIDTH;
            int minY = viewY - PORTHEIGHT;
            int maxX = viewX + PORTWIDTH;
            int maxY = viewY + PORTHEIGHT;
            
            for (int y = 0; y < PORTHEIGHT * 2; y++)
            {
                for (int x = 0; x < PORTWIDTH * 2; x++)
                {
                    if ((minX + x >= 0 && minX + x < MAPWIDTH && minY + y >= 0 && minY + y < MAPHEIGHT))
                    { portBuffer[x, y] = mapBuffer[x + minX, y + minY]; }
                    else portBuffer[x, y] = ' ';
                    
                }
            }
            if (Controls.turnMode != "move")
            { portBuffer[PORTWIDTH, PORTHEIGHT] = '?'; }
        }
        //The entire game is drawn as one string to make it look cleaner and prevent blinking.
        public static void DrawMap(Boolean buffer)
        {
            Console.CursorVisible = false;
            BufferPort(buffer);
            String drawline = "";
            Console.OutputEncoding = Encoding.Unicode;
            Creature creatureAt = null;
            foreach (Creature thing in Creature.creatures)
            {

                if (thing.getX() == viewX && thing.getY() == viewY)
                {
                    creatureAt = thing;
                }
            }
            for (int y = 0; y < PORTHEIGHT * 2; y++)
            {
                for (int x = 0; x < PORTWIDTH * 2; x++)
                {
                    drawline += portBuffer[x, y];
                }
                if (creatureAt != null)
                {
                    switch (y)
                    {
                        case 0: drawline += "[31m" + creatureAt.curHp + "/" + creatureAt.maxHP + " HP    [0m"; break;
                        case 1: drawline += "STR: " + creatureAt.str + "   "; break;
                        case 2: drawline += "DEX: " + creatureAt.dex + "   "; break;
                        case 3: drawline += "CON: " + creatureAt.con + "   "; break;
                        case 4: drawline += "INT: " + creatureAt.intel + "   "; break;
                        case 5: drawline += "WIS: " + creatureAt.wis + "   "; break;
                        case 6: drawline += "CHA: " + creatureAt.cha + "   "; break;
                        case 7: drawline += ConsoleListener.Clicked; break;
                        //case 7: if (entAt != null) { drawline += entAt.getName() + "     "; } else { drawline += "                 "; } break;
                        default: break;
                    }
                }
                drawline += "\n";
            }
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write(drawline);
            Chatlog.messageLog();
        }
    }
}
