using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using SW = System.Windows;

namespace ControlPad
{
    public class AudioController
    {
        private MMDeviceEnumerator _enum;

        public AudioController()
        { 
            _enum = new MMDeviceEnumerator();           
        }
        private SessionCollection GetSessions()
        {
            using var device = _enum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return device.AudioSessionManager.Sessions;
        }

        public void SetProcessVolume(string processName, float volume)
        {
            var sessions = GetSessions();

            volume = Math.Clamp(volume, 0f, 1f);

            for (int i = 0; i < sessions?.Count; i++)
            {
                var session = sessions[i];

                int? pid = null;

                try { pid = (int)session.GetProcessID; }
                catch { continue; }

                if (pid == null) continue;

                try
                {
                    using var process = Process.GetProcessById(pid.Value);
                    if (string.Equals(process.ProcessName, processName, StringComparison.OrdinalIgnoreCase))
                    {
                        session.SimpleAudioVolume.Volume = volume;
                    }
                }
                // The process does not exist anymore
                catch { continue; }
            }
        }

        public void SetSystemVolume(float volume)
        {
            using var device = _enum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            volume = Math.Clamp(volume, 0f, 1f);

            if(device != null) device.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
        }

        public void SetMicVolume(float volume)
        {
            MMDevice mic = _enum.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            mic.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
        }
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
            using var device = _enum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
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
