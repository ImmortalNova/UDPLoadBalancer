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
            bool done = false;

            UdpClient client = new UdpClient();
            IPEndPoint localEp = new IPEndPoint(IPAddress.Loopback, 514);

            try
            {
                while (!done)
                {
                    string line = Console.ReadLine();

                    if (line == "exit" || line == "q")
                    {
                        done = true;
                        break;
                    }

                    byte[] dgram = Encoding.ASCII.GetBytes(line);

                    Console.WriteLine("Sending UDP Datagram to {0}", localEp.ToString());
                    client.Send(dgram, dgram.Length, localEp);
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
