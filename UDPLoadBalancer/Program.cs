using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using UDPLoadBalancer.Configuration;

namespace UDPLoadBalancer
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadBalancerSection config = ConfigurationManager.GetSection("loadBalancers") as LoadBalancerSection;
            List<LoadBalancer> loadBalancers = new List<LoadBalancer>();
            
            Console.WriteLine("Configuration Loaded. {0} Load Balancers loaded.", config.LoadBalancers.Count);
            foreach (LoadBalancerElement el in config.LoadBalancers)
            {
                Console.WriteLine("  Listen Address: {0}", el.ListenAddress);
                Console.WriteLine("  Listen Port: {0}", el.ListenPort);

                LoadBalancer lb = new LoadBalancer(el.ListenAddress, el.ListenPort);

                foreach(LoadBalancerNodeElement node in el.Nodes)
                {
                    Console.WriteLine("     Node Address: {0}", node.Address);
                    Console.WriteLine("     Node Port: {0}", node.Port);
                    Console.WriteLine("==============================");

                    lb.Servers.Add(new LoadBalancer.Node(node.Address, node.Port));
                }

                lb.Start();
            }

            while(true)
            {
                string line = Console.ReadLine();

                if (line == "quit" || line == "q")
                    break;

                if(line == "stats")
                {
                    Console.WriteLine("Fetching Load Balancer Statistics");
                    foreach (var lb in loadBalancers)
                    {
                        foreach (var server in lb.Servers)
                        {
                            Console.WriteLine("Server {0}", server.Hostname);
                            Console.WriteLine("  Status: {0}", server.Status);
                            Console.WriteLine("  Response Time: {0}", server.ResponseTime);
                            Console.WriteLine("  Messages Sent: {0}", server.SendCounter);
                            Console.WriteLine("==========================================");
                        }
                    }
                }
            }

            foreach(var lb in loadBalancers)
            {
                Console.WriteLine("Load Balancer {0} stopping...", lb.ListenEndpoint.ToString());
                lb.Stop();
            }

            Console.WriteLine("Load Balancers stopped. Press a key to exit.");
            Console.ReadKey();
        }
    }
}
