using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

namespace CustomStreamDeck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort _serialPort;
        public MainWindow()
        {
            InitializeComponent();
            _serialPort = new SerialPort("COM6", 9600);
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
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
    }
}