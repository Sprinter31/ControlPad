﻿using NAudio.CoreAudioApi;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ControlPad
{
    public class AudioController
    {
        private MMDeviceEnumerator _enum;

        public AudioController()
        { 
            _enum = new MMDeviceEnumerator();           
        }

        public void SetProcessVolume(string processName, float volume)
        {
            using var device = _enum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            var sessions = device.AudioSessionManager.Sessions;

            volume = Math.Clamp(volume, 0f, 1f);

            List<int> processIds = Process.GetProcessesByName(processName).Select(c => c.Id).ToList(); // this might be slow

            for (int i = 0; i < sessions?.Count; i++)
            {
                var session = sessions[i];
                if (processIds.Contains((int)session.GetProcessID))
                {
                    session.SimpleAudioVolume.Volume = volume;
                }                    
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

        public void MuteProcess(string processName, bool mute)
        {

        }

        public void MuteSystem(bool mute)
        {

        }

        public void MuteMic(bool mute)
        {

        }

        public List<MMDevice> GetMics()
        {
            var mics = new List<MMDevice>();
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(
                DataFlow.Capture,
                DeviceState.Active
            );
           
            foreach (var device in devices)
            {
                mics.Add(device);
            }
            return mics;
        }
    }
}
