using System;
using System.Linq;

namespace FlushTheToiletWebServer.Services
{
    public interface IFlushService
    {
        bool IsFlushing { get; }

        /// <summary>
        /// Flush with default flush time
        /// </summary>
        void Flush();

        void Stop();

        /// <summary>
        /// Flushtime in seconds
        /// </summary>
        /// <param name="flushTime"></param>
        void Flush(double flushTime);
    }
}
