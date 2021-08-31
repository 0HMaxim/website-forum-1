using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD_PROJ
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.SetWindowSize(120, 50);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkBlue;

            Move program = new Move();
            program.Setkeys();
            program.Run();
        }
    }
}