using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Wpf.Ui.Controls;
using Microsoft.Win32;

namespace ControlPad.Windows
{
    public partial class SelectActionPopup : FluentWindow
    {
        public SelectActionPopup()
        {
            InitializeComponent();
            ComboBox_Type.DisplayMemberPath = "Description";
            ComboBox_Type.ItemsSource = DataHandler.ActionTypes;
        }

        private void AddAction_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Type.SelectedItem == null || string.IsNullOrEmpty(ComboBox_Type.SelectedItem.ToString())) return;
            ActionsContainer.Children.Add(CreateAction((ActionType)ComboBox_Type.SelectedItem));
        }

        private ButtonAction CreateAction(ActionType actionType)
        {
            var buttonAction = new ButtonAction(actionType);
            if(actionType.Type == EActionType.MuteMainAudio) buttonAction.btn_Settings.IsEnabled = false;
            buttonAction.TextBlock.Text = actionType.Description;
            AddHandlers(buttonAction);
            return buttonAction;
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
            switch (((ActionType)ComboBox_Type.SelectedItem).Type)
            {
                case EActionType.MuteProcess:
                    {
                        var processDialog = new SelectProcessPopup { Owner = this };
                        if (processDialog.ShowDialog() == true && sender is System.Windows.Controls.Button btn)
                        {
                            ((ButtonAction)((Grid)btn.Parent).Parent)
                                .TextBlock.Text = $"{DataHandler.ActionTypes[0].Description}: {processDialog.SelectedProcessName}";
                        }
                        break;
                    }
                case EActionType.MuteMainAudio: break;
                case EActionType.MuteMic:
                    {
                        var micDialog = new SelectMicPopup { Owner = this };
                        if (micDialog.ShowDialog() == true && sender is System.Windows.Controls.Button btn)
                        {
                            ((ButtonAction)((Grid)btn.Parent).Parent)
                                .TextBlock.Text = $"{DataHandler.ActionTypes[2].Description}: {micDialog.SelectedMicName}";
                        }
                        break;
                    }                   
                case EActionType.OpenProcess:
                    {
                        var fileDialog = new Microsoft.Win32.OpenFileDialog
                        {
                            Title = "Select an EXE",
                            Filter = "Executable (*.exe)|*.exe|All Files (*.*)|*.*",
                            CheckFileExists = true,
                            Multiselect = false,
                            DefaultExt = ".exe"
                        };

                        if (fileDialog.ShowDialog() == true && sender is System.Windows.Controls.Button btn)
                        {
                            ((ButtonAction)((Grid)btn.Parent).Parent)
                                .TextBlock.Text = $"{DataHandler.ActionTypes[3].Description}: {fileDialog.SafeFileName}";
                        }
                        break;
                    }                   
                case EActionType.OpenWebsite: 
                    {
                        var websiteDialog = new EnterWebsitePopup { Owner = this };
                        if (websiteDialog.ShowDialog() == true && sender is System.Windows.Controls.Button btn)
                        {
                            ((ButtonAction)((Grid)btn.Parent).Parent)
                                .TextBlock.Text = $"{DataHandler.ActionTypes[4].Description}: {websiteDialog.DisplayURL}";
                        }
                        break;
                    }
                case EActionType.KeyPress: break;
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
