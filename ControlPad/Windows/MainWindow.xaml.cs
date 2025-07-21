using ControlPad;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class MainWindow : FluentWindow
    {
        private ArduinoController arduinoController;
        private NotifyIcon notifyIcon;     
        private HomeUserControl _homeUserControl;
        private ManageSliderCategoriesUserControl _manageSliderCategoriesUserControl;
        private ManageButtonCategoriesUserControl _manageButtonCategoriesUserControl;

        public MainWindow()
        {
            InitializeComponent();
            _homeUserControl = new HomeUserControl(this);
            _manageSliderCategoriesUserControl = new ManageSliderCategoriesUserControl(this);
            _manageButtonCategoriesUserControl = new ManageButtonCategoriesUserControl(this);
            arduinoController = new ArduinoController(_homeUserControl);
            DataContext = this;                    
            CreateNotifyIcon();

            MainContentFrame.Navigate(_homeUserControl);
            SetActive(NVI_Home);           
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon.ShowBalloonTip(5000, "Notice", "Control Pad minimized to system tray", ToolTipIcon.Info);
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            notifyIcon = null;
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

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        private void NVI_Home_Click(object sender, RoutedEventArgs e)
        {
            if(!NVI_Home.IsActive)
            {
                MainContentFrame.Navigate(_homeUserControl);
                SetActive(NVI_Home);
            }            
        }


        private void NVI_EditMode_Click(object sender, RoutedEventArgs e)
        {
            if (NVI_Home.IsActive && NVI_EditMode.Icon is SymbolIcon symbolIconEditMode)
            {
                if (symbolIconEditMode.Symbol == SymbolRegular.CheckboxChecked24)
                {
                    symbolIconEditMode.Symbol = SymbolRegular.CheckboxUnchecked24;
                    _homeUserControl.SliderCell1.Visibility = Visibility.Hidden;
                    _homeUserControl.SliderCell2.Visibility = Visibility.Hidden;
                    _homeUserControl.SliderCell3.Visibility = Visibility.Hidden;
                    _homeUserControl.SliderCell4.Visibility = Visibility.Hidden;
                    _homeUserControl.SliderCell5.Visibility = Visibility.Hidden;
                    _homeUserControl.SliderCell6.Visibility = Visibility.Hidden;
                }
                else
                {
                    symbolIconEditMode.Symbol = SymbolRegular.CheckboxChecked24;
                    _homeUserControl.SliderCell1.Visibility = Visibility.Visible;
                    _homeUserControl.SliderCell2.Visibility = Visibility.Visible;
                    _homeUserControl.SliderCell3.Visibility = Visibility.Visible;
                    _homeUserControl.SliderCell4.Visibility = Visibility.Visible;
                    _homeUserControl.SliderCell5.Visibility = Visibility.Visible;
                    _homeUserControl.SliderCell6.Visibility = Visibility.Visible;
                }
            }
        }

        private void NVI_Slider_Categories_Click(object sender, RoutedEventArgs e)
        {
            if (!NVI_Slider_Categories.IsActive)
            {
                MainContentFrame.Navigate(_manageSliderCategoriesUserControl);
                SetActive(NVI_Slider_Categories);
            }
        }
        private void NVI_Button_Categories_Click(object sender, RoutedEventArgs e)
        {
            if (!NVI_Button_Categories.IsActive)
            {
                MainContentFrame.Navigate(_manageButtonCategoriesUserControl);
                SetActive(NVI_Button_Categories);
            }
        }
        private void NVI_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (!NVI_Settings.IsActive)
            {
                MainContentFrame.Navigate(_homeUserControl);
                SetActive(NVI_Settings);
            }
        }

        private void SetActive(NavigationViewItem item)
        {
            NVI_Home.IsActive = false;
            NVI_Slider_Categories.IsActive = false;
            NVI_Button_Categories.IsActive = false;
            NVI_Settings.IsActive = false;

            if (NVI_EditMode.Icon is SymbolIcon symbolIconEditMode)
            {
                symbolIconEditMode.Symbol = SymbolRegular.CheckboxUnchecked24;
            }
            if (NVI_Home.Icon is SymbolIcon symbolIconHome) symbolIconHome.Filled = false;
            if (NVI_Slider_Categories.Icon is SymbolIcon symbolIconCategories) symbolIconCategories.Filled = false;
            if (NVI_Button_Categories.Icon is SymbolIcon symbolIconButtonCategories) symbolIconButtonCategories.Filled = false;
            if (NVI_Settings.Icon is SymbolIcon symbolIconSettings) symbolIconSettings.Filled = false;

            if (item.Icon is SymbolIcon symbolIcon) symbolIcon.Filled = true;
            item.IsActive = true;
        }
    }   
}