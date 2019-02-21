using Sentry.EntityFramework;
using Sentry.Extensibility;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sentry.Samples.AspNet.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IDisposable _sentrySdk;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // We add the query logging here so multiple DbContexts in the same project are supported
            SentryDatabaseLogging.UseBreadcrumbs();

            // Set up the sentry SDK
            _sentrySdk = SentrySdk.Init(o =>
            {
                // We store the DSN inside Web.config; make sure to use your own DSN!
                o.Dsn = new Dsn(ConfigurationManager.AppSettings["SentryDsn"]);
                // Add the EntityFramework integration
                o.AddEntityFramework();
                o.SendDefaultPii = true;
                o.AddEventProcessor(new AspNetRequestProcessor(o));
            });
        }

        private class AspNetRequestProcessor : ISentryEventProcessor
        {
            private readonly SentryOptions _options;

            public AspNetRequestProcessor(SentryOptions options)
            {
                _options = options ?? throw new ArgumentNullException(nameof(options));
            }

            public SentryEvent Process(SentryEvent @event)
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                {
                    return @event;
                }

                @event.Request.Method = HttpContext.Current.Request.HttpMethod;
                @event.Request.Url = context.Request.Path;

                @event.Request.QueryString = context.Request.QueryString.ToString();
                foreach (string key in context.Request.Headers.AllKeys)
                {

                    if (!_options.SendDefaultPii
                        // Don't add headers which might contain PII
                        && (key == "Cookie"
                            || key == "Authorization"))
                    {
                        continue;
                    }
                    @event.Request.Headers[key] = context.Request.Headers[key];
                }


                if (_options?.SendDefaultPii == true)
                {
                    @event.Request.Env["REMOTE_ADDR"] = context.Request.UserHostAddress;
                }

                @event.User.Username = context.User.Identity?.Name;

                @event.Request.Env["SERVER_NAME"] = Environment.MachineName;

                string server = context.Response.Headers["Server"];
                if (server != null)
                {
                    @event.Request.Env["SERVER_SOFTWARE"] = server;
                }

                return @event;
            }
        }

        // Global error catcher
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            SentrySdk.CaptureException(exception);
        }

        public override void Dispose()
        {
            _sentrySdk.Dispose();
            base.Dispose();
        }
    }
}
