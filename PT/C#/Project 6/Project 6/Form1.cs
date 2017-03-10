using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_6
{
    public partial class Form1 : Form
    {
        public Point MouseInitialPoint { get; set; }
        public Pen MyPen { get; set; }
        public Graphics MyGraphics { get; set; }
        public UdpClient MyUdpClient { get; set; }
        public UdpClient DrawingUdpClient { get; set; }
        public UdpClient RecvUdpClient { get; set; }
        public int PortNumber { get; set; }
        public int RecvPortNumber { get; set; }
        public bool IsConnected { get; set; }

        public Form1()
        {
            InitializeComponent();
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            MyGraphics = Graphics.FromImage(pictureBox.Image);
            MyPen = new Pen(Color.Red, 5);
            IsConnected = false;
            MyUdpClient = null;
            DrawingUdpClient = null;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(serverPortTextBox.Text);
            string server = serverIPTextBox.Text;
            MyUdpClient = new UdpClient();
            MyUdpClient.Connect(server, port);

            string msg = "connect";
            byte[] data = Encoding.ASCII.GetBytes(msg);
            MyUdpClient.Send(data, data.Length);

            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            data = MyUdpClient.Receive(ref remoteIPEndPoint);

            msg = Encoding.ASCII.GetString(data, 0, 4);
            PortNumber = Convert.ToInt32(msg);
            
            DrawingUdpClient = new UdpClient();
            DrawingUdpClient.Connect(server, PortNumber);
            
            Task.Run(() => ReceivePoints());

            IsConnected = true;
            statusTextBox.Text = "connected";

            MsgTextBox.Text += PortNumber + "\n" + RecvPortNumber + "\n";
            
        }

        public void ReceivePoints()
        {
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Any, PortNumber);
            UdpClient myUdpClient = new UdpClient(localIPEndPoint);
            IPEndPoint remoteIPEndPoint;

            Dictionary<byte, Color> colors = new Dictionary<byte, Color>();
            Dictionary<byte, Point> lastPoint = new Dictionary<byte, Point>();
            while (IsConnected)
            {
                remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = myUdpClient.Receive(ref remoteIPEndPoint);
                string msg = Encoding.ASCII.GetString(data, 0, data.Length);
                MsgTextBox.Text += msg + "\n";
            }
        }
        
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            IsConnected = false;
            statusTextBox.Text = "disconnected";
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowHelp = true;
            MyDialog.Color = MyPen.Color;
            
            if (MyDialog.ShowDialog() == DialogResult.OK)
                MyPen.Color = MyDialog.Color;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsConnected)
            {
                MouseInitialPoint = e.Location;

                string msg = MyPen.Color.ToString() + MouseInitialPoint.ToString();
                byte[] data = Encoding.ASCII.GetBytes(msg);
                DrawingUdpClient.Send(data, data.Length);
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(IsConnected && e.Button == MouseButtons.Left)
            {
                Point mousePoint = e.Location;

                string msg = mousePoint.ToString();
                byte[] data = Encoding.ASCII.GetBytes(msg);
                DrawingUdpClient.Send(data, data.Length);

                MyGraphics.DrawLine(MyPen,
                                    MouseInitialPoint.X, MouseInitialPoint.Y,
                                    mousePoint.X, mousePoint.Y);
                pictureBox.Refresh();
                MouseInitialPoint = mousePoint;
            }
        }
    }
}
