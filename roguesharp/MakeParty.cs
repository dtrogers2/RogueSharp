using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class MakeParty
    {

        public static void enterParty()
        {
           

            bool done = false;
            do
            {
                
                Ally.allies.Add((Ally) makeMember());

                Console.WriteLine("Make a new character?");
                string askDone = Console.ReadLine();
                if (askDone != "y") { done = true; }
            } while (!done);
            Ally.allies[0].isPlayer = true;

            
        }

        public static Ally makeMember()
        {
            string name;
            int str = 0;
            int dex = 0;
            int con = 0;
            int intel = 0;
            int wis = 0;
            int cha = 0;
            int hp = 0;
            Console.WriteLine("Please enter your character name...");
            name = Console.ReadLine();
            Console.WriteLine("Please enter your characters strength score");
            do
            {
                string testString = Console.ReadLine();
                int number = 0;
                int.TryParse(testString, out number);
                str = number;
            } while (str == 0);

            Console.WriteLine("Please enter your characters dexterity score");
            do
            {
                string testString = Console.ReadLine();
                int number = 0;
                int.TryParse(testString, out number);
                dex = number;
            } while (dex == 0);

            Console.WriteLine("Please enter your characters constitution score");
            do
            {
                string testString = Console.ReadLine();
                int number = 0;
                int.TryParse(testString, out number);
                con = number;
            } while (con == 0);

            Console.WriteLine("Please enter your characters intelligence score");
            do
            {
                string testString = Console.ReadLine();
                int number = 0;
                int.TryParse(testString, out number);
                intel = number;
            } while (intel == 0);

            Console.WriteLine("Please enter your characters wisdom score");
            do
            {
                string testString = Console.ReadLine();
                int number = 0;
                int.TryParse(testString, out number);
                wis = number;
            } while (wis == 0);

            Console.WriteLine("Please enter your characters charisma score");
            do
            {
                string testString = Console.ReadLine();
                int number = 0;
                int.TryParse(testString, out number);
                cha = number;
            } while (cha == 0);

            return new Ally(0, 0, '@', name, 1, str, dex,con,intel,wis,cha,hp);
        }
    }
}
