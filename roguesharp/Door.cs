using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    //A door class was made to make it easier to determine the location of doors
    class Door : Entity
    {

        public static ArrayList doors = new ArrayList();
        public bool isClosed { get; set; }


        public Door(int x, int y, char image, String name, bool isSolid = true)
        {

            this.x = x;
            this.y = y;
            this.image = image;
            this.name = name;
            this.isSolid = isSolid;
            this.isClosed = isSolid;

            entities.Add(this);
        }


        public void openDoor(Creature p)
        {
            int roll = p.rollDice(1, 6);
            if (roll >= 6) { blockedPlace[x, y] = false; this.isSolid = false; this.image = '-'; Chatlog.log.Add(p.getName() + " successfully opens the door!"); isClosed = false; }
            else { Chatlog.log.Add(p.getName() + " failed to open the door..."); }
        }
        override
        public Type getType()
        {
            return getType();
        }
    }
}
