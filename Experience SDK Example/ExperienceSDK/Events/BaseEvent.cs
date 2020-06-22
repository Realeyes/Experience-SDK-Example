using System;
using System.Collections.Generic;
using System.Text;

namespace Experience_SDK_Example.ExperienceSDK.Events
{
    public class BaseEvent
    {
        public string Type { get; set; }
        public string AbsoluteUtcTime { get; set; }
        public string AbsoluteLocalTime { get; set; }
        public string SessionId { get; set; }
        public string ParticipantId { get; set; }
        public string AppInstanceId { get; set; }
        public string AppName { get; set; }
        public string AppType { get; set; }
        public EvArg Args { get; set; }
        public string AccountHash { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
    }


    public abstract class EvArg
    {

    }
}
