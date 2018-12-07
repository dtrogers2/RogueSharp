using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSharp
{
    class Chatlog
    {
        public static List<string> log = new List<string>();
        //Method to print the message log after drawing the screen
        public static void messageLog()
        {
            int maxSize = Console.WindowHeight - DrawScreen.PORTHEIGHT * 2 - 1 ;
            if (log != null)
            {
                //If the length of the message log is shorter than the difference between the screen height and the map
                //It just writes them as normal
                if ( log.Count <= maxSize)
                for (int i = 0; i <= log.Count-1; i++)
                    {
                    Console.WriteLine(log[i]);
                    }
                else
                //If the length of the message log is too large, it only writes the most recent ones.
                for (int i = log.Count-maxSize; i <= log.Count-1; i++)
                    {
                        clearLine(i);
                        Console.WriteLine(log[i]);
                    }
            }
        }

        //Because the way the screen is drawn to, the screen can get messy with previous iterations of the drawn screen
        //This clears the screen with blank spaces so it is cleaner
        public static void clearLine(int i)
        {
            StringBuilder logStr = new StringBuilder();
            logStr.Append(log[i]);
            int charLength = log[i].Length;
            int maxLength = DrawScreen.PORTWIDTH * 2;
            if (charLength < maxLength)
            {
                for (int z = charLength; z < maxLength; z++)
                {
                    logStr.Append(" ");
                }

            }
            log[i] = logStr.ToString();
        }
    }
}
