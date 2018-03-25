﻿using System.Configuration;

namespace UDPLoadBalancer.Configuration
{
    public class LoadBalancerNodeElement : ConfigurationElement
    {
        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)base["address"]; }
            set { base["address"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        internal string Key
        {
            get { return string.Format("{0}|{1}", Address, Port); }
        }
    }
}
