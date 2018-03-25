using System.Configuration;

namespace UDPLoadBalancer.Configuration
{
    [ConfigurationCollection(typeof(LoadBalancerNodeElement), AddItemName = "node", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class LoadBalancerNodeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoadBalancerNodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoadBalancerNodeElement)element).Key;
        }

        public void Add(LoadBalancerNodeElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(LoadBalancerNodeElement element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(LoadBalancerNodeElement element)
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

        public LoadBalancerNodeElement this[int index]
        {
            get { return (LoadBalancerNodeElement)BaseGet(index); }
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
