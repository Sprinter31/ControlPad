using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ControlPad
{
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;
        private ArduinoController arCo;
        public MainWindow()
        {
            InitializeComponent();
            arCo = new ArduinoController(this);
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon(@"Resources\logo.ico");
            notifyIcon.Visible = true;
            notifyIcon.Text = "Control Pad";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open", null, (s, e) => {
                this.Show();
                this.WindowState = WindowState.Normal;
                this.Activate();
            });

            contextMenu.Items.Add("Exit", null, (s, e) => {
                notifyIcon.Visible = false;
                System.Windows.Application.Current.Shutdown();
            });

            notifyIcon.ContextMenuStrip = contextMenu;

            notifyIcon.DoubleClick += (s, e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    this.Activate();
                });
            };
        
        }

        public void UpdateUISlider(Slider slider, int value) => slider.Value = value;

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon.ShowBalloonTip(5000, "Notice", "Control Pad minimized to system tray", ToolTipIcon.Info);
        }

        private void ManageSliderGroups_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CategoryDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}