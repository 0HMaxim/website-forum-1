using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD_PROJ
{
    class SupportFunctions
    {
        public bool Enter(string copyOrMove = "Copy")
        {
            Console.Clear();
            if ((Move.path.GetLength(0) == 0) || (Directory.GetDirectories(Move.path[Move.column]).Length +
                Directory.GetFiles(Move.path[Move.column]).Length) == 0)
            {
                string newPath = Move.path[Move.column];
                int x = Console.WindowWidth / 2 - $"You are here -> {Move.path}".Length / 2;
                Move.column = 0;
                Move.path = new string[1];
                Move.path[0] = newPath;
                var key = ConsoleKey.A;
                Dictionary<ConsoleKey, Func<bool>> someMove = new Dictionary<ConsoleKey, Func<bool>>();
                someMove.Add(ConsoleKey.Escape, () =>
                {
                    Move.path[0] += "\\1";
                    Move.key.Escape();
                    return false;
                });
                someMove.Add(ConsoleKey.S, () =>
                {
                    Move.path[0] += "\\1";
                    return true;
                });
                while (!someMove.ContainsKey(key))
                {
                    Console.Clear();
                    Console.WriteLine($"You are here -> {Move.path[Move.column]}");
                    Console.SetCursorPosition(x, 2);
                    Console.WriteLine("FILE IS EMPTY");
                    Console.WriteLine($"Ecs. - Back \nS - {copyOrMove} here");
                    key = Console.ReadKey().Key;
                    if (someMove.ContainsKey(key) && someMove[key]() == true)
                        return true;
                    else
                        Console.WriteLine("Wrong Choise");
                }
                return false;
            }
            else
            {
                int count = Directory.GetDirectories(Move.path[Move.column]).Length + Directory.GetFiles(Move.path[Move.column]).Length;
                string[] directories = Directory.GetDirectories(Move.path[Move.column]);
                string[] folders = Directory.GetFiles(Move.path[Move.column]);
                Move.path = new string[count];
                int i = 0;
                foreach (var item in directories)
                    Move.path[i++] = item;
                foreach (var item in folders)
                    Move.path[i++] = item;
                return false;
            }

        }
        public void WriteTextToCentre(string text)
        {
            int x = Console.WindowWidth / 2 - text.Length / 2;
            int y = Console.WindowHeight / 2 - 1 / 2;
            Console.SetCursorPosition(x, y);
            Console.WriteLine(text);
            y = Console.WindowHeight;
            Console.WriteLine("Press ant key to continue");
            Console.SetCursorPosition(x, 1);
            Console.ReadKey();
        }
        public void UpdateDirectory(string sourse1)
        {
            int count = Directory.GetDirectories(sourse1).Length + Directory.GetFiles(sourse1).Length;
            string[] directories = Directory.GetDirectories(sourse1);
            string[] folders = Directory.GetFiles(sourse1);
            Move.path = new string[count];
            int i = 0;
            foreach (var item in directories)
                Move.path[i++] = item;
            foreach (var item in folders)
                Move.path[i++] = item;
            Move.column = 0;
        }
        public string ChooseWhereToCopyOrMove(string sourceFile, string copyOrMove = "Copy")
        {
            var key = ConsoleKey.P;
            Dictionary<ConsoleKey, Action> move = new Dictionary<ConsoleKey, Action>();
            move.Add(ConsoleKey.Escape, Move.key.Escape);
            move.Add(ConsoleKey.DownArrow, Move.key.DownArrow);
            move.Add(ConsoleKey.UpArrow, Move.key.UpArrow);
            Console.Clear();
            while (key != ConsoleKey.F)
            {
                Console.WriteLine($"\t{sourceFile}\t\t{copyOrMove} to -> Enter way (Press 'F' for {copyOrMove})\t\n\tQ - Quit\n\n");
                Move.ShowDirectory();
                key = Console.ReadKey().Key;
                //////////////      Another dictionary    /////////////////
               if (move.ContainsKey(key))
                {
                    Console.Clear();
                    move[key]();
                }
                else if (key == ConsoleKey.Q)
                {
                    Console.Clear();
                    return "";
                }
                else if (key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    if (Enter(copyOrMove) == true)
                        return GetParent(Move.path[Move.column]);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong choice");
                }
            }
            Console.Clear();
            return GetParent(Move.path[Move.column]);
        }
        public void Rename(FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + newName);
        }
        public string[] SelectFileForRename()
        {
            string[] files = new string[0];
            var key = ConsoleKey.A;
            Move.column = 0;
            Console.Clear();
            Dictionary<ConsoleKey, Func<string[]>> selectFile = new Dictionary<ConsoleKey, Func<string[]>>();
            Dictionary<ConsoleKey, Action> moveByFiles = new Dictionary<ConsoleKey, Action>();
            selectFile.Add(ConsoleKey.F, () =>
            {
                return files;
            });
            selectFile.Add(ConsoleKey.Q, () =>
            {
                return new string[0];
            });
            moveByFiles.Add(ConsoleKey.DownArrow, Move.key.DownArrow);
            moveByFiles.Add(ConsoleKey.UpArrow, Move.key.UpArrow);
            moveByFiles.Add(ConsoleKey.Enter, Move.key.Enter);
            moveByFiles.Add(ConsoleKey.Escape, Move.key.Escape);
            moveByFiles.Add(ConsoleKey.Spacebar, () =>
            {
                string file = Move.path[Move.column];
                Array.Resize(ref files, files.Length + 1);
                int i = files.Length - 1;
                bool alredyThere = false;
                foreach (var item in files)
                {
                    if (file == item)
                    {
                        int indexEl = Array.IndexOf(files, item);
                        Array.Clear(files, indexEl, 1);
                        alredyThere = true;
                        break;
                    }
                }
                if (!alredyThere)
                    files[i] = Move.path[Move.column];
                else
                    files = files.Where(x => x != null).ToArray();
            });
            while (!selectFile.ContainsKey(key))
            {
                Console.WriteLine("\tPress \"SPACE\" for select file (Press 'F' for finish)\n\tQ - Quit\n");
                Move.ShowDirectory();
                Console.WriteLine();
                Console.WriteLine("FILES FOR RENAME");
                foreach (var item in files)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(item);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                key = Console.ReadKey().Key;
                if (moveByFiles.ContainsKey(key)|| selectFile.ContainsKey(key))
                {
                    Console.Clear();
                    if (moveByFiles.ContainsKey(key))
                        moveByFiles[key].Invoke();
                    else 
                        selectFile[key].Invoke();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong choice");
                }
            }
            return files;
        }
        public string GetParent(string path, int createOrMove = 0)
        {
            DirectoryInfo parent = Directory.GetParent(path);
            if (createOrMove != 0)
                parent = Directory.GetParent(parent.ToString());
            else if (parent == null)
                return "";
            return parent.ToString();
        }
        public string[] SearchFile(string patch, string pattern)
        {
            string[] ReultSearch = Directory.GetFiles(patch, pattern, SearchOption.AllDirectories);
            return ReultSearch;
        }
        public string[] SearchDirectory(string patch)
        {
            string[] ReultSearch = Directory.GetDirectories(patch);
            return ReultSearch;
        }
        public void ToLoGicalDrives()
        {
            int i = 0;
            Move.path = new string[Directory.GetLogicalDrives().Length];
            string[] copyTo = new string[Directory.GetLogicalDrives().Length];
            foreach (var item in Directory.GetLogicalDrives())
                Move.path[i++] = item;
        }
    }
}
