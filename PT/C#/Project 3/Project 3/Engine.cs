using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_3
{
    public class Engine
    {
        double displacement;
        double horsePower;
        string model;

        [XmlElement("displacement")]
        public double Displacement
        {
            get
            {
                return displacement;
            }

            set
            {
                displacement = value;
            }
        }

        [XmlElement("horsePower")]
        public double HorsePower
        {
            get
            {
                return horsePower;
            }

            set
            {
                horsePower = value;
            }
        }

        [XmlAttribute("model")]
        public string Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        public Engine(double d, double h, string m)
        {
            Displacement = d;
            HorsePower = h;
            Model = m;
        }

        public Engine()
        {
            Displacement = 0;
            HorsePower = 0;
            Model = "";
        }
    }
}
