using System;
using Isban.Mercados.LogTrace;

namespace Isban.Maps.Host.Webapi
{
    public class WebMessageLoggingHelper : LoggingBase
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="LoggingHelper"/> class from being created.
        /// </summary>
        private WebMessageLoggingHelper()
        {
        }
        public override string LoggingSourceName => "WebMessageLogging";
        /// <summary>
        /// The instance
        /// </summary>
        private static readonly Lazy<WebMessageLoggingHelper> instance = new Lazy<WebMessageLoggingHelper>(() => new WebMessageLoggingHelper());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static WebMessageLoggingHelper Instance => instance.Value;
    }
}