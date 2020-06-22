using System;
using System.Collections.Generic;
using System.Text;

namespace Experience_SDK_Example.ExperienceSDK.Events
{
    public class AppInitArgs : EvArg
    {
        public string CountryCode { get; set; }
        public string PlatformName { get; set; }
        public string NelSdkVersion { get; set; }
        public string NelModelVersion { get; set; }
        public string Result { get; set; }
    }
}
