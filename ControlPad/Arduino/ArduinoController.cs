using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ControlPad
{
    public static class ArduinoController
    {
        private static SerialPort? _serialPort;
        private static ManagementEventWatcher? _insertWatcher;
        private static ManagementEventWatcher? _removeWatcher;
        private static MainWindow _mainWindow;
        private static EventHandler _eventHandler;

        public static void Initialize(MainWindow mainWindow, EventHandler eventHandler)
        {
            _mainWindow = mainWindow;
            _eventHandler = eventHandler;

            _insertWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2")
            );
            _insertWatcher.EventArrived += async (s, e) => await TryOpenAsync();
            _insertWatcher.Start();

            _removeWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3")
            );
            _removeWatcher.EventArrived += (s, e) =>
            {
                if (_serialPort != null && !_serialPort.IsOpen)
                {
                    _mainWindow.Dispatcher.BeginInvoke(() =>
                    {
                        _mainWindow.BoardDisconnectedInfoBar.IsOpen = true;
                        _mainWindow.MainContentFrame.Navigate(_mainWindow.progressRing);
                    });
                }
            };
            _removeWatcher.Start();

            _ = TryOpenAsync();
        }

        private static async Task TryOpenAsync()
        {
            string? port = await Task.Run(() => ArduinoPortFinder.FindFirstArduinoPort());
            if (port != null)
            {
                try
                {
                    var sp = new SerialPort(port, 115200);
                    sp.DataReceived += SerialPort_DataReceived;
                    sp.Open();
                    _serialPort = sp;

                    await _mainWindow.Dispatcher.InvokeAsync(() =>
                    {
                        _mainWindow.BoardDisconnectedInfoBar.IsOpen = false;
                        _mainWindow.MainContentFrame.Navigate(_mainWindow._homeUserControl);
                    });
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.WriteLine($"Access denied to port: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EX: {ex}");
                }
            }
            else
            {
                await _mainWindow.Dispatcher.InvokeAsync(() =>
                {
                    _mainWindow.BoardDisconnectedInfoBar.IsOpen = true;
                    _mainWindow.MainContentFrame.Navigate(_mainWindow.progressRing);
                });
            }
        }

        public static void Dispose()
        {
            _insertWatcher?.Stop();
            _removeWatcher?.Stop();
            _serialPort?.Dispose();
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort == null || !_serialPort.IsOpen) return;

            try
            {
                string line = _serialPort.ReadLine().Replace("\r", "");
                string[] inputs = Regex.Split(line, ",");

                if (inputs.Length < 16) return;

                UpdateValues(inputs);

                _mainWindow._homeUserControl.Dispatcher.BeginInvoke(() => _eventHandler.Update(DataHandler.SliderValues, DataHandler.ButtonValues));
            }
            catch (IOException)
            {
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EX: {ex}");
            }
        }

        private static void UpdateValues(string[] inputs)
        {
            for (int i = 0; i < DataHandler.SliderValues.Count; i++)
                DataHandler.SliderValues[i] = (DataHandler.SliderValues[i].slider, int.Parse(inputs[i]));
            for (int i = 0; i < DataHandler.ButtonValues.Count; i++)
                DataHandler.ButtonValues[i] = (DataHandler.ButtonValues[i].button, int.Parse(inputs[i + DataHandler.SliderValues.Count]));
        }
    }
}
