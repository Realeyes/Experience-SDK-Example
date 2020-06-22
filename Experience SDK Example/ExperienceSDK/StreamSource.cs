using Experience_SDK_Example.ExperienceSDK.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Experience_SDK_Example.ExperienceSDK
{
    /// <summary>
    /// The class just immitating stream processing with NEL SDK
    /// </summary>
    internal class StreamSource
    {
        ushort _fps;
        Task _nelTrackerImitataion = null;

        internal StreamSource(ushort fps = 30)
        {
            _fps = fps;

            // should be retreived from =>  https://developers.realeyesit.com/NEL/api-cpp.html#_CPPv4N3nel7Tracker15get_sdk_versionEv
            NelSdkVersion = "1.0.0";

            // should be retreived from =>   https://developers.realeyesit.com/NEL/api-cpp.html#_CPPv4NK3nel7Tracker14get_model_nameEv
            NelModelVersion = "Algorithm 4.4.1";
        }

        public string NelSdkVersion { get; private set; }
        public string NelModelVersion { get; private set; }

        public event EventHandler DataTracked;

        protected virtual void OnDataTracked(NelDataTrackedEventArgs e)
        {
            EventHandler handler = DataTracked;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// starts tracking stream with NelSDK
        /// </summary>
        /// <returns> returns 'true' if operation succesfull</returns>
        internal bool Start()
        {
            if (_nelTrackerImitataion == null) 
            {
                _nelTrackerImitataion = new Task(GenerateRandomEmotions);
                _nelTrackerImitataion.Start();
            }
            return true;
        }

        /// <summary>
        /// Stops stream tracking 
        /// </summary>
        /// <returns> returns 'true' if operation succesfull</returns>
        internal bool Stop()
        {
            if (_nelTrackerImitataion == null)
            {
                _nelTrackerImitataion = null;
            }

            return true;
        }

        private void GenerateRandomEmotions()
        {
            var rand = new Random();
            while (_nelTrackerImitataion != null)
            {
                var ev = new NelDataTrackedEventArgs();
                ev.Data = new NelArgs()
                {
                    IsGoodQuality = true,
                    Attention = rand.Next(0, 1000000) / 1000000.0,
                    Happy = rand.Next(0, 1000000) / 1000000.0,
                };
                ev.Data.BinaryAttention = ev.Data.Attention > 0.5;
                ev.Data.BinaryHappy = ev.Data.Happy > 0.5;
                OnDataTracked(ev);

                Thread.Sleep(1000 / _fps);
            }
        }
    }

    public class NelDataTrackedEventArgs : EventArgs
    {
        public NelArgs Data { get; set; }
    }
}
