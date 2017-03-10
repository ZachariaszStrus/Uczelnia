using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Car> MyCars { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MyCars = new List<Car>(){
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
            dataGrid.ItemsSource = MyCars;
        }

        public void Task1()
        {
            var query1 = from motor in (from car in MyCars
                                        select new
                                        {
                                           engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol",
                                           hppl = car.Motor.HorsePower / car.Motor.Displacement
                                        })
                         group motor by motor.engineType into motorType
                         select new
                         {
                             engineType = motorType.Key,
                             avgHPPL = motorType.Average(m => m.hppl)
                         };
            textBox.Text += "query expression syntax : \n";
            foreach (var item in query1)
            {
                textBox.Text += item.engineType + " : " + item.avgHPPL + "\n";
            }

            var query2 = MyCars.Select(car => new
                                {
                                    engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol",
                                    hppl = car.Motor.HorsePower / car.Motor.Displacement
                                }).
                                GroupBy(motor => motor.engineType).
                                Select(motorType => new
                                {
                                    engineType = motorType.Key,
                                    avgHPPL = motorType.Average(m => m.hppl)
                                });
            textBox.Text += "\nmethod-based query syntax : \n";
            foreach (var item in query2)
            {
                textBox.Text += item.engineType + " : " + item.avgHPPL + "\n";
            }
        }
        
        public void Task2()
        {
            Func<Car, Car, int> arg1;
            arg1 = delegate(Car c1, Car c2){
                return (int)c2.Motor.HorsePower - (int)c1.Motor.HorsePower;
            };

            Predicate<Car> arg2;
            arg2 = delegate (Car c)
            {
                return c.Motor.Model == "TDI";
            };

            Action<Car> arg3;
            arg3 = delegate (Car c)
            {
                MessageBox.Show(c.ToString());
            };

            MyCars.Sort(new Comparison<Car>(arg1));
            MyCars.FindAll(arg2).ForEach(arg3);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Task1();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Task2();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && e.Column == dataGrid.Columns[1])
            {
                var textBox = e.EditingElement as TextBox;
                var car = e.Row.Item as Car;
                var oldEngine = car.Motor;
                if (!oldEngine.CreateFromString(textBox.Text))
                {
                    textBox.Text = oldEngine.ToString();
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = dataGrid.SelectedItem as Car;
            if(selectedCar != null)
            {
                MyCars.Remove(selectedCar);
            }
        }
    }
}
