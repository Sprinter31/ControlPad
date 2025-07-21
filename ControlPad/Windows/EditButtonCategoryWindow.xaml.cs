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
using System.Reflection.Metadata;

namespace ControlPad
{
    public partial class EditButtonCategoryWindow : FluentWindow
    {
        private int indexOfCategory;
        public EditButtonCategoryWindow(int indexOfCategory)
        {
            InitializeComponent();
            this.indexOfCategory = indexOfCategory;
            ComboBox_Type.DisplayMemberPath = "Description";
            ComboBox_Type.ItemsSource = DataHandler.ActionTypes;
        }

        private void btn_AddAction_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Type.SelectedItem == null || string.IsNullOrEmpty(ComboBox_Type.SelectedItem.ToString())) return;

            var buttonAction = CreateAction((ActionType)ComboBox_Type.SelectedItem);
            ActionsContainer.Children.Add(buttonAction);
            DataHandler.ButtonCategoriesTemp[indexOfCategory].ButtonActions.Add(buttonAction);
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            var btn = (System.Windows.Controls.Button)sender;

            if (btn.Parent is Grid grid && grid.Parent is ButtonAction wrapper)
            {
                RemoveHandlers(wrapper);
                ActionsContainer.Children.Remove(wrapper);
                DataHandler.ButtonCategoriesTemp[indexOfCategory].ButtonActions.Remove(wrapper);
            }
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
            var btn = (System.Windows.Controls.Button)sender;
            var buttonAction = (ButtonAction)((Grid)btn.Parent).Parent;

            switch (buttonAction.ActionType.Type)
            {
                case EActionType.MuteProcess:
                    {
                        var processDialog = new SelectProcessPopup { Owner = this };
                        processDialog.cb_Processes.Text = buttonAction.ActionProperty as string ?? processDialog.cb_Processes.Text;

                        if (processDialog.ShowDialog() == true)
                        {
                            buttonAction.ActionProperty = processDialog.SelectedProcessName;
                            buttonAction.TextBlock.Text = $"{buttonAction.ActionType.Description}: {processDialog.SelectedProcessName}";
                        }
                        break;
                    }
                case EActionType.MuteMainAudio: break;
                case EActionType.MuteMic:
                    {
                        var micDialog = new SelectMicPopup { Owner = this };
                        micDialog.cb_Mics.Text = buttonAction.ActionProperty as string ?? micDialog.cb_Mics.Text;

                        if (micDialog.ShowDialog() == true)
                        {
                            buttonAction.ActionProperty = micDialog.SelectedMic;
                            buttonAction.TextBlock.Text = $"{buttonAction.ActionType.Description}: {micDialog.SelectedMic?.DeviceFriendlyName}";
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

                        if (fileDialog.ShowDialog() == true)
                        {
                            buttonAction.ActionProperty = fileDialog.FileName;
                            buttonAction.TextBlock.Text = $"{buttonAction.ActionType.Description}: {fileDialog.SafeFileName}";
                        }
                        break;
                    }                   
                case EActionType.OpenWebsite: 
                    {
                        var websiteDialog = new EnterWebsitePopup { Owner = this };
                        websiteDialog.tb_WebsiteURL.Text = buttonAction.ActionProperty as string ?? websiteDialog.tb_WebsiteURL.Text;

                        if (websiteDialog.ShowDialog() == true)
                        {
                            buttonAction.ActionProperty = websiteDialog.URL;
                            buttonAction.TextBlock.Text = $"{buttonAction.ActionType.Description}: {websiteDialog.DisplayURL}";
                        }
                        break;
                    }
                case EActionType.KeyPress: break;
                default: break;
            }
        }          
    }
}
