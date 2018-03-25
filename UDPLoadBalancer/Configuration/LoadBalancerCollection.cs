using System.Configuration;

namespace UDPLoadBalancer.Configuration
{
    [ConfigurationCollection(typeof(LoadBalancerElement), AddItemName = "loadBalancer", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class LoadBalancerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoadBalancerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoadBalancerElement)element).Key;
        }

        public void Add(LoadBalancerElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(LoadBalancerElement element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(LoadBalancerElement element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(element.Key);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public LoadBalancerElement this[int index]
        {
            get { return (LoadBalancerElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }
}
