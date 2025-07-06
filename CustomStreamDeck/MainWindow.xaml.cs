using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace CustomStreamDeck
{
    public partial class MainWindow : Window
    {
        private SerialPort _serialPort;
        private AudioController auCo = new AudioController();
        public MainWindow()
        {
            InitializeComponent();
            _serialPort = new SerialPort("COM6", 9600);
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = _serialPort.ReadLine();

            string output = line.Split('%')[0]; // everything before '%'
            Debug.WriteLine(output);
            float.TryParse(output, out float value);
            auCo.SetProcessVolume("Spotify", value * 0.01f);
        }

        private void Btn_On_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(tb_Delay.Text, out int delay);
            _serialPort.WriteLine($"LED_ON:{delay}");
        }

        private void Btn_Off_Click(object sender, RoutedEventArgs e)
        {
            _serialPort.WriteLine("LED_OFF");
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _serialPort.WriteLine("LED_OFF");
        }       
    }
}