using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class Controls
    {
        public static String turnMode { set; get; }
        static ConsoleKey myKey;
        static List<Entity> selectedCreatures;
        static List<Entity> selectedEntities;
        static bool takeTurn = false;

        public static void Control()
        {
            turnMode = "move";
            int x1 = DrawScreen.viewX;
            int y1 = DrawScreen.viewY;
            int x2 = DrawScreen.viewX;
            int y2 = DrawScreen.viewY;

            do
            {
                {
                    myKey = Console.ReadKey().Key;
                    //Depending on which mode you are on, either moves the player object or the center of the viewport
                    if ((myKey == ConsoleKey.UpArrow || myKey == ConsoleKey.LeftArrow
                        || myKey == ConsoleKey.RightArrow || myKey == ConsoleKey.DownArrow) && (turnMode == "look" || turnMode == "selecting"))
                        changeView(myKey);

                    if ((myKey == ConsoleKey.UpArrow || myKey == ConsoleKey.LeftArrow
                        || myKey == ConsoleKey.RightArrow || myKey == ConsoleKey.DownArrow) && turnMode == "move")
                        changeMove(myKey);
                    //Pressing Q exits the game
                    if (myKey == ConsoleKey.Q || myKey == ConsoleKey.Escape) { if (turnMode == "move")turnMode = "quit";  if (turnMode == "look") { turnMode = "move"; DrawScreen.viewX = Ally.allies[0].getX(); DrawScreen.viewY = Ally.allies[0].getY(); } }
                    //This completely resets the game. Clears all the lists, logs, and makes a new map
                    if (myKey == ConsoleKey.R && turnMode == "look")
                    {
                        Room.rooms.Clear();
                        Entity.entities.Clear();
                        Creature.creatures.Clear();
                        Ally.allies.Clear();
                        Enemy.enemies.Clear();
                        Door.doors.Clear();
                        Wall.walls.Clear();
                        Chatlog.log.Clear();
                        MakeMap.GenerateMap();
                        turnMode = "move";
                    }
                    //Changes the game mode so the user can look around
                    if (myKey == ConsoleKey.L) { turnMode = "look"; }
                    //Allows the user to select an area and get the creatures in that area
                    if (myKey == ConsoleKey.S && (turnMode == "look" || turnMode == "selecting"))
                    {


                        if (turnMode == "selecting")
                        {
                            int x3;
                            int y3;
                            x2 = DrawScreen.viewX;
                            y2 = DrawScreen.viewY;
                            if (x2 < x1) { x3 = x1; x1 = x2; x2 = x3; }
                            if (y2 < y1) { y3 = y1; y1 = y2; y2 = y3; }
                            if (selectedCreatures != null) { selectedCreatures.Clear(); }
                            selectedCreatures = selectCreature( x1, y1, x2, y2);
                            turnMode = "selected";
                        }

                        if (turnMode == "look")
                        {
                            x1 = DrawScreen.viewX;
                            y1 = DrawScreen.viewY;
                            turnMode = "selecting";
                        }

                        if (turnMode == "selected") turnMode = "look";

                    }
                    //Forces the seleted creatures to move towards a point
                    if (myKey == ConsoleKey.W && turnMode == "look")
                    {
                        int goalX = DrawScreen.viewX;
                        int goalY = DrawScreen.viewY;
                        Creature.creatures.Sort();
                        if (DrawScreen.viewX >= 0 && DrawScreen.viewX < DrawScreen.MAPWIDTH && DrawScreen.viewY >= 0 && DrawScreen.viewY < DrawScreen.MAPHEIGHT)
                        if (selectedCreatures != null)
                        foreach ( Creature creature in selectedCreatures)
                        {
                            creature.moveTo(goalX,goalY);
                        }
                        
                    }

                    //Sets the goal of each selected creature to the current point so they will move towards it
                    if (myKey == ConsoleKey.A && turnMode == "look" && selectedCreatures.Count > 0)
                    {
                        foreach (Creature item in selectedCreatures)
                        {
                            item.x2 = DrawScreen.viewX;
                            item.y2 = DrawScreen.viewY;
                            if (item.ai != "dead") { 
                            item.prevAI = item.ai;
                            item.ai = "go";
                             }
                        }
                    }
                }
               //Takes a turn, each creature does its thing
                if (myKey == ConsoleKey.Spacebar)
                {
                    takeTurn = true;
                }
                //Makes sure everything that can move takes its turn
                if (takeTurn)
                {
                    foreach (Enemy npc in Enemy.enemies)
                    {
                        npc.takeTurn();
                    }
                    foreach (Ally npc in Ally.allies)
                    {
                        if (!npc.isPlayer)
                        npc.takeTurn();
                    }
                    takeTurn = false;
                }
                //This updates the nodemap (important for pathfinding)
                Node.NodeMap();
                //This updates the blocked map, important for telling the object if a position is blocked
                Entity.setBlocked();
                //Redraws the sreen
                DrawScreen.DrawMap(true);
            } while (turnMode != "quit");
        }

        public static void changeView(ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow)
            {
                //if (!Entity.blockedPlace[DrawScreen.viewX,DrawScreen.viewY-1])
                DrawScreen.viewY -= 1;
            }
            if (key == ConsoleKey.DownArrow)
            {
                //if (!Entity.blockedPlace[DrawScreen.viewX, DrawScreen.viewY + 1])
                DrawScreen.viewY += 1;
            }
            if (key == ConsoleKey.LeftArrow)
            {
                //if (!Entity.blockedPlace[DrawScreen.viewX-1, DrawScreen.viewY])
                DrawScreen.viewX -= 1;
            }
            if (key == ConsoleKey.RightArrow)
            {
                //if (!Entity.blockedPlace[DrawScreen.viewX+1, DrawScreen.viewY])
                DrawScreen.viewX += 1;
            }
        }

        //This is a bunch of code. All it does is check to see if a position is blocked and move there
        public static void changeMove(ConsoleKey key)
        {
            string moveLoc = "no";
            selectCreature(DrawScreen.viewX, DrawScreen.viewY, DrawScreen.viewX, DrawScreen.viewY);
            if (key == ConsoleKey.UpArrow)
            {
                moveLoc = "up";
            }
            if (key == ConsoleKey.DownArrow)
            {
                moveLoc = "down";
            }
            if (key == ConsoleKey.LeftArrow)
            {
                moveLoc = "left";
            }
            if (key == ConsoleKey.RightArrow)
            {
                moveLoc = "right";
            }

            Ally player = Ally.allies[0];
                switch (moveLoc)
                {
                    case "up": player.moveTo(player.getX(), player.getY() - 1); DrawScreen.viewX = player.getX(); DrawScreen.viewY = player.getY(); break;
                    case "down": player.moveTo(player.getX(), player.getY() + 1); DrawScreen.viewX = player.getX(); DrawScreen.viewY = player.getY(); break;
                    case "left": player.moveTo(player.getX()-1, player.getY()); DrawScreen.viewX = player.getX(); DrawScreen.viewY = player.getY(); break;
                    case "right": player.moveTo(player.getX()+1, player.getY()); DrawScreen.viewX = player.getX(); DrawScreen.viewY = player.getY(); break;
                    default: break;
                }
            
            takeTurn = true;
        }
        //Method for selecting creatures with the select mode
        public static List<Entity> selectCreature(int x1,int y1,int x2,int y2)
        {
            List<Entity> retList = new List<Entity>();
            foreach (Entity ent in Entity.entities)
            {
                if (ent is Creature)
                {
                    if (ent.getX() <= x2 && ent.getX() >= x1 && ent.getY() <= y2 && ent.getY() >= y1) retList.Add(ent);
                }
            }

            return retList;
        }
    }

    

}

