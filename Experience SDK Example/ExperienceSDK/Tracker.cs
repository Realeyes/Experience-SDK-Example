using Experience_SDK_Example.ExperienceSDK.Events;
using System;
using System.Collections.Generic;

namespace Experience_SDK_Example.ExperienceSDK
{
    /// <summary>
    ///  Tracker is main interface of SDK.​
    ///  The class is orchestrator for existing modules(Stream/Camerasource, EventProcessor, etc),
    ///  keep parameters of current context and initialize/orchestrate modules with the parameters​
    ///  NOTE: usually the module is implemented as singleton.To avoid double tracking/stream processing​
    // </summary>    
    public class Tracker
    {
        private static Dictionary<string, Tracker> _currentInstances = new Dictionary<string, Tracker>();       

        private EventProcessor _processor;
        private StreamSource _stream;

        private string _sessionId;
        private string _participantId;
        private bool _started = false;
        private string _appName;
        private string _appType;


        /// <summary>
        /// Private Constructor to avoid creation of multiple instances. 
        /// </summary>
        private Tracker(string account, string appName, string appType, string participantId, string sessionId)
        {
            _appName = appName ?? "unknown";
            _appType = appType ?? "unknown";

            // create and initialize internal modules (EventProcessor, StreamSource)
            _processor = new EventProcessor(account, Amazon.RegionEndpoint.APNortheast1);
            _stream = new StreamSource(10);

            // get/create participant and session ids
            _participantId = participantId ?? Guid.NewGuid().ToString();
            _sessionId = sessionId ?? Guid.NewGuid().ToString();

            // Send init event
            SendInitEvent(_stream.NelModelVersion, _stream.NelSdkVersion);

            // subscrabe to Nel data from stream source
            _stream.DataTracked += Stream_DataTracked;
        }

        /// <summary>
        /// Get instance of tracker per app (app name + app type)
        /// </summary>
        /// <returns>Current instance of Tracker</returns>
        public static Tracker GetTracker(string account, string appName, string appType, string participantId = null, string sessionId = null)
        {
            var key = $"{appName}-{appType}";
            if (!_currentInstances.ContainsKey(key))
            {
                _currentInstances.Add(key, new Tracker(account, appName, appType, participantId, sessionId));
            }

            return _currentInstances[key];
        }

        /// <summary>
        /// Get outside events and send them to Realeyes 
        /// </summary>
        /// <param name="sender">String representation of sender</param>
        /// <param name="sourceEv"> Describes event data to transmit</param>
        /// <returns>Returns 'true' if data has been sent</returns>
        public bool TrackEvent(string sender, BaseEvent sourceEv)
        {
            if (!_started)
            {
                return false;
            }

            var ev = new BaseEvent()
            {
                Type = sourceEv.Type,
                ParticipantId = _participantId,
                SessionId = _sessionId,
                AppName = sourceEv.AppName ?? _appName,
                AppType = sourceEv.AppType ?? _appType,
                Args = sourceEv.Args
            };
            
            return _processor.Send(ev);
        }

        /// <summary>
        /// Starts tracking of data from stream, sending data to Realeys 
        /// </summary>
        /// <returns>true if operation successful</returns>
        public bool Start()
        {
            _started = true;
            return _stream.Start();
        }

        /// <summary>
        /// Stops tracking of data from stream, sending data to Realeys 
        /// </summary>
        /// <returns>true if operation successful</returns>
        public bool Stop()
        {
            _started = false;
            _processor.Send(new BaseEvent() { Type = "custom" }, true);
            return _stream.Stop();
        }

        /// <summary>
        /// Sends sdk init event 
        /// </summary>
        /// <param name="modelVersion">Nel model version https://developers.realeyesit.com/NEL/api-cpp.html#_CPPv4NK3nel7Tracker14get_model_nameEv </param>
        /// <param name="sdkVersion">Nel SDK version https://developers.realeyesit.com/NEL/api-cpp.html#_CPPv4N3nel7Tracker15get_sdk_versionEv </param>
        private void SendInitEvent(string modelVersion, string sdkVersion)
        {
            var initEvent = new BaseEvent()
            {
                Type = "init",
                SessionId = _sessionId,
                ParticipantId = _participantId,
                AppName = _appName,
                AppType = _appType,
                Args = new AppInitArgs()
                {
                    CountryCode = "HU",
                    NelModelVersion = modelVersion,
                    NelSdkVersion = sdkVersion,
                    PlatformName = "windows",
                    Result = "success"
                }
            };

            _processor.Send(initEvent);
        }

        /// <summary>
        /// sends data to event processor  from stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stream_DataTracked(object sender, EventArgs e)
        {
            if (_started)
            {
                if (e is NelDataTrackedEventArgs)
                {
                    var args = e as NelDataTrackedEventArgs;
                    var ev = new BaseEvent()
                    {
                        Type = "nel-data",
                        SessionId = _sessionId,
                        ParticipantId = _participantId,
                        AppName = _appName,
                        AppType = _appType,
                        Args = args.Data
                    };

                    _processor.Send(ev);
                }
            }
        }
    }
}
