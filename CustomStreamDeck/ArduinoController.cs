using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomStreamDeck
{   
    public class ArduinoController
    {
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

        public ArduinoController()
        {
            string port = ArduinoPortFinder.FindFirstArduinoPort();
            if (port != null)
            {
                _serialPort = new SerialPort(port, 9600);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
            }
            else
                MessageBox.Show("Arduino Port not found :(", "Custom Stream Deck", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = _serialPort.ReadLine().Replace("\r", "");
            string[] inputs = line.Split(',');

            values["Slider1"] = int.Parse(inputs[0]);
            values["Slider2"] = int.Parse(inputs[1]);

            Debug.WriteLine(values["Slider1"]);

           








            /*string output = line.Split('%')[0]; // everything before '%'
            Debug.WriteLine(output);
            float.TryParse(output, out float value);
            auCo.SetProcessVolume("Spotify", value * 0.01f);*/



        }
    }
}
