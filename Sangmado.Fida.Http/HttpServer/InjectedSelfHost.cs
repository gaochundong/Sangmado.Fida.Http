using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Happer;
using Happer.Hosting;
using Happer.Hosting.Self;
using Sangmado.Inka.Extensions;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Http
{
    public class InjectedSelfHost : SelfHost
    {
        private ILog _log = Logger.Get<InjectedSelfHost>();

        public InjectedSelfHost(IEngine engine, params Uri[] baseUris)
            : base(engine, Environment.ProcessorCount, baseUris)
        {
        }

        public InjectedSelfHost(IEngine engine, int maxConcurrentNumber, params Uri[] baseUris)
            : base(engine, maxConcurrentNumber, baseUris)
        {
        }

        public InjectedSelfHost(IEngine engine, IRateLimiter rateLimiter, params Uri[] baseUris)
            : base(engine, rateLimiter, baseUris)
        {
        }

        protected override async Task Process(HttpListenerContext httpContext, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            await base.Process(httpContext, cancellationToken);

            watch.StopWatch();
            if (watch.ElapsedSeconds() > 0.2)
                _log.WarnFormat("Request, HttpMethod[{0}], Url[{1}], "
                    + "UserHostName[{2}], UserHostAddress[{3}], ContentType[{4}], "
                    + "RemoteEndPoint[{5}], UserAgent[{6}], StatusCode[{7}], "
                    + "cost [{8}] seconds.",
                    httpContext.Request.HttpMethod,
                    httpContext.Request.Url,
                    httpContext.Request.UserHostName,
                    httpContext.Request.UserHostAddress,
                    httpContext.Request.ContentType,
                    httpContext.Request.RemoteEndPoint,
                    httpContext.Request.UserAgent,
                    httpContext.Response.StatusCode,
                    watch.ElapsedSeconds());
            else
                _log.DebugFormat("Request, HttpMethod[{0}], Url[{1}], "
                    + "UserHostName[{2}], UserHostAddress[{3}], ContentType[{4}], "
                    + "RemoteEndPoint[{5}], UserAgent[{6}], StatusCode[{7}], "
                    + "cost [{8}] seconds.",
                    httpContext.Request.HttpMethod,
                    httpContext.Request.Url,
                    httpContext.Request.UserHostName,
                    httpContext.Request.UserHostAddress,
                    httpContext.Request.ContentType,
                    httpContext.Request.RemoteEndPoint,
                    httpContext.Request.UserAgent,
                    httpContext.Response.StatusCode,
                    watch.ElapsedSeconds());
        }
    }
}
