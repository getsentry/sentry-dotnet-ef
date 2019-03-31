using System.Data.Common;

namespace Sentry.EntityFramework
{
    /// <summary>
    /// Default implementation of <see cref="IDbCommandLogFormatter"/>
    /// returns CommandText to log
    /// </summary>
    internal class DbCommandLogFormatter : IDbCommandLogFormatter
    {
        public string GetLogMessage(DbCommand command)
        {
            return command.CommandText;
        }
    }

}
