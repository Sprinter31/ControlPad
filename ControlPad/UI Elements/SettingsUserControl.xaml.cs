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
        public SettingsUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(ThemeComboBox.SelectedIndex)
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

        private void MinimizeToTrayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(MinimizeToTrayCheckBox.IsChecked != null)
                Settings.MinimizeToSystemTray = (bool)MinimizeToTrayCheckBox.IsChecked;
        }

        private void StartWithWindowsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(StartWithWindowsCheckBox.IsChecked != null)
                AutostartHelper.SetAutostart((bool)StartWithWindowsCheckBox.IsChecked);
        }
    }
}
