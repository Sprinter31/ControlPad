using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class SelectMicPopup : FluentWindow
    {
        AudioController audioController = new AudioController();
        public string? SelectedMicName { get; set; }
        public SelectMicPopup()
        {
            InitializeComponent();
            cb_Mics.DisplayMemberPath = "DeviceFriendlyName";
            cb_Mics.ItemsSource = audioController.GetMics();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Mics.SelectedItem is not MMDevice device) return;

            SelectedMicName = device.DeviceFriendlyName;
            DialogResult = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
