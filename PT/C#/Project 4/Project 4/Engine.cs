using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_4
{
    public class Engine : IComparable
    {
        [XmlElement("displacement")]
        public double Displacement { get; set; }

        [XmlElement("horsePower")]
        public double HorsePower { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

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

        public Engine(string e)
        {
            CreateFromString(e);
        }

        public int CompareTo(object other)
        {
            return Convert.ToInt32(HorsePower - (other as Engine).HorsePower);
        }

        public override string ToString()
        {
            return Model + " " + Displacement.ToString() + " (" + HorsePower.ToString() + ")";
        }
        
        public bool CreateFromString(string engine)
        {
            string[] engineAttr = engine.Split(' ');
            if(engineAttr.Length == 3)
            {
                Model = engineAttr[0].Trim();
                Displacement = Convert.ToDouble(engineAttr[1].Trim());
                string hp = engineAttr[2].Trim().Substring(1, engineAttr[2].Trim().Length - 2);
                HorsePower = Convert.ToDouble(hp);
                return true;
            }
            return false;
        }

        public static Engine Parse(string eString)
        {
            string[] engineAttr = eString.Split(' ');
            Engine engine = new Engine();
            engine.Model = engineAttr[0].Trim();
            engine.Displacement = Convert.ToDouble(engineAttr[1].Trim());
            string hp = engineAttr[2].Trim().Substring(1, engineAttr[2].Trim().Length - 2);
            engine.HorsePower = Convert.ToDouble(hp);
            return engine;
        }
    }
}
