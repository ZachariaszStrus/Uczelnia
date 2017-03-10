using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Project_3
{
    public class Program
    {
        Cars myCars;

        public Cars MyCars
        {
            get
            {
                return myCars;
            }

            set
            {
                myCars = value;
            }
        }

        public Program()
        {
            MyCars = new Cars(){
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };
        }

        public void Task1()
        {
            Console.WriteLine("\nTask 1 ---------------------------------------------\n");
            var query1 = from car in MyCars
                          select new { engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol",
                              hppl = car.Motor.HorsePower / car.Motor.Displacement };


            foreach (var motor in query1)
            {
                Console.WriteLine(motor.engineType + " : " + motor.hppl);
            }
            Console.WriteLine();

            var query2 = from motor in query1
                         group motor
                            by motor.engineType into motorType
                         select new
                         {
                             engineType = motorType.Key,
                             avgHPPL = motorType.Average(m => m.hppl)
                         };

            foreach (var motorType in query2)
            {
                Console.WriteLine(motorType.engineType + " : "+motorType.avgHPPL);
            }
        }

        public void Task2()
        {
            Console.WriteLine("\nTask 2 ---------------------------------------------\n");
            XmlSerializer ser = new XmlSerializer(typeof(Cars));
            FileStream fs = new FileStream("CarsCollection.xml", FileMode.Create);
            ser.Serialize(fs, MyCars);
            fs.Close();

            XmlSerializer dser = new XmlSerializer(typeof(Cars));
            FileStream ifs = new FileStream("CarsCollection.xml", FileMode.Open);
            Cars newCollection = ser.Deserialize(ifs) as Cars;
            ifs.Close();

            Console.WriteLine("Deserialized collection : ");
            foreach (var i in newCollection)
            {
                Console.WriteLine(i.Model + " " + i.Year + " " + i.Motor.Model);
            }
        }

        public void Task3()
        {
            Console.WriteLine("\nTask 3 ---------------------------------------------\n");
            
            XElement rootNode = XElement.Load("CarsCollection.xml");
            var avgHP = rootNode.XPathEvaluate("sum(/car/engine[@model != \"TDI\"]/horsePower) "+
                "div count(/car/engine[@model != \"TDI\"])");
            Console.WriteLine("Average HP : " + avgHP);

            IEnumerable<XElement> models = rootNode.XPathSelectElements("/car/model[not(text() = preceding::*/text())]");
            Console.WriteLine("Models : ");
            foreach (XElement item in models)
            {
                Console.WriteLine("  "+item.Value.ToString());
            }

        }

        public void Task4()
        {
            Console.WriteLine("\nTask 4 ---------------------------------------------\n");

            IEnumerable<XElement> nodes = from car in MyCars
                                          select new XElement("car",
                                                        new XElement("model", car.Model),
                                                        new XElement("year", car.Year),
                                                        new XElement("engine", 
                                                            new XAttribute("model", car.Motor.Model),
                                                            new XElement("displacement", car.Motor.Displacement),
                                                            new XElement("horsePower", car.Motor.HorsePower)
                                                        )
                                                     );
            XElement rootNode = new XElement("cars", nodes);
            Console.Write(rootNode+"\n");
            rootNode.Save("CarsFromLinq.xml");
        }

        public void Task5()
        {
            Console.WriteLine("\nTask 5 ---------------------------------------------\n");

            XDocument xmlFile = XDocument.Load("Template.html");
            var body = xmlFile.Root.LastNode as XElement;

            IEnumerable<XElement> tableContent = from car in MyCars
                                          select new XElement("tr",
                                                    new XElement("td", car.Model),
                                                    new XElement("td", car.Motor.Model),
                                                    new XElement("td", car.Motor.Displacement),
                                                    new XElement("td", car.Motor.HorsePower),
                                                    new XElement("td", car.Year)
                                                );
            body.Add(new XElement("table", new XAttribute("border", 1), tableContent));
            xmlFile.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CarsHtml.html"));
            Console.Write(xmlFile + "\n");
        }

        public void Task6()
        {
            Console.WriteLine("\nTask 6 ---------------------------------------------\n");

            XDocument xmlFile = XDocument.Load("CarsCollection.xml");
            var query = from c in xmlFile.Element("cars").Elements("car").Elements("engine").Elements("horsePower")
                        select c;
            foreach (XElement hp in query)
            {
                hp.Name = "hp";
            }
            
            var query2 = from c in xmlFile.Element("cars").Elements("car")
                        select c;
            foreach (XElement car in query2)
            {
                car.Element("model").Add(new XAttribute("year", car.Element("year").Value));
                car.Element("year").Remove();
            }

            Console.Write(xmlFile + "\n");
        }



        static void Main(string[] args)
        {
            Program p = new Program();
            p.Task1();
            p.Task2();
            p.Task3();
            p.Task4();
            p.Task5();
            p.Task6();


        }
    }
}
