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

namespace ControlPad.Windows
{
    public partial class SelectFunctionPopup : FluentWindow
    {
        public SelectFunctionPopup()
        {
            InitializeComponent();
            ComboBox_Type.ItemsSource = DataHandler.FunctionTypes;
        }

        private void AddFunction_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Type.SelectedItem == null || string.IsNullOrEmpty(ComboBox_Type.SelectedItem.ToString())) return;

            FunctionsContainer.Children.Add(CreateFunction(DataHandler.FunctionTypes[DataHandler.FunctionTypes.IndexOf(ComboBox_Type.SelectedItem.ToString())]));
        }

        private ButtonFunction CreateFunction(string functionType)
        {
            var function = new ButtonFunction();
            function.Text.Text = functionType;
            AddHandlers(function);
            return function;
        }

        private void AddHandlers(ButtonFunction function)
        {
            function.btn_Settings.Click += new RoutedEventHandler(btn_settings_Click);
            function.btn_Remove.Click += new RoutedEventHandler(btn_Remove_Click);
        }
            
        private void RemoveHandlers(ButtonFunction function)
        {
            function.btn_Settings.Click -= btn_settings_Click;
            function.btn_Remove.Click -= btn_Remove_Click;
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            switch (ComboBox_Type.SelectedItem.ToString())
            {
                case "Mute Process":
                    var dialog = new ManageCategoriesWindow();
                    dialog.Owner = this;
                    dialog.ShowDialog();
                    break;
                case "Mute Main Audio Stream": break;
                case "Mute Microphone": break;
                case "Open Process": break;
                case "Open Website": break;
                case "Key Press": break;
                default: break;
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            var btn = (System.Windows.Controls.Button)sender;

            if (btn.Parent is Grid grid && grid.Parent is ButtonFunction wrapper)
            {
                RemoveHandlers(wrapper);
                FunctionsContainer.Children.Remove(wrapper);
            }
        }
    }
}
