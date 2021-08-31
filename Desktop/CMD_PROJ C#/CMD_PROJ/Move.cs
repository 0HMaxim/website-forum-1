using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD_PROJ
{
    class Move
    {
        public static Dictionary<ConsoleKey, Action> move = new Dictionary<ConsoleKey, Action>();
        public static string[] path = new string[Directory.GetLogicalDrives().Length];
        public static int column = 0;
        public static Keys key = new Keys();
        public Move() { }
        public void Setkeys()
        {
            move.Add(ConsoleKey.UpArrow, key.UpArrow);
            move.Add(ConsoleKey.DownArrow, key.DownArrow);
            move.Add(ConsoleKey.Enter, key.Enter);
            move.Add(ConsoleKey.Escape, key.Escape);
            move.Add(ConsoleKey.F2, key.F2);
            move.Add(ConsoleKey.M, key.M);
            move.Add(ConsoleKey.C, key.C);
            move.Add(ConsoleKey.X, key.X);
            move.Add(ConsoleKey.V, key.V);
            move.Add(ConsoleKey.A, key.A);
            move.Add(ConsoleKey.Z, key.Z);
            move.Add(ConsoleKey.D, key.D);
            move.Add(ConsoleKey.S, key.S);
            move.Add(ConsoleKey.D1, AllCommands.ChekAllComan);
        }
        public void Run()
        {
            var key = ConsoleKey.A;

            Console.Clear();
            int i = 0;
            string[] copyTo = new string[Directory.GetLogicalDrives().Length];
            foreach (var item in Directory.GetLogicalDrives())
                path[i++] = item;

            while (key != ConsoleKey.Q)
            {
                try
                {
                    Console.Clear();
                    ShowDirectory();
                    AllCommands.ShowHotKeys();

                    key = Console.ReadKey().Key;
                    if (move.ContainsKey(key))
                        move[key].Invoke();
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press ant key to continue");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press ant key to continue");
                    Console.ReadKey();
                }
            }

        }
        static public void ShowDirectory()
        {
            for (int j = 0; j < path.Length; j++)
            {
                if (column == j)
                {
                    Console.BackgroundColor= ConsoleColor.DarkRed;
                    Console.WriteLine(path[j]);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.WriteLine(path[j]);
                }
            }
        }
    }
}