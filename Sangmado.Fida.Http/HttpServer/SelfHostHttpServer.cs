using System;
using System.Net;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Http
{
    public class SelfHostHttpServer
    {
        private ILog _log = Logger.Get<SelfHostHttpServer>();
        private InjectedSelfHost _host;
        private readonly object _sync = new object();
        private IHttpEngineBuilder _engineBuilder;

        public SelfHostHttpServer(int listenPort, IHttpEngineBuilder engineBuilder)
            : this(IPAddress.Any, listenPort, engineBuilder)
        {
        }

        public SelfHostHttpServer(IPAddress localIPAddress, int listenPort, IHttpEngineBuilder engineBuilder)
            : this(new IPEndPoint(localIPAddress, listenPort), engineBuilder)
        {
        }

        public SelfHostHttpServer(IPEndPoint localEP, IHttpEngineBuilder engineBuilder)
        {
            if (localEP == null)
                throw new ArgumentNullException("localEP");
            if (engineBuilder == null)
                throw new ArgumentNullException("engineBuilder");

            LocalEndPoint = localEP;
            _engineBuilder = engineBuilder;
        }

        public bool IsListening { get; private set; }
        public IPEndPoint LocalEndPoint { get; private set; }

        public void Start()
        {
            lock (_sync)
            {
                if (IsListening) return;

                var uri = new Uri(string.Format(@"http://{0}:{1}", LocalEndPoint.Address, LocalEndPoint.Port));
                if (LocalEndPoint.Address == IPAddress.Any)
                {
                    uri = new Uri(string.Format(@"http://{0}:{1}", "localhost", LocalEndPoint.Port));
                }

                var engine = _engineBuilder.Build();
                _host = new InjectedSelfHost(engine, uri);
                _host.Start();
                IsListening = true;

                _log.DebugFormat("Http server is listening to {0}.", uri);
            }
        }

        public void Stop()
        {
            lock (_sync)
            {
                if (!IsListening) return;

                _host.Stop();
                IsListening = false;
            }
        }
    }
}
