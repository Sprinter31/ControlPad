using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CustomStreamDeck
{   
    public class ArduinoController
    {
        const string pattern = @"^\s*-?\d+\s*(,\s*-?\d+\s*){3}$";

        MainWindow mw;

        private SerialPort? _serialPort;

        /*private enum dic
        {
            Slider1,
            Slider2,
            Switch1,
            Switch2
        }*/

        Dictionary<string, int> values = new Dictionary<string, int>()
        {
            { "Slider1", 0 },
            { "Slider2", 0 },
            { "Switch1", 0 },
            { "Switch2", 0 }
        };

        public ArduinoController(MainWindow mainWindow)
        {
            mw = mainWindow;

            string port = ArduinoPortFinder.FindFirstArduinoPort();
            if (port != null)
            {
                _serialPort = new SerialPort(port, 9600);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
            }
            else
                MessageBox.Show("Arduino Port not found", "Custom Stream Deck", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                string line = _serialPort.ReadLine().Replace("\r", "");
                Regex.IsMatch(line, pattern);

                if (line == "") return;

                string[] inputs = line.Split(',');

                if (inputs.Length != values.Count) return;

                values["Slider1"] = int.Parse(inputs[0]);
                values["Slider2"] = int.Parse(inputs[1]);
                values["Switch1"] = int.Parse(inputs[2]);
                values["Switch2"] = int.Parse(inputs[3]);

                UpdateControls();
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"EX: {ex}");
            }
        }  
        
        private void UpdateControls()
        {
            if (Application.Current?.Dispatcher?.HasShutdownStarted == false)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    mw.Slider1.Value = values["Slider1"];
                    mw.Slider2.Value = values["Slider2"];
                });
            }
        }
    }
}
