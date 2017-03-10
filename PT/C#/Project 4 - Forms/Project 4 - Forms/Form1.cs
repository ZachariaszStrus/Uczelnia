using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_4___Forms
{
    public partial class Form1 : Form
    {
        List<Car> MyCars { get; set; }

        MyBindingList<Car> MyCarsBindingList { get; set; }

        public Form1()
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

            MyCarsBindingList = new MyBindingList<Car>(MyCars);
            BindingSource carBindingSource = new BindingSource();
            carBindingSource.DataSource = MyCarsBindingList;

            dataGridView1.DataSource = carBindingSource;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                dataGridView1.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
            }
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
            arg1 = delegate (Car c1, Car c2) {
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


        private void task1_Click(object sender, EventArgs e)
        {
            Task1();
        }

        private void task2_Click(object sender, EventArgs e)
        {
            Task2();
        }

        private void toolStripComboBox1_Enter(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Clear();
            foreach (DataGridViewTextBoxColumn column in dataGridView1.Columns)
            {
                if(column.ValueType == typeof(System.Int32) || column.ValueType == typeof(System.String))
                    toolStripComboBox1.Items.Add(column.Name);
            }
            toolStripComboBox1.SelectedIndex = 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var key = toolStripTextBox1.Text;
            if (string.IsNullOrEmpty(key)) return;

            var findIn = toolStripComboBox1.Text;
            if (string.IsNullOrEmpty(findIn)) return;

            var bindingSource = dataGridView1.DataSource;

            PropertyDescriptorCollection properties =
                    ((ITypedList)bindingSource).GetItemProperties(null);
            PropertyDescriptor property = properties[findIn];
            
            int index = MyCarsBindingList.Find(property, key);
            
            if(index >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[index].Selected = true;
            }
            else
            {
                MessageBox.Show("No items matching your search criteria were found");
            }
        }
    }
}
