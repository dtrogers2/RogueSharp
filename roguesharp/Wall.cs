using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class Wall : Entity
    {

        public static ArrayList walls = new ArrayList();



        public Wall(int x, int y, char image, String name, bool isSolid = true)
        {

            this.x = x;
            this.y = y;
            this.image = image;
            this.name = name;
            this.isSolid = isSolid;
            entities.Add(this);
        }

        override
        public Type getType()
        {
            return getType();
        }
    }
}
