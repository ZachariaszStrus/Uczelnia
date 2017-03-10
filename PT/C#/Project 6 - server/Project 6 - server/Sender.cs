using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_6___server
{
    class Sender
    {
        public Server MyServer { get; set; }

        public Sender(Server server)
        {
            MyServer = server;
        }

        public void Start()
        {
            while (true)
            {
                if (!MyServer.DrawInfos.IsEmpty)
                {
                    byte[] data;
                    MyServer.DrawInfos.TryDequeue(out data);
                    foreach (var clientIP in MyServer.Clients)
                    {
                        IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Any, 9998);
                        UdpClient myUdpClient = new UdpClient(localIPEndPoint);
                        myUdpClient.Send(data, data.Length, clientIP);
                    }
                }
            }
        }
    }
}
