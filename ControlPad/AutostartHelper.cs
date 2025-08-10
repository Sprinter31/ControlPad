using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace ControlPad
{
    public static class AutostartHelper
    {
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Adds or removes the app from Windows startup (Registry).
        /// </summary>
        /// <param name="enable">true = add, false = remove</param>
        /// <param name="appName">
        /// Display name in Startup. Default: AppDomain.CurrentDomain.FriendlyName
        /// </param>
        /// <param name="executablePath">
        /// Path to the EXE. Default: current process executable.
        /// </param>
        public static void SetAutostart(bool enable, string? appName = null, string? executablePath = null)
        {
            if (!OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException("Registry-based autostart is only available on Windows.");

            appName ??= AppDomain.CurrentDomain.FriendlyName;

            // Resolve current EXE if no path is provided
            executablePath ??= Process.GetCurrentProcess().MainModule?.FileName
                                ?? throw new InvalidOperationException("Could not resolve the executable path.");

            // Normalize to absolute path and quote for spaces
            executablePath = Path.GetFullPath(executablePath);
            string command = $"\"{executablePath}\"";

            using var key =
                Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true)
                ?? Registry.CurrentUser.CreateSubKey(RunKeyPath, true)
                ?? throw new InvalidOperationException("Could not open/create the Run registry key.");

            if (enable)
            {
                key.SetValue(appName, command, RegistryValueKind.String);
            }
            else
            {
                // false => don't throw if the value doesn't exist
                key.DeleteValue(appName, false);
            }
        }

        /// <summary>
        /// Checks whether a startup entry exists for this app (and points to the same EXE).
        /// </summary>
        public static bool IsAutostartEnabled(string? appName = null, string? executablePath = null)
        {
            if (!OperatingSystem.IsWindows())
                return false;

            appName ??= AppDomain.CurrentDomain.FriendlyName;
            executablePath ??= Process.GetCurrentProcess().MainModule?.FileName ?? "";
            executablePath = Path.GetFullPath(executablePath);
            string expected = $"\"{executablePath}\"";

            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
            var value = key?.GetValue(appName) as string;

            return string.Equals(value, expected, StringComparison.OrdinalIgnoreCase);
        }
    }
}
