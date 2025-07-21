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
using System.Collections.ObjectModel;
using NAudio.CoreAudioApi;

namespace ControlPad
{
    public partial class EditButtonCategoryWindow : FluentWindow
    {
        private int indexOfCategory;
        public EditButtonCategoryWindow(int indexOfCategory)
        {
            InitializeComponent();
            foreach (var buttonAction in DataHandler.ButtonCategories[indexOfCategory].ButtonActions)
            {
                var ButtonActionControl = CreateAction(buttonAction);
                ActionsContainer.Children.Add(ButtonActionControl);
                switch(buttonAction.ActionType.Type)
                {
                    case EActionType.MuteProcess:
                    case EActionType.OpenProcess:
                    case EActionType.OpenWebsite:
                    case EActionType.MuteMic:
                        ButtonActionControl.TextBlock.Text = $"{buttonAction.ActionType.Description}: {buttonAction.ActionPropertyDisplay}";
                        break;
                    case EActionType.KeyPress: break;
                }
            }
            this.indexOfCategory = indexOfCategory;
            ComboBox_Type.DisplayMemberPath = "Description";
            ComboBox_Type.ItemsSource = DataHandler.ActionTypes;
        }

        private void btn_AddAction_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Type.SelectedItem == null || string.IsNullOrEmpty(ComboBox_Type.SelectedItem.ToString())) return;

            ButtonAction buttonAction = new ButtonAction((ActionType)ComboBox_Type.SelectedItem);
            ActionsContainer.Children.Add(CreateAction(buttonAction));
            DataHandler.ButtonCategories[indexOfCategory].ButtonActions.Add(buttonAction);
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            var btn = (System.Windows.Controls.Button)sender;

            if (btn.Parent is Grid grid && grid.Parent is ButtonActionUserControl wrapper)
            {
                RemoveHandlers(wrapper);
                ActionsContainer.Children.Remove(wrapper);
                DataHandler.ButtonCategories[indexOfCategory].ButtonActions.Remove(wrapper.ButtonAction);
            }
        }

        private ButtonActionUserControl CreateAction(ButtonAction buttonAction)
        {
            var buttonActionControl = new ButtonActionUserControl(buttonAction);
            if(buttonAction.ActionType.Type == EActionType.MuteMainAudio) buttonActionControl.btn_Settings.IsEnabled = false;
            buttonActionControl.TextBlock.Text = buttonAction.ActionType.Description;
            AddHandlers(buttonActionControl);
            return buttonActionControl;
        }

        private void AddHandlers(ButtonActionUserControl control)
        {
            control.btn_Settings.Click += new RoutedEventHandler(btn_settings_Click);
            control.btn_Remove.Click += new RoutedEventHandler(btn_Remove_Click);
        }
            
        private void RemoveHandlers(ButtonActionUserControl control)
        {
            control.btn_Settings.Click -= btn_settings_Click;
            control.btn_Remove.Click -= btn_Remove_Click;
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            var btn = (System.Windows.Controls.Button)sender;
            var control = (ButtonActionUserControl)((Grid)btn.Parent).Parent;

            switch (control.ButtonAction.ActionType.Type)
            {
                case EActionType.MuteProcess:
                    {
                        var processDialog = new SelectProcessPopup { Owner = this };
                        processDialog.cb_Processes.Text = control.ButtonAction.ActionProperty as string ?? processDialog.cb_Processes.Text;

                        if (processDialog.ShowDialog() == true)
                        {
                            control.ButtonAction.ActionProperty = processDialog.SelectedProcessName;
                            control.ButtonAction.ActionPropertyDisplay = processDialog.SelectedProcessName;
                            control.TextBlock.Text = $"{control.ButtonAction.ActionType.Description}: {processDialog.SelectedProcessName}";
                        }
                        break;
                    }
                case EActionType.MuteMainAudio: break;
                case EActionType.MuteMic:
                    {
                        var micDialog = new SelectMicPopup { Owner = this };
                        micDialog.cb_Mics.Text = control.ButtonAction.ActionProperty as string ?? micDialog.cb_Mics.Text;

                        if (micDialog.ShowDialog() == true)
                        {
                            control.ButtonAction.ActionProperty = micDialog.SelectedMic?.DeviceFriendlyName;
                            control.ButtonAction.ActionPropertyDisplay = micDialog.SelectedMic?.DeviceFriendlyName;
                            control.TextBlock.Text = $"{control.ButtonAction.ActionType.Description}: {micDialog.SelectedMic?.DeviceFriendlyName}";
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
                            control.ButtonAction.ActionProperty = fileDialog.FileName;
                            control.ButtonAction.ActionPropertyDisplay = fileDialog.SafeFileName;
                            control.TextBlock.Text = $"{control.ButtonAction.ActionType.Description}: {fileDialog.SafeFileName}";
                        }
                        break;
                    }                   
                case EActionType.OpenWebsite: 
                    {
                        var websiteDialog = new EnterWebsitePopup { Owner = this };
                        websiteDialog.tb_WebsiteURL.Text = control.ButtonAction.ActionProperty as string ?? websiteDialog.tb_WebsiteURL.Text;

                        if (websiteDialog.ShowDialog() == true)
                        {
                            control.ButtonAction.ActionProperty = websiteDialog.URL;
                            control.ButtonAction.ActionPropertyDisplay = websiteDialog.DisplayURL;
                            control.TextBlock.Text = $"{control.ButtonAction.ActionType.Description}: {websiteDialog.DisplayURL}";
                        }
                        break;
                    }
                case EActionType.KeyPress:
                    {
                        var keyDialog = new EnterWebsitePopup { Owner = this };

                        if (keyDialog.ShowDialog() == true)
                        {
                            
                        }
                        break;
                    }
                default: break;
            }
        }

        private void FluentWindow_Closed(object sender, EventArgs e)
        {
            var buttonActions = new List<ButtonAction>();
            var toRemove = new List<ButtonActionUserControl>();

            foreach (ButtonActionUserControl control in ActionsContainer.Children)
            {
                if (control.ButtonAction.ActionProperty == null && control.ButtonAction.ActionType.Type != EActionType.MuteMainAudio)
                {
                    toRemove.Add(control);
                }
                else
                {
                    buttonActions.Add(control.ButtonAction);
                }
            }

            foreach (var control in toRemove)
            {
                RemoveHandlers(control);
                ActionsContainer.Children.Remove(control);
            }

            DataHandler.ButtonCategories[indexOfCategory].ButtonActions = buttonActions;
        }
    }
}
