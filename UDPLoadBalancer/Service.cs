using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using UDPLoadBalancer.Configuration;

namespace UDPLoadBalancer
{
    class Service : ServiceBase
    {
        LoadBalancerSection config;

        public Service()
        {
            
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            StartWorkers(args);
        }

        protected override void OnStop()
        {
            base.OnStop();

            StopWorkers();
        }

        internal void StartWorkers(string[] args)
        {
            config = ConfigurationManager.GetSection("loadBalancers") as LoadBalancerSection;

            foreach (LoadBalancerElement el in config.LoadBalancers)
            {
                Log.Verbose("Creating Load Balancer listening on {ListenAddress}:{ListenPort}", el.ListenAddress, el.ListenPort);

                LoadBalancer lb = new LoadBalancer(el.ListenAddress, el.ListenPort);

                foreach (LoadBalancerNodeElement node in el.Nodes)
                {
                    Log.Verbose("Adding Node to {ListenEndpoint} with Address {NodeAddress} and Port {NodePort}", lb.ListenEndpoint, node.Address, node.Port);

                    lb.Servers.Add(new LoadBalancer.Node(node.Address, node.Port));
                }

                LoadBalancerManager.Instance.LoadBalancers.Add(lb);
            }

            Log.Verbose("Starting {LoadBalancerCount} Load Balancers", LoadBalancerManager.Instance.LoadBalancers.Count);
            foreach (LoadBalancer lb in LoadBalancerManager.Instance.LoadBalancers)
            {
                lb.Start();
            }
        }

        internal void StopWorkers()
        {
            Log.Verbose("Stopping {LoadBalancerCount} Load Balancers", LoadBalancerManager.Instance.LoadBalancers.Count);
            foreach (LoadBalancer lb in LoadBalancerManager.Instance.LoadBalancers)
            {
                lb.Stop();
            }
        }
    }
}
