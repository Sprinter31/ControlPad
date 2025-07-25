﻿using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class SelectProcessPopup : FluentWindow
    {       
        public string SelectedProcessName { get; private set; } = string.Empty;

        public SelectProcessPopup()
        {
            InitializeComponent();            
            cb_Processes.ItemsSource = Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle));
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Processes.SelectedItem is not Process proc) return;

            SelectedProcessName = proc.ProcessName;
            DialogResult = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
