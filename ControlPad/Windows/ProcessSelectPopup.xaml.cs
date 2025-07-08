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

namespace ControlPad.Windows
{
    /// <summary>
    /// Interaction logic for ProcessSelectPopup.xaml
    /// </summary>
    public partial class ProcessSelectPopup : Window
    {
        public List<string> ProcessNames { get; }

        public string SelectedProcessName { get; private set; } = string.Empty;


        public ProcessSelectPopup()
        {
            InitializeComponent();

            ProcessNames = Process.GetProcesses()
                              .OrderBy(p => p.ProcessName)
                              .Select(p => p.ProcessName)
                              .Distinct()
                              .ToList();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            SelectedProcessName = ProcessList.SelectedItem as string;
            DialogResult = true;
        }
    }
}
