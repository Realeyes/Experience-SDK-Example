using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Experience_SDK_Example.ExperienceSDK
{
    /// <summary>
    /// Time to time gets time sync point from RE services (https://darwinapi-cdn.realeyesit.com/api/info/time) 
    /// To keep sending events in sync accross multiple devices  
    /// </summary>
    internal class TimeSyncService
    {
        private const string timesyncUrl = "https://darwinapi-cdn.realeyesit.com/api/info/time";
        private TimeSpan _syncInt;
        private DateTime _time;
        private Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="syncInterval"> Defines interval to sync data with Realeyes time server</param>
        internal TimeSyncService(TimeSpan syncInterval)
        {
            _time = GetTime(true);
            _syncInt = syncInterval;
        }

        /// <summary>
        /// Gets synchronized time with Realeyes servers
        /// </summary>
        /// <param name="force">Signals to force time cache and retreive data from RE sync service now</param>
        /// <returns></returns>
        internal DateTime GetTime(bool force =false) 
        {
            if (force || _stopwatch.ElapsedMilliseconds > _syncInt.TotalMilliseconds) 
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(timesyncUrl).Result;
                    response.EnsureSuccessStatusCode();
                    if (DateTime.TryParse(response.Content.ReadAsStringAsync().Result.Replace("\"", ""), out  _time)) 
                    {
                        _stopwatch.Restart();
                    }
                }
            }

            return _time.AddMilliseconds(_stopwatch.ElapsedMilliseconds);
        }
    }
}
