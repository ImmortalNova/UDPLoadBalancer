using System.Configuration;
using UDPLoadBalancer.Configuration;

namespace UDPLoadBalancer.Configuration
{
    public class LoadBalancerSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty _propLoadBalancers = new ConfigurationProperty(
            null,
            typeof(LoadBalancerCollection),
            null,
            ConfigurationPropertyOptions.IsDefaultCollection
        );

        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        static LoadBalancerSection()
        {
            _properties.Add(_propLoadBalancers);
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public LoadBalancerCollection LoadBalancers
        {
            get { return (LoadBalancerCollection)base[_propLoadBalancers]; }
        }
    }
}
