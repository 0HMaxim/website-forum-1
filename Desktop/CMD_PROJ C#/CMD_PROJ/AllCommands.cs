using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CMD_PROJ
{
    static class AllCommands
    {
        public static void ChekAllComan()
        {
            Console.Clear();
            int x = Console.WindowWidth / 2 - "\t╔═════════════╗".Length / 2;
            int y = Console.WindowHeight - 30;
            Console.SetCursorPosition(x, y);
            Console.WriteLine();
            Console.WriteLine("\t╔═════════════════════════════════╦═══════════ CONTROL ═════════╦═════════════════════════════════╗");
            Console.WriteLine("\t╠════ F2 - Rename file(s)         ║ Enter - Go to the directory ╠════ Z - Create Directory        ║");
            Console.WriteLine("\t╠════ C  - Copy file              ║ UpArrow(↑)  - file above    ╠════ X - Create .txt file        ║");
            Console.WriteLine("\t╠════ A  - Show file attributes   ╠═════════════════════════════╬════ M - Move file               ║");
            Console.WriteLine("\t╠════ S  - Search file            ║ DownArrow(↓) - File below   ╠════ V - View .txt file          ║");
            Console.WriteLine("\t╠════ D  - Delete file            ║ Escape - Exit directory back╠════ Q - Quit                    ║");
            Console.WriteLine("\t╚═════════════════════════════════╩═════════════════════════════╩═════════════════════════════════╝");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        public static void ShowHotKeys()
        {
            Console.WriteLine();
            Console.WriteLine("\t╔═════════════════════════════════╦═════════════════════════════╦═════════════════════════════════╗");
            Console.WriteLine("\t╠═════════════════════════════════╬══════════ CONTROL ══════════╬═════════════════════════════════╣");
            Console.WriteLine("\t╠══ Escape - Exit directory back ═╣                             ╠══ Enter - Go to the directory ══╣");
            Console.WriteLine("\t╠═════════════════════════════════╬══ UpArrow(↑)  - file above ═╬═════════════════════════════════╣");
            Console.WriteLine("\t║                                 ╠═════════════════════════════╣                                 ║");
            Console.WriteLine("\t╠══ Q - Quit                      ╠═ DownArrow(↓) - File below ═╬══ 1 - Show all commands         ║");
            Console.WriteLine("\t╚═════════════════════════════════╩═════════════════════════════╩═════════════════════════════════╝");
        }
    }
}
