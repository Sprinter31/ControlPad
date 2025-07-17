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
    public partial class SelectActionPopup : FluentWindow
    {
        public SelectActionPopup()
        {
            InitializeComponent();
            ComboBox_Type.ItemsSource = DataHandler.ActionTypes;
        }

        private void AddAction_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Type.SelectedItem == null || string.IsNullOrEmpty(ComboBox_Type.SelectedItem.ToString())) return;

            ActionsContainer.Children.Add(CreateAction(DataHandler.ActionTypes[DataHandler.ActionTypes.IndexOf(ComboBox_Type.SelectedItem.ToString())]));
        }

        private ButtonAction CreateAction(string actionType)
        {
            var action = new ButtonAction();
            action.Text.Text = actionType;
            AddHandlers(action);
            return action;
        }

        private void AddHandlers(ButtonAction action)
        {
            action.btn_Settings.Click += new RoutedEventHandler(btn_settings_Click);
            action.btn_Remove.Click += new RoutedEventHandler(btn_Remove_Click);
        }
            
        private void RemoveHandlers(ButtonAction action)
        {
            action.btn_Settings.Click -= btn_settings_Click;
            action.btn_Remove.Click -= btn_Remove_Click;
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            switch (ComboBox_Type.SelectedItem.ToString())
            {
                case "Mute Process":
                    var dialog = new ManageCategoriesWindow();
                    dialog.Owner = this;
                    bool? result = dialog.ShowDialog();
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

            if (btn.Parent is Grid grid && grid.Parent is ButtonAction wrapper)
            {
                RemoveHandlers(wrapper);
                ActionsContainer.Children.Remove(wrapper);
            }
        }
    }
}
