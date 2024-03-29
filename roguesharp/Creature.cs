using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    //Creatures are a subtype of Entity
    //Creatures have stats, which allows them to attack, defend, and determines how durable they are
    class Creature : Entity
    {
        public static ArrayList creatures = new ArrayList();
        public List<Creature> friendlies { get; }
        public List<Creature> hostiles { get; }
        string AI;
        //Coordinate destination;
        Random rng = new Random();
        public int hd { set; get; }
        public int str { get; set; }
        public int dex { get; set; }
        public int con { get; set; }
        public int intel { get; set; }
        public int wis { get; set; }
        public int cha { get; set; }

        public int maxHP {get ; set;}
        public int curHp { get;  set;}

        public string ai { get; set; }
        public string prevAI { get; set; }

        public int x2 { get; set; }
        public int y2 { get; set; }


        public Creature(int x,int y, char image, String name,int hd, bool isSolid = true,  String ai = "alive")
        {
            
            this.x = x;
            this.y = y;
            x2 = x;
            y2 = y;
            this.image = image;
            this.name = name;
            this.isSolid = isSolid;
            //destination = new Coordinate(x, y);
            entities.Add(this);
            this.str = rollDice(3, 6);
            this.dex = rollDice(3, 6);
            this.con = rollDice(3, 6);
            this.intel = rollDice(3, 6);
            this.wis = rollDice(3, 6);
            this.cha = rollDice(3, 6);
            this.hd = 1;
            if (con > 10) this.maxHP = rollDice(hd, 8) + (con / 2 - 5);
            else this.maxHP = rollDice(hd, 8);
            curHp = maxHP;
            this.ai = ai;
            prevAI = ai;
            friendlies = new List<Creature>();
            hostiles = new List<Creature>();

        }

        public Creature(int x, int y, char image, string name, int hd, int str, int dex, int con, int intel, int wis, int cha, int hp, bool isSolid = true, string ai = "alive")
        {

            this.x = x;
            this.y = y;
            x2 = x;
            y2 = y;
            this.image = image;
            this.name = name;
            this.isSolid = isSolid;
           // destination = new Coordinate(x, y);
            entities.Add(this);
            this.str = str;
            this.dex = dex;
            this.con = con;
            this.intel = intel;
            this.wis = wis;
            this.cha = cha;
            this.hd = 1;
            this.maxHP = hp;
            curHp = maxHP;
            this.ai = ai;
            prevAI = ai;
            friendlies = new List<Creature>();
            hostiles = new List<Creature>();

        }

        public void takeTurn()
        {
            if (ai != "dead")
            {
                Creature player = Ally.allies[0];
                int tX = player.getX();
                int tY = player.getY();
                if (distanceTo(tX, tY) <= 10)
                {
                    moveTo(tX, tY);
                }
            }

        }
        
        override
        public Type getType()
        {
            return getType();
        }

        public void attack(Creature other)
        {
            Chatlog.log.Add(this.name + " swipes at " + other.name + "!");
            int otherAC = 10 + (other.dex/2-5);
            int myAttack = ((str / 2) - 5) + rollDice(1, 20);
            int myDamage = ((str / 2) - 5) + rollDice(1, 6);
            if (myAttack >= otherAC)
            {
                if (myDamage > 0)
                {
                    Chatlog.log.Add(this.name + " hits " + other.name + " for " + myDamage + " damage!");
                    other.takeDamage(myDamage);
                }
                else Chatlog.log.Add(this.name + " hits " + other.name + " but failed to do any damage!");
            }

        }

        public void takeDamage(int damage)
        {
            curHp -= damage;
            if (curHp <= 0)
            {
                Chatlog.log.Add(this.name + " dies!");
                isSolid = false;
                image = '%';
                name = name + " corpse";
                setBlocked(x, y);
                ai = "dead";
            }
        }

        public int rollDice(int quantity, int size)
        {
            int tempN =0;
            for (int i = 0; i < quantity; i++)
            {
                tempN = tempN + rng.Next(1, size + 1);
            }
            return tempN;
        }

        //Rolls an entities morale
        public void getMorale(List<Creature> creatures)
        {
            Random diceRoll = new Random();
            int roll1 = diceRoll.Next(1, 7);
            int roll2 = diceRoll.Next(1, 7);
            int moraleRoll = roll1 + roll2;
        }

        //Rolls an entity reaction
        public void getReaction(List<Creature> creatures)
        {
            Random diceRoll = new Random();
            int roll1 = diceRoll.Next(1, 7);
            int roll2 = diceRoll.Next(1, 7);
            int reactionRoll = roll1 + roll2;
        }
    }
}
