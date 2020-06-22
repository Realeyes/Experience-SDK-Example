using Experience_SDK_Example.ExperienceSDK;
using Experience_SDK_Example.ExperienceSDK.Events;
using System;
using System.Diagnostics;
using System.Threading;

namespace Experience_SDK_Example
{
    public class Program
    {

        static string  _currentParticipantId = "participant 1";
        static Stopwatch _stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            var tracker = Tracker.GetTracker("test", "console-test", "console app", _currentParticipantId);
            _stopwatch.Start();
            tracker.Start();
            BaseEvent joinMeetingEvent = CreateMeetingEvent("joinedToMeeting", _currentParticipantId);
            var rand = new Random();
            Thread.Sleep(rand.Next(0, 10000));
            tracker.TrackEvent("console app", CreateMeetingEvent("joinedToMeeting", _currentParticipantId)); // imitate event of joining to meeting 
            Thread.Sleep(rand.Next(0, 10000));
            tracker.TrackEvent("console app", CreateMeetingEvent("joinedToMeeting", "participant 2", "host")); //  imitate event of joining to meeting 
            Thread.Sleep(rand.Next(0, 10000));
            tracker.TrackEvent("console app", CreateMeetingEvent("contentSharingStarted", "participant 2", "host")); //  imitate event of  meeting 
            Thread.Sleep(rand.Next(0, 180000));
            tracker.TrackEvent("console app", CreateMeetingEvent("contentSharingFinished", "participant 2", "host")); //  imitate event of meeting 
           
            Thread.Sleep(10000);


            tracker.TrackEvent("console app", CreateMeetingEvent("leftMeeting", "participant 2", "host")); //  imitate event of leaving to meeting  
            Thread.Sleep(rand.Next(0, 10000));
            tracker.TrackEvent("console app", CreateMeetingEvent("leftMeeting", _currentParticipantId)); // imitate event of leaving to meeting


            tracker.Stop();
        }

        private static BaseEvent CreateMeetingEvent(string evName, string participantId, string role = "attendee")
        {
            return new BaseEvent()
            {
                Type = "meeting-event",
                Args = new MeetingArgs()
                {
                    EventName = evName,
                    MeetingId = "1234555466",
                    MeetingName = "API overview",
                    MeetingStatus = "started",
                    ParticipantId = participantId,
                    ParticipantRole = role,
                    IsCurrentParticipant = participantId == _currentParticipantId,
                    ElapsedMeetingTime = _stopwatch.ElapsedMilliseconds,
                    PlannedDuration = 30,
                    CameraMuted = false,
                    MicrophoneMuted = true,
                }
            };
        }
    }
}
