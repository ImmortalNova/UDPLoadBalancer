using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPLoadBalancer
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadBalancer loadBalancer = new LoadBalancer(IPAddress.Any, 514);
            loadBalancer.Servers.Add(new LoadBalancerServer("localhost", 51411));
            loadBalancer.Servers.Add(new LoadBalancerServer("localhost", 51412));
            loadBalancer.Servers.Add(new LoadBalancerServer("deadserver", 51413));
            loadBalancer.Servers.Add(new LoadBalancerServer("localhost", 51414));
            loadBalancer.Servers.Add(new LoadBalancerServer("localhost", 51415));
            loadBalancer.Start();

            while(true)
            {
                string line = Console.ReadLine();

                if (line == "quit" || line == "q")
                    break;

                if(line == "stats")
                {
                    Console.WriteLine("Fetching Load Balancer Statistics");
                    foreach (var server in loadBalancer.Servers)
                    {
                        Console.WriteLine("Server {0}", server.Hostname);
                        Console.WriteLine("  Status: {0}", server.Status);
                        Console.WriteLine("  Response Time: {0}", server.ResponseTime);
                        Console.WriteLine("  Messages Sent: {0}", server.SendCounter);
                        Console.WriteLine("==========================================");
                    }
                }
            }

            Console.WriteLine("Load Balancer stopping...");

            loadBalancer.Stop();

            Console.WriteLine("Load Balancer stopped. Press a key to exit.");
            Console.ReadKey();
        }
    }
}
