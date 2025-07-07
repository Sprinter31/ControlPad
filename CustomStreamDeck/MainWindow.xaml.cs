using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;

namespace CustomStreamDeck
{
    public partial class MainWindow : Window
    {
        private Forms.NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();

            notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.Icon = new Drawing.Icon(@"Resources\rofl.ico");
            notifyIcon.Text = "Custom Stream Deck";

            var contextMenu = new Forms.ContextMenuStrip();
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
            notifyIcon.ShowBalloonTip(5000, "Notice", "Custom Stream Deck minimized to system tray", ToolTipIcon.Info);
        }
    }
}