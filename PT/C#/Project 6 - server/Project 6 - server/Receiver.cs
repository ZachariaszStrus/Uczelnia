using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_6___server
{
    class Receiver
    {
        public int PortNumber { get; set; }
        public byte ID { get; set; }
        public Server MyServer { get; set; }

        public Receiver(byte id, int port, Server server)
        {
            PortNumber = port;
            ID = id;
            MyServer = server;
        }

        public void Start()
        {
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Any, PortNumber);
            UdpClient myUdpClient = new UdpClient(localIPEndPoint);
            IPEndPoint remoteIPEndPoint;

            byte[] data;
            string msg;
            while (true)
            {
                remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                data = myUdpClient.Receive(ref remoteIPEndPoint);
                msg = Encoding.ASCII.GetString(data, 0, data.Length);
                if (msg.Length > 0)
                {
                    Console.WriteLine("{0} > {1}", ID, msg);
                    byte[] drawInfo = new byte[data.Length+1];
                    Buffer.BlockCopy(data, 0, drawInfo, 1, data.Length);
                    drawInfo[0] = ID;
                    MyServer.DrawInfos.Enqueue(drawInfo);
                }
            }
        }
    }
}
