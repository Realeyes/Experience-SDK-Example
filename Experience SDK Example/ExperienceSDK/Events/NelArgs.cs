using System;
using System.Collections.Generic;
using System.Text;

namespace Experience_SDK_Example.ExperienceSDK.Events
{
    public class NelArgs : EvArg
    {
        public bool IsGoodQuality { get; set; }
        public bool BinaryAttention { get; set; }
        public bool BinaryHappy { get; set; }
        public bool BinaryConfusion { get; set; }
        public bool BinaryContempt { get; set; }
        public bool BinaryDisgust { get; set; }
        public bool BinaryEmpathy { get; set; }
        public bool BinaryFear { get; set; }
        public bool BinarySurprise { get; set; }
        public double Attention { get; set; }
        public double Happy { get; set; }
        public double Confusion { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Empathy { get; set; }
        public double Fear { get; set; }
        public double Surprise { get; set; }
    }
}
