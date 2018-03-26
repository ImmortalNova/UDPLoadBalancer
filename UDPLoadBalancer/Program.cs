using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.ServiceProcess;
using UDPLoadBalancer.Configuration;

namespace UDPLoadBalancer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                   .WriteTo.Console()
                   .WriteTo.EventLog("UDPLoadBalancer", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                   .CreateLogger();

            Service svc = new Service();

            if (Environment.UserInteractive)
            {
                svc.StartWorkers(args);

                while (true)
                {
                    string line = Console.ReadLine();

                    if (line == "quit" || line == "q")
                        break;

                    if (line == "stats")
                    {
                        Console.WriteLine("Fetching Load Balancer Statistics");
                        foreach (var lb in LoadBalancerManager.Instance.LoadBalancers)
                        {
                            foreach (var server in lb.Servers)
                            {
                                Log.Debug("Server {NodeHostname}\r\n  Status: {NodeStatus}\r\n  Response Time: {NodeResponseTime}\r\n  Messages Sent: {0}", server.Hostname, server.Status, server.ResponseTime, server.SendCounter);
                            }
                        }
                    }
                }

                svc.StopWorkers();

                Log.Debug("Load Balancers stopped. Press a key to exit.");
                Console.ReadKey();
            }
            else
            {
                ServiceBase.Run(svc);
            }
        }
    }
}
