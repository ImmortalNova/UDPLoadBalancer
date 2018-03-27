using System.Configuration;

namespace UDPLoadBalancer.Configuration
{
    public class LoadBalancerElement : ConfigurationElement
    {
        [ConfigurationProperty("listenAddress", IsRequired = true)]
        public string ListenAddress
        {
            get { return (string)base["listenAddress"]; }
            set { base["listenAddress"] = value; }
        }

        [ConfigurationProperty("listenPort", IsRequired = true)]
        public int ListenPort
        {
            get { return (int)base["listenPort"]; }
            set { base["listenPort"] = value; }
        }

        [ConfigurationProperty("nodes", IsRequired = true)]
        public LoadBalancerNodeCollection Nodes
        {
            get { return (LoadBalancerNodeCollection)base["nodes"]; }
            set { base["nodes"] = value; }
        }

        internal string Key
        {
            get { return string.Format("{0}:{1}", ListenAddress, ListenPort); }
        }
    }
}
