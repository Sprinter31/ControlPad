using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using SW = System.Windows;

namespace CustomStreamDeck
{
    public class AudioController
    {
        private MMDeviceEnumerator devEnum;
        private MMDevice? device;

        public AudioController()
        { 
            devEnum = new MMDeviceEnumerator();
            device = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        public void SetProcessVolume(string processName, float volume)
        {
            SW.Application.Current.Dispatcher.Invoke(() => 
            {
                var sessions = device?.AudioSessionManager.Sessions;

                for (int i = 0; i < sessions?.Count; i++)
                {
                    var session = sessions[i];
                    if (session.DisplayName == processName)
                        session.SimpleAudioVolume.Volume = volume;
                }
            });
        }

        public void SetSystemVolume(float volume)
        {
            if(device != null)
                SW.Application.Current.Dispatcher.Invoke(() => device.AudioEndpointVolume.MasterVolumeLevelScalar = volume);
        }

        public void SetMicVolume(float volume)
        {

        }

        public void AdjustProcessVolume(string processName, float adjustment)
        {

        }

        public void AdjustSystemVolume(float adjustment)
        {

        }

        public void AdjustMicVolume(float adjustment)
        {

        }

        public void MuteProcess(string processName, bool mute)
        {

        }

        public void MuteSystem(bool mute)
        {

        }

        public void MuteMic(bool mute)
        {

        }

        public float GetProcessVolume(string processName)
        {
            return 0;
        }

        public float GetSystemVolume()
        {
            return 0;
        }

        public float GetMicVolume()
        {
            return 0;
        }
    }
}
