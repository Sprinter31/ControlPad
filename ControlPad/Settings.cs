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
        private static bool _startWithWindows = false;
        private static bool _startMinimized = false;
        private static bool _minimizeToSystemTray = true;       
        private static int _selectedThemeIndex = 0;

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

        public static bool StartWithWindows
        {
            get => _startWithWindows;
            set
            {
                _startWithWindows = value;
                Save();
            }
        }

        public static bool StartMinimized
        {
            get => _startMinimized;
            set
            {
                _startMinimized = value;
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

        public static int SelectedThemeIndex
        {
            get => _selectedThemeIndex;
            set
            {
                _selectedThemeIndex = value;
                Save();
            }
        }

        private class Data
        {
            public bool TrayIconMessageShown { get; set; } = false;
            public bool StartWithWindows { get; set; } = false;
            public bool StartMinimized { get; set; } = true;
            public bool MinimizeToSystemTray { get; set; } = true;           
            public int SelectedThemeIndex { get; set; } = 0;
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
                _startWithWindows = data.StartWithWindows;
                _startMinimized = data.StartMinimized;
                _minimizeToSystemTray = data.MinimizeToSystemTray;               
                _selectedThemeIndex = data.SelectedThemeIndex;
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
                    StartWithWindows = _startWithWindows,
                    StartMinimized = _startMinimized,
                    MinimizeToSystemTray = _minimizeToSystemTray,                    
                    SelectedThemeIndex = _selectedThemeIndex,
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