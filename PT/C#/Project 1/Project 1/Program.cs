using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    class Program 
    {   
        static void Main(string[] args)
        {
            string d = "";
            foreach (var str in args)
            {
                d += str;
                d += ' ';
            }

            DirectoryInfo dir = new DirectoryInfo(d);
            StreamWriter sw = new StreamWriter(@"C:\Users\Zaki\Desktop\tmp.txt");
            Print(sw, dir);
            sw.Close();
        }

        public static void Print(FileSystemInfo dir, int indent = 0)
        {
            for (int i = 0; i < indent; i++) Console.Write("    ");
            Console.Write(dir.Name + " ");
            Console.Write(" (" + Convert.ToString(dir.GetSize()) + ") ");
            Console.Write(dir.GetRahs() + "\n");

            if (Directory.Exists(dir.FullName))
            {
                foreach (string child in Directory.GetFiles(dir.FullName))
                {
                    Print(new FileInfo(child), indent + 1);
                }
                foreach (string child in Directory.GetDirectories(dir.FullName))
                {
                    Print(new DirectoryInfo(child), indent + 1);
                }
            }
        }

        public static void Print(StreamWriter sw, FileSystemInfo dir)
        {
            //for (int i = 0; i < indent; i++) Console.Write("    ");
            //Console.Write(dir.Name + " ");
            //Console.Write(" (" + Convert.ToString(dir.GetSize()) + ") ");
            //Console.Write(dir.GetRahs() + "\n");

            
            if (Directory.Exists(dir.FullName))
            {
                sw.Write(Path.GetFileName(dir.FullName) + " ");
                foreach (string child in Directory.GetFiles(dir.FullName))
                {
                    sw.Write(Path.GetFileName(child) + " ");
                }
                foreach (string child in Directory.GetDirectories(dir.FullName))
                {
                    sw.Write(Path.GetFileName(child) + " ");
                }
                sw.Write(" 0\n");
                foreach (string child in Directory.GetFiles(dir.FullName))
                {
                    Print(sw, new FileInfo(child));
                }
                foreach (string child in Directory.GetDirectories(dir.FullName))
                {
                    Print(sw, new DirectoryInfo(child));
                }
            }
        }
    }
}
