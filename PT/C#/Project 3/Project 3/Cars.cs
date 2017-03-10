using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_3
{
    [XmlRoot("cars")]
    public class Cars : List<Car>
    {
        public Cars()
        {
        }
    }
}
