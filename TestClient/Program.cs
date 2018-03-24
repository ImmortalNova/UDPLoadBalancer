using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();

            Console.Write("Please enter the Remote Hostname/IP Address [localhost]: ");
            String hostname = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(hostname))
            {
                hostname = "localhost";
            }
            IPHostEntry ipHost = Dns.GetHostEntry(hostname);
            IPAddress ipAddress = ipHost.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).First();

            Console.Write("Please enter the Remote Port [514]: ");
            String strPort = Console.ReadLine();
            int port = int.TryParse(strPort, out port) ? port : 514;

            IPEndPoint remoteEp = new IPEndPoint(ipAddress, port);

            Console.WriteLine("Remote Endpoint set to {0}", remoteEp);

            try
            {
                while (true)
                {
                    string line = Console.ReadLine();

                    if (line == "quit" || line == "q")
                    {
                        break;
                    }

                    byte[] dgram = Encoding.ASCII.GetBytes(line);

                    Console.WriteLine("Sending UDP Datagram to {0}", remoteEp.ToString());
                    client.Send(dgram, dgram.Length, remoteEp);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                client.Close();
            }
        }
    }
}
