using Experience_SDK_Example.ExperienceSDK.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Experience_SDK_Example.ExperienceSDK
{
    /// <summary>
    /// Internal class. 
    /// Gets application and data events, extends it with common information, account(from init params), 
    /// time(from time sync service). And ingest it to Data processing services via Data transfer module​
    /// </summary>
    internal class EventProcessor
    {
        private string _accHash;
        Amazon.RegionEndpoint _region;
        DataTransport _transport;
        TimeSyncService _timeService;

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="accHash">Account hash provided by Realeyes</param>
        /// <param name="region">Region to ingest data</param>
        internal EventProcessor(string accHash, Amazon.RegionEndpoint region)
        {
            _accHash = accHash;
            _region = region;
            _transport = new DataTransport(_region);
            _timeService = new TimeSyncService(new TimeSpan(0, 5, 0));
        }

        /// <summary>
        /// Sends data to Realeyes services 
        /// </summary>
        /// <param name="eventData">Data object to transfer</param>
        /// <param name="force">Signals to send messages immediatelly without queueing</param>
        /// <returns>Returns 'true' if data has been sent successfully </returns>
        internal bool Send(BaseEvent eventData, bool force = false)
        {
            eventData.AccountHash = _accHash;
            eventData.AbsoluteLocalTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + DateTime.Now.ToString("zzz").Replace(":", "");
            eventData.AbsoluteUtcTime = _timeService.GetTime().ToString("yyyy-MM-ddTHH:mm:ss.fff\\Z");
            eventData.Platform = "desktop";
            eventData.Version = "2.0";
            eventData.AppInstanceId = Process.GetCurrentProcess().Id.ToString();
            eventData.Platform = "desktop";
            return _transport.Send(eventData, force);
        }
    }
}
