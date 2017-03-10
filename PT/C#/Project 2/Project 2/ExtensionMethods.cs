using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_2
{
    public static class ExtensionMethods
    {
        public static string GetRahs(this FileSystemInfo file)
        {
            string rahs = "";
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
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
    }
}
