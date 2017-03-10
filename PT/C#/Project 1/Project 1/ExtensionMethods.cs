using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    public static class ExtensionMethods
    {
        public static string GetRahs(this FileSystemInfo file)
        {
            string rahs = "";
            if((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                rahs += "r";
            }
            else
            {
                rahs += "-";
            }

            if ((file.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                rahs += "a";
            }
            else
            {
                rahs += "-";
            }

            if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                rahs += "h";
            }
            else
            {
                rahs += "-";
            }

            if ((file.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                rahs += "s";
            }
            else
            {
                rahs += "-";
            }


            return rahs;
        }

        public static long GetSize(this FileSystemInfo file)
        {
            if (Directory.Exists(file.FullName))
            {
                return Directory.GetFiles(file.FullName).Length;
            }
            else
            {
                return ((FileInfo)file).Length;
            }
        }

        public static DateTime GetOldestFile(this DirectoryInfo dir, DateTime oldestModification)
        {
            foreach (string child in Directory.GetFiles(dir.FullName))
            {
                FileInfo childFile = new FileInfo(child);
                oldestModification = childFile.LastWriteTime < oldestModification ?
                    childFile.LastWriteTime : oldestModification;
            }
            foreach (string child in Directory.GetDirectories(dir.FullName))
            {
                DirectoryInfo childDir = new DirectoryInfo(child);
                oldestModification = childDir.GetOldestFile(oldestModification);
            }

            return oldestModification;
        }
    }
}
