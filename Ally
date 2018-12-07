using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class Ally : Creature
    {
        public static List<Ally> allies = new List<Ally>();
        public bool isPlayer {set; get;}
        public Ally(int x, int y, char image, string name, int hd, bool isPlayer, bool isSolid = true) : base(x, y, image, name, hd, isSolid)
        {
            this.x = x;
            this.y = y;
            this.image = image;
            this.name = name;
            this.hd = hd;
            this.isPlayer = isPlayer;
            creatures.Add(this);
            ai = "follow";
        }

        public Ally(int x, int y, char image, string name, int hd, int str, int dex, int con, int intel, int wis, int cha, int hp, bool isSolid = true, string ai = "alive") : base(x,y,image, name,hd,str, dex, con, intel, wis, cha, hp, isSolid, ai)
        {
            this.x = x;
            this.y = y;
            this.image = image;
            this.name = name;
            this.hd = hd;
            this.str = str;
            this.dex = dex;
            this.con = con;
            this.intel = intel;
            this.wis = wis;
            this.cha = cha;
            this.maxHP = hp;
            this.maxHP = hp;
            creatures.Add(this);
            ai = "follow";
        }

        public new void takeTurn()
        {
            if (ai != "dead" && !isPlayer)
            {
                Creature player = Ally.allies[0];
                int eX=0;
                int eY=0;
                Enemy prime = null;
                //This selects the closest enemy out of the list of enemies, so the ally will move towards the closest target
                if (Enemy.enemies != null)
                {
                    prime = Enemy.enemies[0];
                    foreach (Enemy enemy in Enemy.enemies)
                    {
                        if (distanceTo(enemy.getX(), enemy.getY()) < distanceTo(prime.getX(), prime.getY()))
                            prime = enemy;
                    }
                    //Attacks the closest enemy as long as it is not dead
                    //Two checks to make sure the ally doesn't wander too far from the player if the player is active, otherwise it doesn't care
                    //care about the players position
                    if ((distanceTo(prime) < 10 && distanceTo(player) < 10 && prime.ai != "dead") || (distanceTo(prime) < 10 && prime.ai != "dead" && ai == "go"))
                    {
                        eX = prime.getX();
                        eY = prime.getY();
                        prevAI = ai;
                        ai = "attack";
                    }
                }
                //Gets the players X and Y and moves towards the player if there are no enemies nearby
                int tX = player.getX();
                int tY = player.getY();
                if (ai == "follow" && distanceTo(player)> 1)
                { base.moveTo(tX, tY); }
                if (ai == "attack")
                {
                    if (distanceTo(eX, eY) <= 1)
                    {
                        attack(prime);
                    }
                    if ( distanceTo(eX, eY) > 1)
                    {
                        base.moveTo(eX, eY);
                    }
                    ai = prevAI;
                }

                if (ai == "go" && distanceTo(x2, y2) > 1) {
                    base.moveTo(x2, y2);
                }
            }

        }
        //Slightly altered moveTo method
        public new void moveTo(int x, int y)
        {
            //Checks to make sure the place isn't blocked by a solid object before moving there
            if (!Entity.blockedPlace[x, y])
            {
                blockedPlace[x, y] = false;
                this.x = x;
                this.y = y;
                blockedPlace[x, y] = isSolid;
                DrawScreen.viewX = x;
                DrawScreen.viewY = y;
            }
            //If the place is blocked it checks to see what type of thing is blocking its path
            if (Entity.blockedPlace[x, y])
            {
                //If its a door it goes through the list of doors to see which one is blocking its position
                //Once it finds one, it attempts to open it
                foreach (Door door in Door.doors)
                {
                    if (door.getX() == x && door.getY() == y && door.isClosed)
                    {
                        Chatlog.log.Add("You attempt to open the door...");
                        door.openDoor(this);
                    }
                }
                //Goes through the list of enemies to see if one is blocking the position
                //If so, it attacks it
                foreach (Enemy npc in Enemy.enemies)
                {
                    if (npc.getX() == x && npc.getY() == y && npc.ai!="dead")
                    {
                        attack(npc);
                    }
                }
                //If an ally is blocking the position, it swaps places with the ally
                foreach (Ally npc in Ally.allies)
                {
                    if (npc.getX() == x && npc.getY() == y)
                    {
                        npc.x = this.x;
                        npc.y = this.y;
                        this.x = x;
                        this.y = y;
                    }
                }
            }
            
        }
    }
}
