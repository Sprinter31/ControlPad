using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ControlPad
{
    public static class Settings
    {
        private static bool _trayIconMessageShown = false;
        private static bool _minimizeToSystemTray = true;
        private static bool _startWithWindows = true;

        static Settings()
        {
            Load();
        }

        public static bool TrayIconMessageShown
        {
            get => _trayIconMessageShown;
            set
            {
                _trayIconMessageShown = value;
                Save();
            }
        }

        public static bool MinimizeToSystemTray
        {
            get => _minimizeToSystemTray;
            set
            {
                _minimizeToSystemTray = value;
                Save();
            }
        }

        public static bool StartWithWindows
        {
            get => _startWithWindows;
            set
            {
                _startWithWindows = value;
                Save();
            }
        }

        private class Data
        {
            public bool TrayIconMessageShown { get; set; } = false;
            public bool MinimizeToSystemTray { get; set; } = true;
            public bool StartWithWindows { get; set; } = true;
        }

        private static void Load()
        {
            try
            {
                if (!File.Exists(DataHandler.SettingsPath))
                    return;

                string json = File.ReadAllText(DataHandler.SettingsPath);
                var data = JsonSerializer.Deserialize<Data>(json);
                if (data is null)
                    return;

                _trayIconMessageShown = data.TrayIconMessageShown;
                _minimizeToSystemTray = data.MinimizeToSystemTray;
                _startWithWindows = data.StartWithWindows;
            }
            catch
            {
                
            }
        }

        private static void Save()
        {
            try
            {
                var data = new Data
                {
                    TrayIconMessageShown = _trayIconMessageShown,
                    MinimizeToSystemTray = _minimizeToSystemTray,
                    StartWithWindows = _startWithWindows
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(DataHandler.SettingsPath, json);
            }
            catch
            {
                
            }
        }
    }
}