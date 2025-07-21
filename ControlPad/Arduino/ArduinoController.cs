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
        private MainUserControl MUC;
        private SerialPort _serialPort;
        private EventHandler eventHandler;
        private Dictionary<Control, int> values = new Dictionary<Control, int>();

        public ArduinoController(MainUserControl mainUserControl)
        {
            MUC = mainUserControl;
            eventHandler = new EventHandler(MUC);

            values = new Dictionary<Control, int>
            {
                { MUC.Slider1, 0 },
                { MUC.Slider2, 0 },
                /*{ mw.Slider3, 0 },
                { mw.Slider4, 0 },
                { mw.Slider5, 0 },
                { mw.Slider6, 0 },
                { mw.Switch1, 0 },
                { mw.Switch2, 0 },
                { mw.Switch3, 0 },
                { mw.Switch4, 0 },
                { mw.Switch5, 0 },
                { mw.Switch6, 0 },
                { mw.Switch7, 0 },
                { mw.Switch8, 0 },
                { mw.Switch9, 0 },
                { mw.Switch10, 0 },
                { mw.Switch11, 0 }*/
            };

            string port = ArduinoPortFinder.FindFirstArduinoPort();
            if (port != null)
            {
                _serialPort = new SerialPort(port, 9600);
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
                if (!Regex.IsMatch(line, @"^\s*-?\d+\s*(,\s*-?\d+\s*){3}$")) return;

                string[] inputs = line.Split(',');

                UpdateValues(inputs);
                MUC.Dispatcher.Invoke(() => eventHandler.Update(values));
            }
            catch (IOException ex)
            {
                OnSerialPortDisconnected(ex.ToString());
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
            /*values[mw.Slider3] = int.Parse(inputs[2]);
            values[mw.Slider4] = int.Parse(inputs[3]);
            values[mw.Slider5] = int.Parse(inputs[4]);
            values[mw.Slider6] = int.Parse(inputs[5]);
            values[mw.Switch1] = int.Parse(inputs[6]);
            values[mw.Switch3] = int.Parse(inputs[7]);
            values[mw.Switch4] = int.Parse(inputs[8]);
            values[mw.Switch5] = int.Parse(inputs[9]);
            values[mw.Switch6] = int.Parse(inputs[10]);
            values[mw.Switch7] = int.Parse(inputs[11]);
            values[mw.Switch8] = int.Parse(inputs[12]);
            values[mw.Switch9] = int.Parse(inputs[13]);
            values[mw.Switch10] = int.Parse(inputs[14]);
            values[mw.Switch11] = int.Parse(inputs[15]);*/
        }

        private void OnSerialPortDisconnected(string ex)
        {
            MessageBox.Show($"Arduino Port disconnected with the exception: {ex}", "Control Pad", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
