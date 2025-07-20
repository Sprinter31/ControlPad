using System;
using System.Collections.Generic;
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
    public partial class ProcessActionSettingsPopup : FluentWindow
    {
        public string? ProcessName;
        public ProcessActionSettingsPopup()
        {
            InitializeComponent();
        }

        private void btn_SelectProcess_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectProcessPopup();
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                ProcessName = dialog.SelectedProcessName;
                TextBlock_ProcessName.Text = ProcessName;
            }
        }
    }
}
