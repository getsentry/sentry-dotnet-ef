using System.Data.Entity.Infrastructure.Interception;

namespace Sentry.EntityFramework
{
    /// <summary>
    /// Sentry Database Logger
    /// </summary>
    public static class SentryDatabaseLogging
    {
        /// <summary>
        /// Adds an instance of <see cref="SentryCommandInterceptor"/> to <see cref="DbInterception"/>
        /// This is a static setup call, so make sure you only call it once for each <see cref="IQueryLogger"/> instance you want to register globally
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbCommandLogFormatter"></param>
        public static SentryCommandInterceptor UseBreadcrumbs(IQueryLogger logger = null, IDbCommandLogFormatter dbCommandLogFormatter = null)
        {
            logger = logger ?? new SentryQueryLogger();
            dbCommandLogFormatter = dbCommandLogFormatter ?? new DbCommandLogFormatter();
            var interceptor = new SentryCommandInterceptor(logger, dbCommandLogFormatter);
            DbInterception.Add(interceptor);
            return interceptor;
        }
    }
}
