using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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

namespace ControlPad
{
    /// <summary>
    /// Interaction logic for ProcessSelectPopup.xaml
    /// </summary>
    public partial class ProcessSelectPopup : Window
    {
        public string SelectedProcessName { get; private set; } = string.Empty;

        public ProcessSelectPopup()
        {
            InitializeComponent();             
            cb_Processes.ItemsSource = Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle));
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Processes.SelectedItem is not Process proc)
            {
                MessageBox.Show("Please select a valid process", "Control Pad");
                return;
            }

            SelectedProcessName = proc.ProcessName;
            DialogResult = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
