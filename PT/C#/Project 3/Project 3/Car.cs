using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project_3
{
    [XmlType("car")]
    public class Car
    {
        string model;
        int year;
        Engine motor;

        [XmlElement("model")]
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

        [XmlElement("year")]
        public int Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
            }
        }

        [XmlElement("engine")]
        public Engine Motor
        {
            get
            {
                return motor;
            }

            set
            {
                motor = value;
            }
        }

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
    }
}
