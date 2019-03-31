using System.Data.Common;

namespace Sentry.EntityFramework
{
    public interface IDbCommandLogFormatter
    {
        /// <summary>
        /// Gets the message to log from <paramref name="command"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string GetLogMessage(DbCommand command);
    }
}
