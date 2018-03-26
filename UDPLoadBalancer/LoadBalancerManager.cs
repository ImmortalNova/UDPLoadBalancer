using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPLoadBalancer
{
    internal sealed class LoadBalancerManager
    {
        internal static LoadBalancerManager Instance { get; } = new LoadBalancerManager();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static LoadBalancerManager() { }

        internal List<LoadBalancer> LoadBalancers { get; } = new List<LoadBalancer>();

        private LoadBalancerManager()
        {

        }
    }
}
