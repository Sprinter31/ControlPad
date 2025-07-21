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
        private Dictionary<Control, int> values = new Dictionary<Control, int>();

        public ArduinoController(HomeUserControl homeUserControl)
        {
            MUC = homeUserControl;
            eventHandler = new EventHandler(MUC);

            values = new Dictionary<Control, int>
            {
                { MUC.Slider1, 0 },
                { MUC.Slider2, 0 },
                { MUC.Slider3, 0 },
                { MUC.Slider4, 0 },
                { MUC.Slider5, 0 },
                { MUC.Slider6, 0 },
                { MUC.Switch1, 0 },
                { MUC.Switch2, 0 },
                { MUC.Switch3, 0 },
                { MUC.Switch4, 0 },
                { MUC.Switch5, 0 },
                { MUC.Switch6, 0 },
                { MUC.Switch7, 0 },
                { MUC.Switch8, 0 },
                { MUC.Switch9, 0 },
                { MUC.Switch10, 0 },
                { MUC.Switch11, 0 }
            };

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

                    MUC.Dispatcher.BeginInvoke(() => eventHandler.Update(values));

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
            values[MUC.Slider1] = int.Parse(inputs[0]);
            values[MUC.Slider2] = int.Parse(inputs[1]);
            values[MUC.Slider3] = int.Parse(inputs[2]);
            values[MUC.Slider4] = int.Parse(inputs[3]);
            values[MUC.Slider5] = int.Parse(inputs[4]);
            values[MUC.Slider6] = int.Parse(inputs[5]);
            values[MUC.Switch1] = int.Parse(inputs[6]);
            values[MUC.Switch3] = int.Parse(inputs[7]);
            values[MUC.Switch4] = int.Parse(inputs[8]);
            values[MUC.Switch5] = int.Parse(inputs[9]);
            values[MUC.Switch6] = int.Parse(inputs[10]);
            values[MUC.Switch7] = int.Parse(inputs[11]);
            values[MUC.Switch8] = int.Parse(inputs[12]);
            values[MUC.Switch9] = int.Parse(inputs[13]);
            values[MUC.Switch10] = int.Parse(inputs[14]);
            values[MUC.Switch11] = int.Parse(inputs[15]);
        }

        private void OnSerialPortDisconnected(string ex)
        {
            MessageBox.Show($"Arduino Port disconnected with the exception: {ex}", "Control Pad", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
