using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    //Enemy class to make it easier to keep track of enemy creatures
    class Enemy : Creature
    {
        public static List<Enemy> enemies = new List<Enemy>();

        public Enemy(int x, int y, char image, string name, int hd, bool isSolid = true) : base(x, y, image, name, hd, isSolid)
        {
            this.x = x;
            this.y = y;
            this.image = image;
            this.hd = hd;
            creatures.Add(this);
        }

        public new void takeTurn()
        {
            if (ai != "dead")
            {
                Creature target = Ally.allies[0];
                foreach (Ally target2 in Ally.allies)
                {
                    if (distanceTo(target2) < distanceTo(target) && target2.ai!="dead") target = target2;
                }
                
                int tX = target.getX();
                int tY = target.getY();
                if (distanceTo(tX, tY) <= 1 && target.ai !="dead")
                {
                    attack(target);
                }
                if (distanceTo(tX, tY) <= 10 && distanceTo(tX, tY) > 1)
                {
                    moveTo(tX, tY);
                }
                
            }

        }

    }
}
