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
            _isInitialized = true;
            cb_StartWithWindows.IsChecked = Settings.StartWithWindows;
            cb_StartMinimized.IsChecked = Settings.StartMinimized;
            cb_MinimizeToTray.IsChecked = Settings.MinimizeToSystemTray;
            ThemeComboBox.SelectedIndex = Settings.SelectedThemeIndex;
        }

        private void cb_StartWithWindows_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
                return;

            bool startWithWindows = cb_StartWithWindows.IsChecked == true;
            bool startMinimized = cb_StartMinimized.IsChecked == true;

            AutoStart.Set(startWithWindows, startMinimized);
            Settings.StartWithWindows = startWithWindows;

            GridStartMinimized.IsEnabled = startWithWindows;
        }

        private void cb_StartMinimized_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
                return;

            bool startMinimized = cb_StartMinimized.IsChecked == true;

            AutoStart.Set(true, startMinimized);
            Settings.StartMinimized = startMinimized;
        }      

        private void cb_MinimizeToTray_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
                return;

            Settings.MinimizeToSystemTray = cb_StartMinimized.IsChecked == true;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized)
                return;

            ChangeAppTheme(ThemeComboBox.SelectedIndex);
            Settings.SelectedThemeIndex = ThemeComboBox.SelectedIndex;
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
