using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_6___server
{
    class Server
    {
        public Dictionary<byte, Task> Receivers { get; set; }
        public List<IPEndPoint> Clients { get; set; }
        public ConcurrentQueue<byte[]> DrawInfos { get; set; }


        private byte _nextID;

        public Server()
        {
            Receivers = new Dictionary<byte, Task>();
            Clients = new List<IPEndPoint>();
            DrawInfos = new ConcurrentQueue<byte[]>();
            _nextID = 0;
        }

        public void Start()
        {
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Any, 9999);
            UdpClient myUdpClient = new UdpClient(localIPEndPoint);
            IPEndPoint remoteIPEndPoint;

            Console.WriteLine("Waiting for a client ...");

            Task.Run(() => new Sender(this).Start());

            byte[] data;
            string msg;
            while (true)
            {
                remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                data = myUdpClient.Receive(ref remoteIPEndPoint);
                msg = Encoding.ASCII.GetString(data, 0, data.Length);
                if (msg == "connect")
                {
                    byte id = GetID();
                    int port = GetRecvPort(id);
                    
                    Console.WriteLine("{0} > {1} , {2}", remoteIPEndPoint, msg, port);

                    msg = port.ToString();
                    data = Encoding.ASCII.GetBytes(msg);
                    myUdpClient.SendAsync(data, data.Length, remoteIPEndPoint);

                    Receiver receiver = new Receiver(id, port, this);
                    Task task = Task.Run(() => receiver.Start());
                    Receivers.Add(id, task);
                    
                    Clients.Add(remoteIPEndPoint);
                }
            }
        }
        
        public byte GetID()
        {
            return _nextID++;
        }

        private int GetRecvPort(byte id)
        {
            return 9998 - id;
        }

        private int GetSendPort(byte id)
        {
            return 9998 - id - 256;
        }

        static void Main(string[] args)
        {
            new Server().Start();
        }
    }
}
