using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace UDPLoadBalancer
{
    public static class HelperMethods
    {
        public static IPAddress DnsLookup(string ipOrHostname)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(ipOrHostname);
            return ipHost.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).First();
        }

        public static IPEndPoint IPEndPointFromHostname(string ipOrHostname, int port)
        {
            return new IPEndPoint(DnsLookup(ipOrHostname), port);
        }
    }
}
