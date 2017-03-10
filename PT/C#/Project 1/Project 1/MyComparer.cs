using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    [Serializable]
    class MyComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if(x.Length != y.Length)
            {
                return x.Length - y.Length;
            }
            else
            {
                return x.CompareTo(y);
            }
        }
    }
}
