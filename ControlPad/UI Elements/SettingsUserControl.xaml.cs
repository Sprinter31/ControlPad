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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlPad
{
    public partial class SettingsUserControl : UserControl
    {
        private bool _isInitialized = false;
        public SettingsUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            ThemeComboBox.SelectedIndex = Settings.SelectedThemeIndex;
            MinimizeToTrayCheckBox.IsChecked = Settings.MinimizeToSystemTray;
            StartWithWindowsCheckBox.IsChecked = Settings.StartWithWindows;
            _isInitialized = true;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized)
                return;

            ChangeAppTheme(ThemeComboBox.SelectedIndex);
            Settings.SelectedThemeIndex = ThemeComboBox.SelectedIndex;
        }

        private void MinimizeToTrayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
                return;

            if (MinimizeToTrayCheckBox.IsChecked != null)
                Settings.MinimizeToSystemTray = (bool)MinimizeToTrayCheckBox.IsChecked;
        }

        private void StartWithWindowsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
                return;

            if (StartWithWindowsCheckBox.IsChecked != null)
            {
                AutostartHelper.SetAutostart((bool)StartWithWindowsCheckBox.IsChecked);
                Settings.StartWithWindows = (bool)StartWithWindowsCheckBox.IsChecked;
            }              
        }

        public static void ChangeAppTheme(int index)
        {
            switch (index)
            {
                case 0:
                    Wpf.Ui.Appearance.ApplicationThemeManager.ApplySystemTheme();
                    break;
                case 1:
                    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
                    break;
                case 2:
                    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
                    break;
            }
        }
    }
}
