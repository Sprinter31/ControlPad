using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ControlPad
{
    public class ArduinoController
    {
        private HomeUserControl MUC;
        private SerialPort _serialPort;
        private EventHandler eventHandler;

        public ArduinoController(HomeUserControl homeUserControl)
        {
            MUC = homeUserControl;
            eventHandler = new EventHandler(MUC);

            string port = ArduinoPortFinder.FindFirstArduinoPort();
            if (port != null)
            {
                _serialPort = new SerialPort(port, 115200);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
            }
            else
                MessageBox.Show("Arduino Port not found", "Control Pad", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
                try
                {
                    string line = _serialPort.ReadLine().Replace("\r", "");
                    string[] inputs = Regex.Split(line, ",");

                    if (inputs.Length < 16) return;

                    UpdateValues(inputs);

                    MUC.Dispatcher.BeginInvoke(() => eventHandler.Update(DataHandler.SliderValues, DataHandler.ButtonValues));

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

        private void UpdateValues(string[] inputs)
        {
            for (int i = 0; i < DataHandler.SliderValues.Count; i++)
                DataHandler.SliderValues[i] = (DataHandler.SliderValues[i].slider, int.Parse(inputs[i]));
            for (int i = 0; i < DataHandler.ButtonValues.Count; i++)
                DataHandler.ButtonValues[i] = (DataHandler.ButtonValues[i].button, int.Parse(inputs[i + 6]));
        }

        private void OnSerialPortDisconnected(string ex)
        {
            MessageBox.Show($"Arduino Port disconnected with the exception: {ex}", "Control Pad", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
