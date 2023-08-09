using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleLib.NativeMethods;
using static ConsoleLib.ConsoleListener;
using ConsoleLib;
namespace RogueSharp
{
    class Program
    {

        static void Main(string[] args)
        {
            IntPtr inHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint mode = 0;
            GetConsoleMode(inHandle, ref mode);
            mode &= ~ENABLE_QUICK_EDIT_MODE; //disable
            mode |= ENABLE_WINDOW_INPUT; //enable (if you want)
            mode |= ENABLE_MOUSE_INPUT; //enable
            SetConsoleMode(inHandle, mode);
    
            MakeParty.enterParty();
            Console.Clear();
            MakeMap.GenerateMap();
            DrawScreen.DrawMap(true);
            //ConsoleListener.Start();
            Controls.Control();

        }
    }
}

            
