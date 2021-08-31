using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMD_PROJ
{

    public class Keys
    {
        SupportFunctions sf = new SupportFunctions();
        public void Enter()
        {
            Console.Clear();
            if (File.Exists(Move.path[Move.column]))
                Process.Start(Move.path[Move.column]);
            else if ((Move.path.GetLength(0) == 0) || (Directory.GetDirectories(Move.path[Move.column]).Length + Directory.GetFiles(Move.path[Move.column]).Length) == 0)
            {
                string newPath = Move.path[Move.column];
                int x = Console.WindowWidth / 2 - $"You are here -> {Move.path}".Length / 2;
                Move.column = 0;
                Move.path = new string[1];
                Move.path[0] = newPath;
                var key = ConsoleKey.A;
                Dictionary<ConsoleKey, Action> moveForEmptyFile = new Dictionary<ConsoleKey, Action>();
                moveForEmptyFile.Add(ConsoleKey.Escape, Escape);
                moveForEmptyFile.Add(ConsoleKey.X, X);
                moveForEmptyFile.Add(ConsoleKey.Z, Z);
                Console.Clear();
                while (!moveForEmptyFile.ContainsKey(key))
                {
                    Console.WriteLine($"You are here -> {Move.path[Move.column]}");
                    Console.SetCursorPosition(x, 2);
                    Console.WriteLine("FILE IS EMPTY");
                    Console.WriteLine("Ecs. - Back\nX. - Create .txt File\nZ. - Create directory");
                    key = Console.ReadKey().Key;
                    if (moveForEmptyFile.ContainsKey(key))
                    {
                        Console.Clear();
                        Move.path[0] += "\\1";
                        Move.move[key].Invoke();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong choice");
                    }
                }
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
            }
            Move.column = 0;
        }
        public void S()
        {
            Console.WriteLine("enter file name to search");
            string fileForSearh = Console.ReadLine();
            string[] files = fileForSearh.Split('.');
            string[] findFiles;
            string sourse = sf.GetParent(Move.path[Move.column]);
            if (files.Length > 1)
                findFiles = sf.SearchFile(sourse, fileForSearh);
            else
                findFiles = sf.SearchDirectory(sourse);
            Console.Clear();
            foreach (var item in findFiles)
                Console.WriteLine(item);
            Console.WriteLine("Enter any key for continue");
            Console.ReadKey();
        }

        public void Escape()
        {
            if (Move.path.GetLength(0) != 0)
            {
                string[] sourse = Move.path[Move.column].Split('\\');
                if (sourse.Length > 2)
                {
                    string sourse1 = sf.GetParent(Move.path[Move.column], 1);
                    sf.UpdateDirectory(sourse1);
                }
                else
                    sf.ToLoGicalDrives();
            }
            else
                sf.ToLoGicalDrives();
            Move.column = 0;
        }
        public void F2()
        {
            string[] files = sf.SelectFileForRename();
            if (files.Length != 0)
            {
                Console.WriteLine("Enter new name for THIS FILE(S)");
                string newName = Console.ReadLine();
                Console.WriteLine("ENTER file extension");
                string extension = Console.ReadLine();
                int i = 0;
                if (files.Length == 1)
                {
                    FileInfo fileForRename = new FileInfo(files[0]);
                    sf.Rename(fileForRename, newName + extension);
                }
                else
                {
                    foreach (var item in files)
                    {
                        FileInfo fileForRename = new FileInfo(item);
                        sf.Rename(fileForRename, newName + $" ({i++})" + extension);
                    }
                }
            }
            Move.path[Move.column] = sf.GetParent(Move.path[Move.column]);
            sf.UpdateDirectory(Move.path[Move.column]);
            Move.column = 0;
        }
        public void C()
        {
            string[] sourse = Move.path[Move.column].Split('\\');
            string fileName = sourse[sourse.Length - 1];
            string sourceFile = Move.path[Move.column];
            string targetPath = sf.ChooseWhereToCopyOrMove(sourceFile);
            if (targetPath != "")
            {
                targetPath = Path.Combine(targetPath, fileName);
                if (File.Exists(targetPath))
                {
                    while (File.Exists(targetPath))
                    {
                        Console.WriteLine("This name already using, enter another name for copy");
                        fileName = Console.ReadLine();
                        targetPath = Path.Combine(sf.GetParent(targetPath), fileName);
                    }
                    File.Copy(sourceFile, targetPath, true);
                }
                else if (File.Exists(sourceFile))
                    File.Copy(sourceFile, targetPath, true);
                else if (Directory.Exists(sourceFile))
                {
                    string[] files = Directory.GetFiles(sf.GetParent(targetPath));
                    string destFile = "";
                    Directory.CreateDirectory(sf.GetParent(targetPath));
                    foreach (string s in files)
                    {
                        fileName = Path.GetFileName(s);
                        destFile = Path.Combine(sf.GetParent(targetPath), fileName);
                        File.Copy(s, destFile, true);
                    }
                }
            }
            Move.path[Move.column] = sf.GetParent(Move.path[Move.column]);
            if (Move.path[Move.column] == "")
                sf.ToLoGicalDrives();
            else
                sf.UpdateDirectory(Move.path[Move.column]);
        }
        public void M()
        {
            string[] sourse = Move.path[Move.column].Split('\\');
            string fileName = sourse[sourse.Length - 1];
            string sourceFile = Move.path[Move.column];
            string destinationFile = sf.ChooseWhereToCopyOrMove(sourceFile, "Move");
            if (destinationFile != "" && (File.Exists(sf.GetParent(destinationFile)) || Directory.Exists(sf.GetParent(destinationFile))))
            {
                destinationFile = Path.Combine(destinationFile, fileName);
                if (File.Exists(destinationFile))
                    File.Move(sourceFile, destinationFile);
                else
                    Directory.Move(sourceFile, destinationFile);
            }
            Move.path[Move.column] = sf.GetParent(Move.path[Move.column]);
            if (Move.path[Move.column] == "")
                sf.ToLoGicalDrives();
            else
                sf.UpdateDirectory(Move.path[Move.column]);
        }
        public void X()
        {
            Console.WriteLine("Enter name new text file (without .txt)");

            string targetPath = sf.GetParent(Move.path[Move.column]);
            string fileName = Console.ReadLine();

            targetPath = Path.Combine(targetPath, fileName + ".txt");

            while (File.Exists(targetPath))
            {
                Console.WriteLine("This name already using, enter another name for copy");
                fileName = Console.ReadLine();
                targetPath = Path.Combine(sf.GetParent(targetPath), fileName + ".txt");
            }

            string sourse1 = sf.GetParent(Move.path[Move.column]);
            fileName = Path.Combine(sourse1, fileName + ".txt");
            using (File.Create(fileName)) { }
            sf.UpdateDirectory(sourse1);
        }
        public void A()
        {
            Console.Clear();
            FileAttributes attributes = File.GetAttributes(Move.path[Move.column]);
            int x = Console.WindowWidth / 2 - "File attributes".Length / 2;
            Console.SetCursorPosition(x, 0);
            Console.WriteLine("File attributes");
            Console.WriteLine(attributes);
            Console.WriteLine("Press ant key to continue");
            Console.ReadKey();
        }
        public void V()
        {
            Console.Clear();
            string[] fileName = Move.path[Move.column].Split('\\');
            string[] txt = fileName[fileName.Length - 1].Split('.');
            if (txt[txt.Length - 1] == "txt")
            {
                using (FileStream fs = new FileStream(Move.path[Move.column], FileMode.Open))
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(1251)))
                    Console.WriteLine(sr.ReadToEnd());
                Console.WriteLine("Press ant key to continue");
                Console.ReadKey();
            }
            else
            {
                sf.WriteTextToCentre("It's FILE not is .TXT");
                Process.Start(Move.path[Move.column]);
            }
        }
        public void D()
        {
            Console.Clear();
            if (File.Exists(Move.path[Move.column]))
                File.Delete(Move.path[Move.column]);
            else if (Directory.Exists(Move.path[Move.column]))
                Directory.Delete(Move.path[Move.column], true);
            int saveColumn = Move.column;
            string sourse = sf.GetParent(Move.path[Move.column]);
            sf.UpdateDirectory(sourse);
            if (Move.path.Length == 0)
            {
                Move.path = new string[1];
                Move.path[0] = sourse;
                Enter();
            }
            Move.column = saveColumn - 1;
            if (Move.column < 0)
                Move.column = 0;
        }
        public void Z()
        {
            string newPath = sf.GetParent(Move.path[Move.column]);
            Console.WriteLine("Enter name Directory");
            string nameDir = Console.ReadLine();
            newPath = Path.Combine(newPath, nameDir);
            if (!Directory.Exists(newPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(newPath);
            }
            string sourse = sf.GetParent(Move.path[Move.column]);
            sf.UpdateDirectory(sourse);
        }
        public void DownArrow()
        {
            int movee = (Move.column == Move.path.Length - 1) ? Move.column = 0 : Move.column++;
        }
        public void UpArrow()
        {
            int movee = (Move.column == 0) ? Move.column = Move.path.Length - 1 : Move.column--;
        }
    }
}
