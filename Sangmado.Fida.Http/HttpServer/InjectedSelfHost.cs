using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Happer;
using Happer.Hosting.Self;
using Sangmado.Inka.Extensions;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Http
{
    public class InjectedSelfHost : SelfHost
    {
        private ILog _log = Logger.Get<InjectedSelfHost>();

        public InjectedSelfHost(IEngine engine, params Uri[] baseUris)
            : base(engine, baseUris)
        {
        }

        protected override async Task Process(HttpListenerContext httpContext)
        {
            var watch = Stopwatch.StartNew();

            await base.Process(httpContext);

            watch.StopWatch();
            if (watch.ElapsedSeconds() > 0.2)
                _log.WarnFormat("Process, Url[{0}], StatusCode[{1}], cost [{2}] seconds.",
                    httpContext.Request.Url, httpContext.Response.StatusCode, watch.ElapsedSeconds());
            else
                _log.DebugFormat("Process, Url[{0}], StatusCode[{1}], cost [{2}] seconds.",
                    httpContext.Request.Url, httpContext.Response.StatusCode, watch.ElapsedSeconds());
        }
    }
}
