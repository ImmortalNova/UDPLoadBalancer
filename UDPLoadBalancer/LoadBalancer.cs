using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;

namespace UDPLoadBalancer
{
    class LoadBalancerServer
    {
        public enum State
        {
            Dead,
            Alive
        }

        private string _hostOrAddress;
        private int _port;
        private IPEndPoint _endpoint;

        public int SendCounter { get; set; } = 0;
        public long ResponseTime { get; private set; } = -1;
        public DateTime LastChecked { get; private set; }
        public State Status { get; private set; }

        // TODO: Implement various status checking algorithms?
        public State CheckStatus()
        {
            LastChecked = DateTime.Now;

            if(Endpoint != null)
            {
                var ping = new Ping();
                var reply = ping.Send(Endpoint.Address, 50);

                ResponseTime = reply.RoundtripTime;

                if (reply.Status == IPStatus.Success)
                {
                    return Status = State.Alive;
                }
            }

            return Status = State.Dead;
        }

        internal LoadBalancerServer() { }
        internal LoadBalancerServer(string hostOrAddress, int port)
        {
            _hostOrAddress = hostOrAddress;
            _port = port;
        }

        public string Hostname { get { return _hostOrAddress; } set { _hostOrAddress = value; _endpoint = null; } }
        public int Port { get { return _port; } set { _port = value; _endpoint = null; } }

        public IPEndPoint Endpoint {
            get {
                if (_endpoint == null)
                {
                    try
                    {
                        _endpoint = HelperMethods.IPEndPointFromHostname(_hostOrAddress, _port);
                    }
                    catch(SocketException)
                    {
                    }
                }

                return _endpoint;
            }

            set
            {
                _endpoint = value;
            }
        }
    }

    class LoadBalancer
    {
        public readonly List<LoadBalancerServer> Servers = new List<LoadBalancerServer>();
        public IPEndPoint ListenEndpoint { get; private set; }

        private volatile bool _isRunning = true;
        private Thread _listenThread;
        private Thread _statusMonitor;
        private int _statusMonitorThreadInterval = 30000;

        public LoadBalancer(string ipOrHostname, int listenPort) : this(HelperMethods.IPEndPointFromHostname(ipOrHostname, listenPort)) { }

        public LoadBalancer(IPAddress ipAddress, int listenPort) : this(new IPEndPoint(ipAddress, listenPort)) { }

        public LoadBalancer(IPEndPoint listenEndpoint) : this()
        {
            ListenEndpoint = listenEndpoint;
        }

        private LoadBalancer()
        {
            _statusMonitor = new Thread(new ThreadStart(StatusMonitorThreadWork));
            _statusMonitor.IsBackground = true;

            _listenThread = new Thread(new ThreadStart(ListenThreadWork));
            _listenThread.IsBackground = true;
        }

        public void Start()
        {
            _isRunning = true;

            _statusMonitor.Start();
            _listenThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;

            _statusMonitor.Interrupt();
            _listenThread.Interrupt();
        }

        private void StatusMonitorThreadWork()
        {
            try
            {
                while (_isRunning)
                {
                    foreach (LoadBalancerServer s in Servers)
                    {
                        s.CheckStatus();
                    }

                    Thread.Sleep(_statusMonitorThreadInterval);
                }
            }
            catch(ThreadInterruptedException) { }
        }

        private void ListenThreadWork()
        {
            using (UdpClient listener = new UdpClient(ListenEndpoint))
            {
                using (UdpClient relayer = new UdpClient())
                {
                    try
                    {
                        Console.WriteLine("Load Balancer listening on {0}", ListenEndpoint.ToString());

                        IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                        LoadBalancerServer targetServer = null;

                        while (_isRunning)
                        {
                            byte[] bytes = listener.Receive(ref remoteEp);

                            targetServer = Server;
                            if (targetServer != null && targetServer.Endpoint != null)
                            {
                                Console.WriteLine("Relaying UDP Datagram from {0} to {1}:\r\n {2}\r\n=========", remoteEp.ToString(), targetServer.Endpoint.ToString(), Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                                relayer.Send(bytes, bytes.Length, targetServer.Endpoint);
                                ++targetServer.SendCounter;
                            }
                            else
                            {
                                Console.WriteLine("No servers are available for relaying.");
                            }
                        }
                    }
                    catch(ThreadInterruptedException) { }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception in Listen Thread.\r\n{0}", e.ToString());
                    }
                    finally
                    {
                        listener.Close();
                        relayer.Close();
                    }
                }
            }
        }

        public enum ServerSelectionAlgorithm
        {
            Random,
            FirstAlive,
            RoundRobin
        }

        public ServerSelectionAlgorithm SelectionAlgorithm { get; set; }
        private Random _random = new Random();
        private int _rrLastIndex = 0;

        // TODO: Implement different Load Balancer selection algorithms (round robin, random etc)
        public LoadBalancerServer Server
        {
            get
            {
                var availableServers = Servers.Where(server => server.Status == LoadBalancerServer.State.Alive);
                var cntAvailable = availableServers.Count();
                if (availableServers.Count() > 0)
                {
                    int i = 0;

                    switch(SelectionAlgorithm)
                    {
                        default:
                        case ServerSelectionAlgorithm.Random:
                            i = _random.Next(availableServers.Count());
                            break;
                        case ServerSelectionAlgorithm.RoundRobin:
                            i = (_rrLastIndex >= (availableServers.Count()-1)) ? 0 : _rrLastIndex+1;
                            _rrLastIndex = i;
                            break;
                        case ServerSelectionAlgorithm.FirstAlive:
                            i = 0;
                            break;
                    }

                    return availableServers.ElementAt(i);
                }

                return null;
            }
        }
    }
}
