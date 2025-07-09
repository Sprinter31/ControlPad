using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;

namespace ControlPad
{
    public partial class MainWindow : Window
    {
        private bool closeFromX = false;
        private NotifyIcon notifyIcon;
        private ArduinoController arCo;
        public MainWindow()
        {
            InitializeComponent();

            arCo = new ArduinoController(this);
            CreateNotifyIcon();
            GlobalData.LoadCategories(GlobalData.CategoryPath);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(hwnd);
            source.AddHook(WndProc);    
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (msg == WM_SYSCOMMAND && wParam.ToInt32() == SC_CLOSE)
            {
                closeFromX = true;
            }

            return IntPtr.Zero;
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closeFromX)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon.ShowBalloonTip(5000, "Notice", "Control Pad minimized to system tray", ToolTipIcon.Info);
                closeFromX = false;
            }
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            notifyIcon = null;
        }      

        private void ManageSliderCategories_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ManageCategoriesWindow();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void CreateNotifyIcon()
        {
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

        private void cb_EditMode_Checked(object sender, RoutedEventArgs e)
        {
            Border1.Visibility = Visibility.Visible;
        }

        private void cb_EditMode_Unchecked(object sender, RoutedEventArgs e)
        {            
            Border1.Visibility = Visibility.Collapsed;
        }

        public void UpdateUISlider(Slider slider, int value) => slider.Value = value;
        private void Exit_Click(object sender, EventArgs e) => this.Close();
    }
}