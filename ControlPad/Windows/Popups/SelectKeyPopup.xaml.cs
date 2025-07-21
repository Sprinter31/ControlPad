using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace ControlPad
{
    /// <summary>
    /// Interaction logic for SelectKeyPopup.xaml
    /// </summary>
    public partial class SelectKeyPopup : FluentWindow
    {
        public SelectKeyPopup()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
