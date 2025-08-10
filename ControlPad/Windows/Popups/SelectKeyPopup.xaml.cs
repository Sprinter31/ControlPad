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
    public partial class SelectKeyPopup : FluentWindow
    {
        public ObservableCollection<KeyGroup> KeyGroups { get; } = new();
        public KeyCode SelectedKey { get; private set; }

        public SelectKeyPopup(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            BuildKeyGroups();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (TreeViewKeys.SelectedItem is KeyCode key)
            {
                SelectedKey = key;
                DialogResult = true;
            }
        }

        private void TreeViewKeys_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TreeViewKeys.SelectedItem is KeyCode key)
            {
                SelectedKey = key;
                DialogResult = true;
            }
        }

        private void BuildKeyGroups()
        {
            // Media
            KeyGroups.Add(new KeyGroup("Media", new[]
            {
                new KeyCode(0xB0u, "Next Track"),
                new KeyCode(0xB1u, "Previous Track"),
                new KeyCode(0xB2u, "Stop Media"),
                new KeyCode(0xB3u, "Play/Pause Media"),
                new KeyCode(0xADu, "Volume Mute"),
                new KeyCode(0xAEu, "Volume Down"),
                new KeyCode(0xAFu, "Volume Up"),
                new KeyCode(0xB4u, "Launch Mail"),
                new KeyCode(0xB5u, "Select Media"),
                new KeyCode(0xB6u, "Launch App 1"),
                new KeyCode(0xB7u, "Launch App 2"),
            }));

            // Browser
            KeyGroups.Add(new KeyGroup("Browser", new[]
            {
                new KeyCode(0xA6u, "Browser Back"),
                new KeyCode(0xA7u, "Browser Forward"),
                new KeyCode(0xA8u, "Browser Refresh"),
                new KeyCode(0xA9u, "Browser Stop"),
                new KeyCode(0xAAu, "Browser Search"),
                new KeyCode(0xABu, "Browser Favorites"),
                new KeyCode(0xACu, "Browser Home"),
            }));

            // Function (F1–F24)
            KeyGroups.Add(new KeyGroup(
                "Function",
                Enumerable.Range(0, 24)
                    .Select(i => new KeyCode((uint)(0x70 + i), $"F{i + 1}"))));

            // Navigation
            KeyGroups.Add(new KeyGroup("Navigation", new[]
            {
                new KeyCode(0x1Bu, "Esc"),
                new KeyCode(0x08u, "Backspace"),
                new KeyCode(0x09u, "Tab"),
                new KeyCode(0x0Du, "Enter"),
                new KeyCode(0x20u, "Space"),
                new KeyCode(0x2Cu, "Print Screen"),
                new KeyCode(0x13u, "Pause"),
                new KeyCode(0x2Du, "Insert"),
                new KeyCode(0x2Eu, "Delete"),
                new KeyCode(0x24u, "Home"),
                new KeyCode(0x23u, "End"),
                new KeyCode(0x21u, "Page Up"),
                new KeyCode(0x22u, "Page Down"),
                new KeyCode(0x25u, "Left"),
                new KeyCode(0x26u, "Up"),
                new KeyCode(0x27u, "Right"),
                new KeyCode(0x28u, "Down"),
            }));

            // Modifier/System
            KeyGroups.Add(new KeyGroup("Modifier/System", new[]
            {
                new KeyCode(0x10u, "Shift"),
                new KeyCode(0xA0u, "Left Shift"),
                new KeyCode(0xA1u, "Right Shift"),
                new KeyCode(0x11u, "Ctrl"),
                new KeyCode(0xA2u, "Left Ctrl"),
                new KeyCode(0xA3u, "Right Ctrl"),
                new KeyCode(0x12u, "Alt"),
                new KeyCode(0xA4u, "Left Alt"),
                new KeyCode(0xA5u, "Right Alt"),
                new KeyCode(0x5Bu, "Left Win"),
                new KeyCode(0x5Cu, "Right Win"),
                new KeyCode(0x5Du, "Context Menu"),
                new KeyCode(0x14u, "Caps Lock"),
                new KeyCode(0x90u, "Num Lock"),
                new KeyCode(0x91u, "Scroll Lock"),
                new KeyCode(0x5Fu, "Sleep"),
            }));

            // Numbers
            KeyGroups.Add(new KeyGroup(
                "Numeric",
                Enumerable.Range(0, 10)
                    .Select(i => new KeyCode((uint)(0x30 + i), i.ToString()))));

            // Alphabetic (A–Z)
            KeyGroups.Add(new KeyGroup(
                "Alphabetic (A–Z)",
                Enumerable.Range('A', 26)
                    .Select(c => new KeyCode((uint)c, ((char)c).ToString()))));

            // Numpad
            KeyGroups.Add(new KeyGroup("Numpad", new[]
            {
                new KeyCode(0x60u, "NumPad 0"),
                new KeyCode(0x61u, "NumPad 1"),
                new KeyCode(0x62u, "NumPad 2"),
                new KeyCode(0x63u, "NumPad 3"),
                new KeyCode(0x64u, "NumPad 4"),
                new KeyCode(0x65u, "NumPad 5"),
                new KeyCode(0x66u, "NumPad 6"),
                new KeyCode(0x67u, "NumPad 7"),
                new KeyCode(0x68u, "NumPad 8"),
                new KeyCode(0x69u, "NumPad 9"),
                new KeyCode(0x6Au, "NumPad *"),
                new KeyCode(0x6Bu, "NumPad +"),
                new KeyCode(0x6Cu, "NumPad Separator"),
                new KeyCode(0x6Du, "NumPad -"),
                new KeyCode(0x6Eu, "NumPad ."),
                new KeyCode(0x6Fu, "NumPad /"),
            }));
        }
    }
}