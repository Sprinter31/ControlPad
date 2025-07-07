using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using SW = System.Windows;

namespace CustomStreamDeck
{
    public class AudioController
    {
        private MMDeviceEnumerator devEnum;
        private MMDevice device;
        MMDevice mic;

        public AudioController()
        { 
            devEnum = new MMDeviceEnumerator();
            device = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            mic = devEnum.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        }

        public void SetProcessVolume(string processName, float volume)
        {
            var sessions = device.AudioSessionManager.Sessions;

            if (volume >= 1)
                volume = 1;
            else if(volume < 0)
                volume = 0;           

            for (int i = 0; i < sessions?.Count; i++)
            {
                var session = sessions[i];
                if (session.DisplayName == processName)
                    session.SimpleAudioVolume.Volume = volume;
            }
        }

        public void SetSystemVolume(float volume)
        {
            if (volume >= 1)
                volume = 1;
            else if (volume < 0)
                volume = 0;

            if (device != null)
                SW.Application.Current.Dispatcher.Invoke(() => device.AudioEndpointVolume.MasterVolumeLevelScalar = volume);
        }

        public void SetMicVolume(float volume) => mic.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
        public void AdjustProcessVolume(string processName, float adjustment) => SetProcessVolume(processName, GetProcessVolume(processName) + adjustment);
        public void AdjustSystemVolume(float adjustment) => SetSystemVolume(GetSystemVolume() + adjustment);
        public void AdjustMicVolume(float adjustment) => SetMicVolume(GetMicVolume() + adjustment);

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
            var sessions = device?.AudioSessionManager.Sessions;

            for (int i = 0; i < sessions?.Count; i++)
            {
                var session = sessions[i];
                if (session.DisplayName == processName)
                    return session.SimpleAudioVolume.Volume;
            }
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
