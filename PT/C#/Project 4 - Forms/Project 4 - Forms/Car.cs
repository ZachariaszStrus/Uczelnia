using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_4___Forms
{
    class Car
    {
        public string Model { get; set; }
        
        public Engine Motor { get; set; }
        
        public int Year { get; set; }

        public Car(string m, Engine e, int y)
        {
            Model = m;
            Motor = e;
            Year = y;
        }

        public Car()
        {
            Model = "";
            Motor = null;
            Year = 0;
        }

        public override string ToString()
        {
            return Model + " " + Motor.ToString() + " " + Year;
        }
        
    }
}
