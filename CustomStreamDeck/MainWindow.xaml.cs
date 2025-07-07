using System.Diagnostics;
using System.IO.Ports;
using System.Windows;
using System.Windows.Forms;

namespace CustomStreamDeck
{
    public partial class MainWindow : Window
    {        
        AudioController auCo;
        ArduinoController ardCo;
        public MainWindow()
        {
            InitializeComponent();
            auCo = new AudioController();
            ardCo = new ArduinoController(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            auCo.AdjustProcessVolume("Spotify", -0.1f);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            auCo.AdjustProcessVolume("Spotify", 0.1f);
        }
    }
}