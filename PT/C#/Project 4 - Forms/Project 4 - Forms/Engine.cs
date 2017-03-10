using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_4___Forms
{
    public class Engine : IComparable
    {
        public double Displacement { get; set; }
        
        public double HorsePower { get; set; }
        
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

        public int CompareTo(object other)
        {
            return Convert.ToInt32(HorsePower - (other as Engine).HorsePower);
        }

        public override string ToString()
        {
            return Model + " " + Displacement.ToString() + " (" + HorsePower.ToString() + ")";
        }

        public static Engine Parse(string eString)
        {
            string[] engineAttr = eString.Split(' ');
            if(engineAttr.Length == 3)
            {
                try
                {
                    Engine engine = new Engine();
                    engine.Model = engineAttr[0].Trim();
                    engine.Displacement = Convert.ToDouble(engineAttr[1].Trim());
                    string hp = engineAttr[2].Trim().Substring(1, engineAttr[2].Trim().Length - 2);
                    engine.HorsePower = Convert.ToDouble(hp);
                    return engine;
                }
                catch (Exception)
                {
                    return new Engine();
                }
            }
            else
            {
                return new Engine();
            }
        }
    }
}
