using System;
using System.Collections.Generic;
using System.Text;

namespace Experience_SDK_Example.ExperienceSDK.Events
{
    public class MeetingArgs : EvArg
    {
        public string EventName { get; set; }
        public string MeetingId { get; set; }
        public string MeetingName { get; set; }
        public string MeetingStatus { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantRole { get; set; }
        public bool IsCurrentParticipant { get; set; }
        public long ElapsedMeetingTime { get; set; }
        public int PlannedDuration { get; set; }
        public bool CameraMuted { get; set; }
        public bool MicrophoneMuted { get; set; }
    }
}
