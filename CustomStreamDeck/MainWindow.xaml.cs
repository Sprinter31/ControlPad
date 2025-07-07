using System.Diagnostics;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CustomStreamDeck
{
    public partial class MainWindow : Window
    {                
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UpdateUISlider(Slider slider, int value) => slider.Value = value;
    }
}